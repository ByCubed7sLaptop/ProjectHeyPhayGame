using Godot;
using System;

public partial class TransitionArea : Area2D
{
    [Export(PropertyHint.File, "*.tscn,")]
    public string targetScenePath;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        BodyEntered += TransitionArea_BodyEntered;
	}

    private void TransitionArea_BodyEntered(Node2D body)
    {
        if (body is not Player)
            return;

        Tween tween = Game.Controller.screenFade.Fade(Colors.Black, 1);
        tween.TweenCallback(Callable.From(() => { Game.Controller.LoadLevel(targetScenePath); }));
        Game.Controller.screenFade.FadeOut(1, tween);
    }
}
