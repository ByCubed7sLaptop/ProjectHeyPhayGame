shader_type canvas_item;

uniform highp vec2 offset;
uniform highp float speed : hint_range(0, 1) = 1.;

vec4 effect(sampler2D tex, vec2 texture_coords) {
	vec4 texel = texture(tex, texture_coords);
	vec2 uv = texture_coords;

	vec2 adjusted_uv = uv - vec2(0.5, 0.5);
	
	float low = min(texel.r, min(texel.g, texel.b));
    float high = max(texel.r, max(texel.g, texel.b));
	float delta = min(high, max(0.5, 1. - low));
	
	vec2 foil = vec2(TIME / (1. / speed) + offset.x, offset.y);
	
	float fac = max(min(2. * sin((length(90. * adjusted_uv) + foil.r * 2.) + 3. * (1. + 0.8 * cos(length(113.1121 * adjusted_uv) - foil.r * 3.121))) - 1. - max(5. - length(90. * adjusted_uv), 0.), 1.), 0.);
	vec2 rotater = vec2(cos(foil.r * 0.1221), sin(foil.r * 0.3512));
	float angle = dot(rotater, adjusted_uv) / (length(rotater) * length(adjusted_uv));
	float fac2 = max(min(5. * cos(foil.g * 0.3 + angle * 3.14*(2.2 + 0.9*sin(foil.r * 1.65 + 0.2 * foil.g))) - 4. - max(2.-length(20.*adjusted_uv), 0.), 1.), 0.);
	float fac3 = 0.3 * max(min(2. * sin(foil.r * 5. + uv.x * 3. + 3. * (1. + 0.5 * cos(foil.r * 7.))) - 1., 1.), -1.);
	float fac4 = 0.3 * max(min(2. * sin(foil.r * 6.66 + uv.y * 3.8 + 3. * (1. + 0.5 * cos(foil.r * 3.414))) - 1., 1.), -1.);

	float maxfac = max(max(fac, max(fac2, max(fac3, max(fac4, 0.0)))) + 2.2 * (fac + fac2 + fac3 + fac4), 0.);

	texel.r = texel.r - delta + delta * maxfac * 0.3;
	texel.g = texel.g - delta + delta * maxfac * 0.3;
	texel.b = texel.b + delta * maxfac * 1.9;
	texel.a = min(texel.a, 0.3 * texel.a + 0.9 * min(0.5, maxfac * 0.1));

	return texel;
}

void fragment() {
	COLOR *= effect(TEXTURE, UV.xy);
}