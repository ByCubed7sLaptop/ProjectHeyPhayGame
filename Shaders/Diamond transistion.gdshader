shader_type canvas_item;

uniform vec4 diamond_color: source_color = vec4(1.0);

uniform float aspect_ratio = 1.5;
uniform float diamonds_count = 10.0;
uniform float delta = 0.0;

const float _min = -1.0;
const float _max = 1.0;

vec4 transition(vec2 uv, sampler2D input) {
	vec3 uv_grid = fract(vec3(uv, 0.0) * vec3(aspect_ratio * diamonds_count, diamonds_count, 0.0));
	float grid = abs(uv_grid.x - 0.5) + abs(uv_grid.y - 0.5);
	vec3 raw_cam_image = vec3(uv.y + delta*2.0-1.0);
	vec3 grayscale;
	{
		float max1 = max(raw_cam_image.r, raw_cam_image.g);
		float max2 = max(max1, raw_cam_image.b);
		grayscale = vec3(max(max1, max2));
	}
	vec3 clamped = clamp(grayscale, vec3(_min), vec3(_max));

	bool black_dot_grid = grid > dot(vec3(1.0) - clamped, vec3(0.333333));

	vec4 texture_uv = texture(input, uv);
	vec4 grid_result = black_dot_grid ? texture_uv : vec4(0.0);

	vec4 color = grid_result * diamond_color;
	
	return color;
}



void fragment() {
	COLOR = transition(UV, TEXTURE);
}