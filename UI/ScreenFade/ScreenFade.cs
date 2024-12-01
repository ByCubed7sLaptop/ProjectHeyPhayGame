using Godot;
using System;

public partial class ScreenFade : Node
{
    private ColorRect colorRect;

    public override void _Ready()
    {
        colorRect = GetNode<ColorRect>("ColorRect");
        colorRect.Color = new Color(0, 0, 0, 0);
    }

    private void Fade(Color target, float duration)
    {
        Tween tween = CreateTween();
        tween.TweenProperty(colorRect, "color", target, duration);
    }

    private void FadeOut(float duration)
    {
        Fade(Colors.Transparent, duration);
    }

}
