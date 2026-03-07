shader_type canvas_item;

uniform float radius = 0.5;
uniform float edge = 0.01;

group_uniforms halfShade;
uniform vec2 halfShade_pivot = vec2(0.5f);
uniform float halfShade_rotation = 0.0f;

group_uniforms outline;
uniform vec4 outline_color : source_color = vec4(0.0f, 0.0f, 0.0f, 1.0f);
uniform vec4 outline_color2 : source_color = vec4(0.1f, 0.1f, 0.1f, 1.0f);
uniform float outline_offset = 0.04f;
uniform float outline_circle = 0.04f;



void vertex() {
	VERTEX += (UV * 2.0 - 1.0) * outline_offset;
}

bool hasContraryNeighbour(vec2 uv, vec2 texture_pixel_size, sampler2D texture, float width, int pattern) {
	for (float i = -ceil(width); i <= ceil(width); i++) {
		float x = abs(i) > width ? width * sign(i) : i;
		float offset;
		
		if (pattern == 0) {
			offset = width - abs(x);
		} else if (pattern == 1) {
			offset = floor(sqrt(pow(width + 0.5, 2) - x * x));
		} else if (pattern == 2) {
			offset = width;
		}
		
		for (float j = -ceil(offset); j <= ceil(offset); j++) {
			float y = abs(j) > offset ? offset * sign(j) : j;
			vec2 xy = uv + texture_pixel_size * vec2(x, y);
			
			if ((xy != clamp(xy, vec2(0.0), vec2(1.0)) || texture(texture, xy).a <= 0.0) == false) {
				return true;
			}
		}
	}
	
	return false;
}

float halfShade(vec2 uv, vec2 pivot, float rotation) {
    // move pivot to origin
    uv -= pivot;

    // rotation matrix
    float s = sin(rotation);
    float c = cos(rotation);
    mat2 rot = mat2(vec2(s, -c), vec2(c, s));

    uv = rot * uv;

    // shift dividing line
    float mask = uv.x > 0.0f ? 0.0 : 1.0;
    return mask;
}

void fragment() {
	
	
    vec4 tex = texture(TEXTURE, UV);

	vec2 uv = UV;
	if (true) {
		vec2 texture_pixel_size = vec2(1.0) / (vec2(1.0) / TEXTURE_PIXEL_SIZE + vec2(outline_offset * 2.0));
		
		uv = (uv - texture_pixel_size * outline_offset) * TEXTURE_PIXEL_SIZE / texture_pixel_size;
		
		if (uv != clamp(uv, vec2(0.0), vec2(1.0))) {
			COLOR.a = 0.0;
		} else {
			COLOR = texture(TEXTURE, uv);
		}
	} else {
		COLOR = texture(TEXTURE, uv);
	}



	if (hasContraryNeighbour(uv, TEXTURE_PIXEL_SIZE, TEXTURE, outline_offset, 1)) {
		tex = mix(outline_color, tex, tex.a);
	}
	
    float d = distance(uv, vec2(0.5));
    float mask = 1.0 - smoothstep(radius - edge, radius, d);
	
	float circle2 = 1.0f - smoothstep(radius - edge+outline_circle, radius+outline_circle, d) - mask;
	
	mask = max(mask, halfShade(uv, halfShade_pivot, halfShade_rotation));
	mask = max(0.0f, min(1.0f, mask));
	
	float circle = 1.0f - smoothstep(radius - edge+outline_circle, radius+outline_circle, d) - mask;
	circle = max(0.0f, min(1.0f, circle));
	
	vec4 result = mix(tex * mask, circle * outline_color, circle);
	
	result = mix(tex * mask, circle * outline_color, circle);
	result = mix(circle2 * outline_color2, result, result.a);
	
	
	COLOR = result;
}


