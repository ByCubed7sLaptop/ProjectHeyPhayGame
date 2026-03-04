using Godot;
using System;
using System.Diagnostics.Metrics;

public partial class EncounterBody : CharacterBody2D
{
	[Export] private EncounterResource Resource { get; set; }
	[Export] public float Speed = 30.0f;

	private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private bool directionRight = false;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		velocity.X = Speed;
		if (directionRight) velocity.X = -velocity.X;

		// Flip when hitting a wall
		if (IsOnWall()) 
			directionRight = GetWallNormal().X < 0;

		Velocity = velocity;
		MoveAndSlide();
	}

	/// <summary>
	/// Get the resource the encounter has. Returns a duplicate so that changes aren't reflected for every battle.
	/// </summary>
	public EncounterResource GetResource()
    {
		return Resource.Duplicate(true) as EncounterResource;
    }
}
