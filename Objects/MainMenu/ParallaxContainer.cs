using Godot;
using System;

[Tool]
public partial class ParallaxContainer : Container
{
	[Export] private Vector2 multiplication = Vector2.One;
	[Export] private Vector2 offset = Vector2.Zero;

	public override void _Notification(int what)
	{
		if (what == NotificationSortChildren)
		{
			foreach (Node child in GetChildren())
			{
				if (child is Control c)
					FitChildInRect(c, new Rect2(Vector2.Zero, Size));
			}
		}
	}

	public override void _Process(double delta)
	{
		Vector2 target = Size / 2;

		if (!Engine.IsEditorHint())
			if (GetViewportRect().HasPoint(GetGlobalMousePosition()))
				target = GetGlobalMousePosition();

		offset = offset.Lerp(target, 0.01f);

		foreach (Node child in GetChildren())
		{
			if (child is Control c)
			{
				c.Position = offset;
				c.Position -= Size / 2;
				c.Position *= multiplication;
				c.Position /= 20 / c.SizeFlagsStretchRatio;
			}
		}

	}
}
