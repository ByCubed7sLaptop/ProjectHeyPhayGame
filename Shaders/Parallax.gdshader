shader_type canvas_item;

uniform sampler2D texture1;
uniform vec2 scale = vec2(1);
uniform vec2 position = vec2(0);
uniform vec2 expo = vec2(1);

varying vec2 UVSCALED;
void vertex() {
    vec2 scaled_uv = UV * scale.xy + position.xy * 1.0/expo;
    UVSCALED = scaled_uv;
}

void fragment() {
	
	vec2 uv = vec2(mod(UVSCALED.x, 1.0), mod(UVSCALED.y, 1.0));
	
	if (mod(UVSCALED.x, 2.0) > 1.0)
		uv.x = 1.0-uv.x;
	if (mod(UVSCALED.y, 2.0) > 1.0)
		uv.y = 1.0-uv.y;
	
    COLOR = texture(texture1, uv);
}