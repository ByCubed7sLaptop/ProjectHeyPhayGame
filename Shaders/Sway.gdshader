shader_type canvas_item;

uniform float speed = 1.0;
uniform float amount = 1.0;

void vertex(){
	VERTEX.x += amount * sin(speed * TIME * 1.25 + UV.y) * ( 1.0 - UV.y) * 0.2;
	VERTEX.y += amount * cos(speed * TIME * 0.45 + UV.y) * ( 1.0 - UV.y) * 0.15;
}