using Godot;
using System;

public partial class LevelCamera : Camera2D
{
	[Export] public bool horizontal;
	[Export] public CharacterBody2D Target { get => _target; 
		set {
			_target = value;
            Position = Target.Position;
        }}
	private CharacterBody2D _target;


	private Vector2 TargetPosition { get; set; }

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
		if (Target is null) return;


		// Follow the target if below or moving down
		if (Target.Position.Y >= Position.Y)
            TargetPosition = Target.Position;

		// Move to target (more slowler) only if on the floor, no matter what Y level
		else if (Target.IsOnFloor())
            TargetPosition = new Vector2(
				Target.Position.X,
				(float)Mathf.Lerp(Position.Y, Target.Position.Y, 0.2f)
			);

		// Otherwise, Lock Y and only move to X of target
		else TargetPosition = TargetPosition with { X = Target.Position.X };


		Position = Position.Lerp(TargetPosition, 0.005f);
        Zoom = Zoom.Lerp(Vector2.One * (2.5f * 1-(Mathf.Clamp(Position.DistanceTo(TargetPosition), 0, 200)/200)), 0.2f);
    }
}
