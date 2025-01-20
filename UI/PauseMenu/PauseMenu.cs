using Godot;
using System;

public partial class PauseMenu : Control
{
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("ui_esc"))
        {
            if (Game.IsPaused)
                Game.Play();
            else
                Game.Pause();
        }
    }
}
