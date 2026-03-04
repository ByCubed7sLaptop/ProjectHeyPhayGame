using Godot;
using System;

[Tool]
public partial class AnimatedTextureRect : TextureRect
{
	[Export] public Godot.Collections.Array<Texture2D> textures;
	[Export] public float animationSpeed = 10f;

	private float currentFrame = 0f;

	public override void _Process(double delta)
	{
		if (textures is null)
			return;

		if (textures.Count == 0)
			return;

		currentFrame += animationSpeed * (float)delta;

		int frameIndex = (int)currentFrame % textures.Count;
		Texture = textures[frameIndex];
	}
}
