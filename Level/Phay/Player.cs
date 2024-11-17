using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Player : CharacterBody2D
{
	[Export] public Area2D Hitbox;
    [Export] public Area2D InteractableHitbox;

    [ExportCategory("Movement")]
	[Export] public float Speed = 100.0f; // Max speed
	[Export] public float Acceleration = 0.15f; // Time to max speed
	[Export] public float JumpVelocity = 300.0f;

	private Vector2 PlayerVelocity;

	// Time since the last jump press to buffer
	[Export] public const float JumpBufferTime = 0.1f;
	private double jumpBufferCounter = 0f;

	// Time since last on jumpable ground
	[Export] public const float CoyoteTime = 0.3f;
	private double coyoteTimeCounter = 0f;

	private bool isJumping = false;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public EventHandler<EncounterBody> OnHitEncounter;

	private Vector2 RespawnPosition { get; set; }

    private List<EntityInteractionHitbox> Interactables = new List<EntityInteractionHitbox>();
    private Label InteractLabel { get; set; }

    public override void _Ready()
	{
		// Disabling a CollisionObject node during a physics callback is not allowed and will cause undesired behavior.
		// Disable with call_deferred() instead.
		Hitbox.BodyEntered += (e) => CallDeferred(nameof(OnBodyEntered), e);
        Hitbox.AreaEntered += (e) => CallDeferred(nameof(OnAreaEntered), e);

        InteractableHitbox.AreaEntered += (e) => CallDeferred(nameof(InteractableHitboxAreaEntered), e.GetNode("InteractionHitbox") as EntityInteractionHitbox);
        InteractableHitbox.AreaExited += (e) => CallDeferred(nameof(InteractableHitboxAreaExited), e.GetNode("InteractionHitbox") as EntityInteractionHitbox);

        InteractLabel = GetNode($"Interaction Components/InteractLabel") as Label;
        UpdateInteractions();
    }

    public override void _Process(double delta)
    {
		if (OS.IsDebugBuild())
			DebugDrawer.DrawCircle(RespawnPosition, 3, Colors.Red);
    }

    public override void _PhysicsProcess(double delta)
	{
        ProcessMovementVelocity(delta);
        MoveAndSlide();
		ProcessRespawnPosition();
        ProcessInteract();
    }

    private void ProcessMovementVelocity(double delta)
    {
        PlayerVelocity = Velocity;
        ProcessMovementDirection();
        ProcessJump(delta);
        ProcessGravity(delta);

        // Accelerate and Deaccelerate
        PlayerVelocity.X = Mathf.Lerp(Velocity.X, PlayerVelocity.X, 0.15f);

        // Clamp max speed
        PlayerVelocity.X = Mathf.Clamp(PlayerVelocity.X, -Speed, Speed);
        PlayerVelocity.Y = Mathf.Min(PlayerVelocity.Y, gravity);

        Velocity = PlayerVelocity;
    }

    private void ProcessGravity(double delta)
    {
        if (!IsOnFloor()) PlayerVelocity.Y += gravity * (float)delta;
    }

	private void ProcessMovementDirection()
	{
        // Get the input direction and handle the movement/deceleration.
        Vector2 direction = Input.GetVector("player_left", "player_right", "player_up", "player_down");
        if (direction != Vector2.Zero)
            PlayerVelocity.X = direction.X * Speed;
        else
            PlayerVelocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

    }

	private void ProcessJump(double delta)
	{
        // Handle Jump
        // Set jump buffer when jump is pressed
        jumpBufferCounter -= delta;
        if (Input.IsActionPressed("player_jump"))
            jumpBufferCounter = JumpBufferTime;

        // Set coyote buffer
        coyoteTimeCounter -= delta;
        if (IsOnFloor())
            coyoteTimeCounter = CoyoteTime;

        bool requestJump = jumpBufferCounter > 0;
        bool canJump = IsOnFloor() || coyoteTimeCounter > 0;

        // Apply jump velocity
        if (requestJump && canJump && !isJumping)
        {
            PlayerVelocity.Y = -JumpVelocity;
            isJumping = true;
        }

        // If no longer requests to jump
        if (!requestJump && isJumping && PlayerVelocity.Y < gravity)
            PlayerVelocity.Y += gravity * (float)delta;

        if (IsOnFloor())
            isJumping = false;
    }

    public void ProcessInteract()
    {
        if (Input.IsActionJustPressed("player_interact"))
        {
            OnInteract();
        }
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is EncounterBody encounter) OnEnemyHitPlayer(encounter);
    }

    public void OnAreaEntered(Node2D area)
    {
        if (area is DeathPlane) OnTouchDeathPlane();
        if (area is Spike) OnTouchStageHazard();
    }

    private void OnEnemyHitPlayer(EncounterBody encounter)
	{
		var handler = OnHitEncounter;
		handler?.Invoke(this, encounter);
	}

    private void OnTouchDeathPlane()
    {
        Party.DamageAllMembers(1);
        Position = RespawnPosition;
    }

    private void OnTouchStageHazard()
    {
        Party.DamageRandomMember(1);
        //Get knocked back
        //Become invulnerable for a while
    }


	// Create the player and place at the given position
	public static Player CreateAt(Node parent, Vector2 position)
    {
        // TODO: Should be Game.PackedScenes.Player?
		var player = Game.Controller.PlayerPackedScene.Instantiate<Player>();
		parent.AddChild(player);

		player.Position = position;

		return player;
	}

	private void ProcessRespawnPosition()
	{
        if (IsOnFloor() && IsValidRespawnLocation())
        {
            RespawnPosition = Position;
        }
    }

	private bool IsValidRespawnLocation()
	{
        var spaceState = GetWorld2D().DirectSpaceState;
        var query = PhysicsRayQueryParameters2D.Create(Position, new Vector2(Position.X, Position.Y + 50));
        var result = spaceState.IntersectRay(query);

		return result.Count > 0;
    }

    public void InteractableHitboxAreaEntered(EntityInteractionHitbox interactable)
    {
        Interactables.Insert(0, interactable);
        UpdateInteractions();
    }

    public void InteractableHitboxAreaExited(EntityInteractionHitbox interactable)
    {
        Interactables.Remove(interactable);
        UpdateInteractions();
    }

    public void UpdateInteractions()
    {
        if (Interactables.Count > 0)
        {
            InteractLabel.Text = Interactables.First().InteractionLabelText;
        } 
        else
        {
            InteractLabel.Text = string.Empty;
        }
    }

    public void OnInteract()
    {
        if (Interactables.Count > 0)
        {
            Interactables.First().Interact();
        }
    }
}


