shader_type canvas_item;

// Screen-space shader with Brightness, Contrast and HSL controls

uniform sampler2D screen_texture : hint_screen_texture, filter_linear_mipmap;

uniform float brightness = 0.8;
uniform float contrast = 1.5;

// HSL controls
uniform float hue_shift : hint_range(-1.0, 1.0) = 0.0;      // -1 to 1 (full rotation)
uniform float saturation : hint_range(0.0, 2.0) = 1.0;      // 1 = normal
uniform float lightness : hint_range(-1.0, 1.0) = 0.0;      // -1 dark, +1 light

// --- RGB <-> HSL conversion helpers ---

vec3 rgb_to_hsl(vec3 color) {
    float r = color.r;
    float g = color.g;
    float b = color.b;

    float maxc = max(max(r, g), b);
    float minc = min(min(r, g), b);
    float h = 0.0;
    float s = 0.0;
    float l = (maxc + minc) * 0.5;

    float d = maxc - minc;

    if (d != 0.0) {
        s = l > 0.5 ? d / (2.0 - maxc - minc) : d / (maxc + minc);

        if (maxc == r)
            h = (g - b) / d + (g < b ? 6.0 : 0.0);
        else if (maxc == g)
            h = (b - r) / d + 2.0;
        else
            h = (r - g) / d + 4.0;

        h /= 6.0;
    }

    return vec3(h, s, l);
}

float hue_to_rgb(float p, float q, float t) {
    if (t < 0.0) t += 1.0;
    if (t > 1.0) t -= 1.0;
    if (t < 1.0/6.0) return p + (q - p) * 6.0 * t;
    if (t < 1.0/2.0) return q;
    if (t < 2.0/3.0) return p + (q - p) * (2.0/3.0 - t) * 6.0;
    return p;
}

vec3 hsl_to_rgb(vec3 hsl) {
    float h = hsl.x;
    float s = hsl.y;
    float l = hsl.z;

    float r, g, b;

    if (s == 0.0) {
        r = g = b = l;
    } else {
        float q = l < 0.5 ? l * (1.0 + s) : l + s - l * s;
        float p = 2.0 * l - q;

        r = hue_to_rgb(p, q, h + 1.0/3.0);
        g = hue_to_rgb(p, q, h);
        b = hue_to_rgb(p, q, h - 1.0/3.0);
    }

    return vec3(r, g, b);
}

// --- Main ---

void fragment() {
    vec3 c = textureLod(screen_texture, SCREEN_UV, 0.0).rgb;

    // Brightness & contrast (your original logic)
    c = mix(vec3(0.0), c, brightness);
    c = mix(vec3(0.5), c, contrast);

    // Convert to HSL
    vec3 hsl = rgb_to_hsl(c);

    // Apply HSL adjustments
    hsl.x = fract(hsl.x + hue_shift);      // Hue wraparound
    hsl.y *= saturation;                   // Saturation scale
    hsl.z = clamp(hsl.z + lightness, 0.0, 1.0);

    // Convert back to RGB
    c = hsl_to_rgb(hsl);

    COLOR.rgb = c;
}