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

    public Tween Fade(Color target, float duration, Tween tween = null)
    {
        tween ??= CreateTween();
        tween.TweenProperty(colorRect, "color", target, duration);
        return tween;
    }

    public Tween FadeOut(float duration, Tween tween = null) => Fade(colorRect.Color with { A=0 }, duration, tween);
    

}
