using Godot;
using System;
using System.Collections.Generic;

public partial class Background : CanvasLayer
{
    private List<ShaderMaterial> shaderMaterials;
    
    public override void _Ready()
    {
        shaderMaterials = new();
        foreach (Node node in GetChildren())
            if (node is ColorRect colorRect)
                if (colorRect.Material is ShaderMaterial colorRectShaderMaterial)
                    shaderMaterials.Add(colorRectShaderMaterial);
    }

	public override void _Process(double delta)
	{
        Vector2? position = Game.Level?.Camera?.Position;

        if (position.HasValue)
            foreach (var shaderMaterial in shaderMaterials)
                shaderMaterial.SetShaderParameter("position", position.Value);
    }
}
