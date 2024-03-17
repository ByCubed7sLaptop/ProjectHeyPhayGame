using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	
	[Export] public float Speed = 100.0f; // Max speed
	[Export] public float Acceleration = 0.15f; // Time to max speed

	[Export] public float JumpVelocity = 300.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity
		if (!IsOnFloor()) velocity.Y += gravity * (float)delta;

		// Handle Jump
		if (Input.IsActionPressed("ui_accept") && IsOnFloor())
			velocity.Y = -JumpVelocity;

		// TODO: Replace UI actions with custom gameplay actions.
		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		velocity.X = Mathf.Clamp(velocity.X, -Speed, Speed);

		// Accelerate and Deaccelerate
		velocity.X = Mathf.Lerp(Velocity.X, velocity.X, 0.15f);

		Velocity = velocity;
		MoveAndSlide();
	}
}
