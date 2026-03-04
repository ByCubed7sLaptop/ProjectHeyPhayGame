using Godot;
using System;

public partial class TransitionArea : Area2D
{
    [Export(PropertyHint.File, "*.tscn,")]
    public string targetScenePath;

    [Export]
    public Vector2I spawnPosition;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        BodyEntered += TransitionArea_BodyEntered;
	}

    private void TransitionArea_BodyEntered(Node2D body)
    {
        if (body is not Player)
            return;

        Game.Level.OverrideNextPlayerSpawnPosition(spawnPosition);
        Game.Controller.LoadLevel(targetScenePath);
    }
}
