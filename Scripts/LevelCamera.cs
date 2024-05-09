using Godot;
using System;

public partial class LevelCamera : Camera2D
{
	[Export] public CharacterBody2D Target;

	[Export] public bool horizontal;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Target is null) return;


		// Follow the target if below or moving down
		if (Target.Position.Y >= Position.Y)
			Position = Target.Position;

		// Move to target (more slowler) only if on the floor, no matter what Y level
		else if (Target.IsOnFloor())
			Position = new Vector2(
				Target.Position.X,
				(float)Mathf.Lerp(Position.Y, Target.Position.Y, 0.2f)
			);

		// Otherwise, Lock Y and only move to X of target
		else Position = Position with { X = Target.Position.X };

	}
}
