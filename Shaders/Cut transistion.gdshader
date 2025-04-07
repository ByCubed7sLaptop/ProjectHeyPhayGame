shader_type canvas_item;

uniform float delta = 0.0;
uniform vec4 color: source_color = vec4(1);
uniform vec2 aspect_ratio = vec2(1);

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

// Function to rotate a 2D vector by a specific angle
vec2 rotateVector2(vec2 vector, float angle) {
    // Calculate the sine and cosine of the angle
    float s = sin(angle);
    float c = cos(angle);

    // Perform the rotation transformation
    return vec2(vector.x * c - vector.y * s, vector.x * s + vector.y * c);
}

vec4 transition(sampler2D input, vec2 uv, vec2 screen_pixel_size) {
        vec2 rotateUv = rotateVector2(uv.xy - 0.5, (1.0-delta)*TAU*1.0) + 0.5;
    
  float y = 0.5 + (rotateUv.y-0.5) / (1.0-pow(clamp(delta*1.1, 0.0, 1.0), 1.0f));
  if (y < 0.0 || y > 1.0) {
    return color;
  }
  else {
    return vec4(0.0);
  }
}

void fragment() {
	COLOR = transition(TEXTURE, UV, SCREEN_PIXEL_SIZE);
}