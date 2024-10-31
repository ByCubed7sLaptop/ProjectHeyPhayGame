using Godot;
using System;
using System.Collections.Generic;

public partial class DebugDrawer
{
	static public DebugDrawer Instance { get; private set; }
	private DebugDrawerNode DrawingNode { get; set; }
	public DebugDrawer(DebugDrawerNode drawingNode)
    {
		Instance = this;
		DrawingNode = drawingNode;
	}

	public void DrawCircle(Vector2 position, float raduis, Color color)
    {
		DrawingNode.drawQueue.Add((node) => {
			node.DrawCircle(position, raduis, color);
		});
	}
}
