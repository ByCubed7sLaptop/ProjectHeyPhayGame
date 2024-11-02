using Godot;
using System;
using System.Collections.Generic;

public partial class DebugDrawerNode : Node2D
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//DebugDrawer.DrawCircle(Vector2.Zero, 30, Colors.RebeccaPurple);
		QueueRedraw();
	}

	public override void _Draw()
    {
		// Run each draw request
		foreach (var draw in DebugDrawer.Data.drawQueue) draw(this);
		DebugDrawer.Data.Clear();
	}
}
