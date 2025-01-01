using Godot;
using System;
using System.Collections.Generic;
using System.Data;

public partial class LevelCamera : Camera2D
{


    [ExportCategory("Updates")]
    [Export] bool UpdateOnPhysicsProcess { get; set; } = false;
    [Export] bool UpdateOnProcess { get; set; } = false;
    

    [ExportCategory("Position")]
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
    [Export] public Vector2I TargetOffset { get; set; }
    [Export] float VelocityLookAhead { get; set; } = 5;
    [Export] float Speed { get; set; } = 3.2f;


    [ExportCategory("Zoom")]
    [Export] float ZoomDefault { get; set; } = 3f;
	[Export] float ZoomExponential { get; set; } = 2000f;
	[Export] float ZoomSpeed { get; set; } = 3f;
	[Export] float CareDistance { get; set; } = 200f;


	private CharacterBody2D _target;
	private Vector2 targetPosition;
    Dictionary<Node2D, int> PointsOfInterest { get; set; } = new();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var example = GetTree().Root.GetNode<Node2D>("Level/Spike");
        Tween tween = CreateTween();
        tween.TweenInterval(2);
        tween.TweenCallback(Callable.From(() => { AddPOI(example); }));
        tween.TweenInterval(4);
        tween.TweenCallback(Callable.From(() => { RemovePOI(example); }));


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
        targetPosition += TargetOffset;

        // Pan to POI
        if (PointsOfInterest.Count != 0)
            targetPosition = targetPosition.Lerp(GetPointsOfInterestCenter(), 0.8f);


        Position = Position.Lerp(targetPosition, Speed * (float)delta);

        // Percentage of the distance between 0 and ZoomExponential
        // Zoom is inversed so 1-percentage
        float distance = Mathf.Abs(Position.X - Target.Position.X);
        float percentage = 1 - Mathf.Clamp(Mathf.Abs(distance), 0, ZoomExponential) / ZoomExponential;
        
        if (distance < CareDistance)
            Zoom = Zoom.Lerp(Vector2.One * ZoomDefault * percentage, ZoomSpeed * (float)delta);
    }


    public Vector2 GetPointsOfInterestCenter()
    {
        if (PointsOfInterest is null)
            throw new NullReferenceException("PointsOfInterest is null.");

        Vector2 total = Vector2.Zero;
        int totalWeight = 0;

        foreach((Node2D nodepath, int weight) in PointsOfInterest)
        {
            total += nodepath.Position * weight;
            totalWeight += weight;
        }

        if (totalWeight == 0)
            throw new Exception("PointsOfInterest total weight is 0.");

        return total / totalWeight;
    }

    public void AddPOI(Node2D node, int weight = 1)
    {
        PointsOfInterest[node] = weight;
    }

    public void RemovePOI(Node2D node)
    {
        PointsOfInterest.Remove(node);
    }

    public void TargetPlayer() => Target = Game.Level.Player;
    
}
