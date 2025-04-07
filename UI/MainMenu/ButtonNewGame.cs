using Godot;
using System;
using System.Transactions;

public partial class ButtonNewGame : Button
{
	[Export] public PackedScene targetScene;

	[Export] public ShaderMaterial transistion;

	public override void _Ready()
	{
		Connect(SignalName.ButtonUp, new Callable(this, nameof(OnPressed)));
	}

	Tween transitioningTween = null;
    private void OnPressed()
    {
        Game.Controller.CallDeferred(nameof(Game.Controller.GoToLevelFromMenu), targetScene);
    }

	private void TweenShaderParam(float delta) => transistion.SetShaderParameter("delta", delta);
}
