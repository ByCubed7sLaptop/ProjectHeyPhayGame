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

        GD.Print($"spawnPosition: {spawnPosition}");

        Tween tween = Game.Controller.screenFade.FadeColorTo(Colors.Black, 1); 
        tween.TweenCallback(Callable.From(() => { Game.Level.OverrideNextPlayerSpawnPosition(spawnPosition); }));
        tween.TweenCallback(Callable.From(() => { Game.Controller.LoadLevel(targetScenePath); }));
        Game.Controller.screenFade.FadeColorOut(1, tween);
    }
}
