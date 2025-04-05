using Godot;
using System;

public partial class ScreenFade : Node
{
    private ColorRect colorRect;
    private ShaderMaterial shaderMaterial;

    [Export] private ShaderMaterial shaderMaterialDiamonds;
    [Export] private ShaderMaterial shaderMaterialBlocky;
    [Export] private ShaderMaterial shaderMaterialCut;

    public override void _Ready()
    {
        colorRect = GetNode<ColorRect>("ColorRect");
        colorRect.Color = new Color(0, 0, 0, 0);

        shaderMaterial = (ShaderMaterial) colorRect.Material;
    }

    private void ChangeShaderMaterial(ShaderMaterial to)
    {
        colorRect.Material = to;
        shaderMaterial = to;
        SetDeltaParameter(0);
    }

    public void UseDiamonds() => ChangeShaderMaterial(shaderMaterialDiamonds);
    public void UseBlocky() => ChangeShaderMaterial(shaderMaterialBlocky);
    public void UseCut() => ChangeShaderMaterial(shaderMaterialCut);

    // TODO: Confirm fade transition again
    public void UseFade() => ChangeShaderMaterial(shaderMaterialBlocky);

    public Tween FadeColorTo(Color target, float duration, Tween tween = null)
    {
        tween ??= CreateTween();
        tween.TweenProperty(colorRect, "color", target, duration);
        return tween;
    }

    public Tween FadeColorOut(float duration, Tween tween = null) => FadeColorTo(colorRect.Color with { A=0 }, duration, tween);


    /// <summary>
    /// Transition out in duration time, call the action, then transition back in.
    /// </summary>
    /// <returns>The running tween.</returns>
    public Tween TransitionThen(Action action, float duration, Tween tween = null)
    {
        tween ??= CreateTween();

        Transition(0, 1, duration, tween);

        tween.TweenCallback(Callable.From(action));

        return Transition(1, 0, duration, tween);
    }

    /// <summary>
    /// Transition the screen so that it animates between a blank screen using the assigned shader
    /// </summary>
    public Tween Transition(float to, float duration, Tween tween = null)
    {
        float fromValue = shaderMaterial.GetShaderParameter("delta").AsSingle();
        return Transition(fromValue, to, duration, tween);
    }

    /// <summary>
    /// Transition the screen so that it animates between a blank screen using the assigned shader
    /// </summary>
    public Tween Transition(float from, float to, float duration, Tween tween = null)
    {
        tween ??= CreateTween();
        tween.TweenMethod(new Callable(this, nameof(SetDeltaParameter)), from, to, duration);
        return tween;
    }

    /// <summary> Set the shader transition parameter. </summary>
    private void SetDeltaParameter(float delta) => shaderMaterial.SetShaderParameter("delta", delta);
}
