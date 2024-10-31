using Godot;
using System;
using System.Collections.Generic;

public partial class DebugDrawerNode : Node2D
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		DebugDrawer.Instance.DrawCircle(Vector2.Zero, 30, Colors.RebeccaPurple);
		QueueRedraw();
	}

	public readonly List<Action<DebugDrawerNode>> drawQueue = new List<Action<DebugDrawerNode>>();
	public override void _Draw()
    {
		// Run each draw request
		foreach (var drawRequest in drawQueue)
			drawRequest(this);
		drawQueue.Clear();
	}
}
