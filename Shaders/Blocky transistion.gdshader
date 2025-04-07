shader_type canvas_item;

uniform float delta = 0.0;
uniform vec4 color: source_color = vec4(1);
uniform vec2 aspect_ratio = vec2(1);

uniform float resolution = 5.0;

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

vec4 transition(sampler2D input, vec2 uv, vec2 screen_pixel_size) {
    vec2 lowresxy = vec2(
    	floor(uv.x * resolution * aspect_ratio.x),
    	floor(uv.y * resolution * aspect_ratio.y)
    );
    
	bool isColored = delta > rand(lowresxy);
	
	vec4 grid_result = isColored ? texture(input, uv) : vec4(0.0);
	vec4 fragColor = grid_result * color;
	
	return fragColor;
}



void fragment() {
	COLOR = transition(TEXTURE, UV, SCREEN_PIXEL_SIZE);
}