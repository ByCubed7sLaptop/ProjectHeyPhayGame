using Godot;
using System;
using System.Collections.Generic;

public partial class DebugDrawer
{
	// TODO: Persistant draw queue?
	public readonly List<Action<DebugDrawerNode>> drawQueue = new();

	static public DebugDrawer Data { get; private set; }
	static public void Initialization()
	{
		Data = new();
	}

	private DebugDrawer() { }

	/// <summary>
	/// Clear the draw queue. Should only be called by DebugDrawerNode
	/// </summary>
	public void Clear()
	{
		drawQueue.Clear();
	}


	/// <summary>
	/// Draw a filled in circle at the given position.
	/// </summary>
	static public void DrawCircle(Vector2 position, float raduis, Color color)
    {
		Data.drawQueue.Add((node) => {
			node.DrawCircle(position, raduis, color);
		});
	}
}
