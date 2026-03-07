shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture, filter_linear_mipmap;
uniform float blur_amount : hint_range(0.0, 10.0) = 3.0;
uniform int blur_samples : hint_range(1, 8) = 4;

// Gaussian Blur Sampling
vec3 blur_screen(vec2 screen_uv, vec2 pixel_size) {
    vec3 color = vec3(0.0);
    float total_weight = 0.0;
    
    for (int x = -blur_samples; x <= blur_samples; x++) {
        for (int y = -blur_samples; y <= blur_samples; y++) {
            vec2 offset = vec2(float(x), float(y)) * pixel_size * blur_amount;
            float weight = 1.0 / (1.0 + float(x * x + y * y));
            color += texture(screen_texture, screen_uv + offset).rgb * weight;
            total_weight += weight;
        }
    }
    
    return color / total_weight;
}

void fragment() {
    
    // Apply Screen Blur (Frosted Glass Effect)
    vec3 blurred_bg = blur_screen(SCREEN_UV, SCREEN_PIXEL_SIZE);
    
    COLOR = vec4(blurred_bg, 1.0);
}
