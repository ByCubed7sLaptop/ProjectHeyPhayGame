using Godot;
using System;

public partial class UnpauseButton : Button
{
	public override void _Ready()
	{
        Pressed += UnpauseButton_Pressed;
	}

    private void UnpauseButton_Pressed()
    {
        Game.Controller.HidePauseMenu();
    }
}
