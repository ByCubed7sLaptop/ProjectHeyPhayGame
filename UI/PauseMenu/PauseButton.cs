using Godot;
using System;

public partial class PauseButton : TextureButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Pressed += PauseButton_Pressed;
	}

    private void PauseButton_Pressed()
    {
        Game.Controller.ShowPauseMenu();
    }
}
