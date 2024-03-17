using Godot;
using System;

public partial class ButtonNewGame : Button
{
	[Export] public PackedScene targetScene;

	public override void _Ready()
	{
		Connect(SignalName.ButtonUp, new Callable(this, nameof(OnPressed)));
	}

	private void OnPressed()
    {
		GetTree().ChangeSceneToPacked(targetScene);
    }
}
