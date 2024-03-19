using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	[Export] public Area2D Hitbox;

	[ExportCategory("Movement")]
	[Export] public float Speed = 100.0f; // Max speed
	[Export] public float Acceleration = 0.15f; // Time to max speed

	[Export] public float JumpVelocity = 300.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public EventHandler<IEnemy> OnHitEnemy;

	public override void _Ready()
	{
		Hitbox.Connect(Area2D.SignalName.BodyEntered, new Callable(this, nameof(OnEnemyHitPlayer)));

		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity
		if (!IsOnFloor()) velocity.Y += gravity * (float)delta;

		// Handle Jump
		if (Input.IsActionPressed("player_jump") && IsOnFloor())
			velocity.Y = -JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("player_left", "player_right", "player_up", "player_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		// Clamp max speed
		velocity.X = Mathf.Clamp(velocity.X, -Speed, Speed);

		// Accelerate and Deaccelerate
		velocity.X = Mathf.Lerp(Velocity.X, velocity.X, 0.15f);

		Velocity = velocity;
		MoveAndSlide();
	}

	private void OnEnemyHitPlayer(Node2D body)
	{
		if (body is not IEnemy enemy) return;

		var handler = OnHitEnemy;
		handler?.Invoke(this, enemy);
	}


	// Create the player and place at the given position
	public static PlayerController CreateAt(Node parent, Vector2 position)
    {
		var resource = GD.Load<PackedScene>("res://Entities/Phay/PhayPlayer.tscn");
		var player = resource.Instantiate<PlayerController>();
		parent.AddChild(player);

		player.Position = position;

		return player;
	}
}


