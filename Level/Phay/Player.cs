using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public Area2D Hitbox;

	[ExportCategory("Movement")]
	[Export] public float Speed = 100.0f; // Max speed
	[Export] public float Acceleration = 0.15f; // Time to max speed

	[Export] public float JumpVelocity = 300.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public EventHandler<EncounterResource> OnHitEncounter;


	public override void _Ready()
	{
		// Disabling a CollisionObject node during a physics callback is not allowed and will cause undesired behavior.
		// Disable with call_deferred() instead.
		Hitbox.BodyEntered += (e) => CallDeferred(nameof(OnEnemyHitPlayer), e);

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
		if (body is not EncounterController encounter) return;

		var handler = OnHitEncounter;
		handler?.Invoke(this, encounter.Resource);

		// BUG: Disabling the ProcessMode for the tree seems to freeze the body physics updates.
		// Meaning the hitbox gets "stuck" when hitting another body.
		// Toggling the hitboxs monitoring off and on seems to fix this problem
		Hitbox.Monitoring = false;
		Hitbox.Monitoring = true;
	}


	// Create the player and place at the given position
	public static Player CreateAt(Node parent, Vector2 position)
    {
		var resource = GD.Load<PackedScene>("res://Level/Phay/PhayPlayer.tscn");
		var player = resource.Instantiate<Player>();
		parent.AddChild(player);

		player.Position = position;

		return player;
	}
}


