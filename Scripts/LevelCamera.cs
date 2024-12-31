using Godot;
using System;
using System.Data;

public partial class LevelCamera : Camera2D
{

    [Export]
    public CharacterBody2D Target
    {
        get => _target;
        set
        {
            _target = value;
            Position = Target.Position;
        }
    }

    [Export] public Vector2I offset;

    [ExportCategory("Updates")]
    [Export] bool UpdateOnPhysicsProcess { get; set; } = false;
    [Export] bool UpdateOnProcess { get; set; } = false;

    [ExportCategory("Zoom")]
    [Export] float ZoomDefault { get; set; } = 3f;
	[Export] float ZoomExponential { get; set; } = 200f;
	[Export] float ZoomSpeed { get; set; } = 3f;

    [ExportCategory("")]
    [Export] float VelocityLookAhead { get; set; } = 5;
	private CharacterBody2D _target;

	private Vector2 targetPosition;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Target is null)
            return;
        Position = Target.Position;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
        if (UpdateOnProcess)
            UpdateCamera(delta);
    }
	

    public override void _PhysicsProcess(double delta)
    {
        if (UpdateOnPhysicsProcess)
            UpdateCamera(delta);
    }

	private void UpdateCamera(double delta)
    {
        if (Target is null)
            return;

        // Follow the target if below or moving down
        if (Target.Position.Y >= Position.Y)
            targetPosition = Target.Position;

        // Move to target (more slowler) only if on the floor, no matter what Y level
        else if (Target.IsOnFloor())
            targetPosition = new Vector2(
                Target.Position.X,
                (float)Mathf.Lerp(Position.Y, Target.Position.Y, 0.2f)
            );

        // Otherwise, Lock Y and only move to X of target
        else
            targetPosition = targetPosition with { X = Target.Position.X };

        // Check the velocity and pivot forwards
        targetPosition.X += Target.Velocity.X / VelocityLookAhead;

        // Add offset
        targetPosition += offset;

        Position = Position.Lerp(targetPosition, 3.2f * (float)delta);
        Zoom = Zoom.Lerp(Vector2.One * (ZoomDefault * 1 - (Mathf.Clamp(Mathf.Abs(Position.X - Target.Position.X), 0, ZoomExponential) / ZoomExponential)), ZoomSpeed * (float)delta);
    }
}
