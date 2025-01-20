using Godot;
using System;

public partial class QuitButton : Button
{
    public override void _Ready()
    {
        Pressed += QuitButton_Pressed;
    }

    private void QuitButton_Pressed()
    {
        GetTree().Quit();
    }
}
