using Godot;
using System;
using System.Collections.Generic;

public partial class TargetSelector : Control
{
    private readonly List<Node2D> targetNodes = new List<Node2D>();
    public event EventHandler<TargetElement> OnTargetSelected;
    private int currentTargetIndex = 0;

    public void Setup(IEnumerable<Node2D> targets)
    {
        Clear();
        foreach (var node in targets)
            Target(node);
        Highlight();
    }

    public void Target(Node2D target)
    {
        targetNodes.Add(target);
    }

    public void Clear()
    {
        targetNodes.Clear();
    }


    public override void _Input(InputEvent @event)
    {
        if (!Visible)
            return;

        if (targetNodes.Count == 0)
            return;

        // Check for navigation keys
        if (@event.IsActionPressed("ui_down") || @event.IsActionPressed("ui_right"))
        {
            // Move to the next target
            currentTargetIndex = (currentTargetIndex + 1) % targetNodes.Count;
            Highlight();
        }
        else if (@event.IsActionPressed("ui_up") || @event.IsActionPressed("ui_left"))
        {
            // Move to the previous target
            currentTargetIndex = (currentTargetIndex - 1 + targetNodes.Count) % targetNodes.Count;
            Highlight();
        }

        // Check for selection keys (Enter or Space)
        if (@event.IsActionPressed("ui_accept"))
        {
            Node2D selectedTarget = targetNodes[currentTargetIndex];
            OnTargetSelected?.Invoke(this, new(selectedTarget, currentTargetIndex));
        }
    }

    public void Highlight()
    {
        if (targetNodes.Count == 0)
        {
            Hide();
            return;
        }

        Show();
        currentTargetIndex %= targetNodes.Count;
        Position = targetNodes[currentTargetIndex].Position;
    }

    public class TargetElement : EventArgs
    {
        public Node2D Target { get; init; }
        public int Index { get; init; }

        public TargetElement(Node2D target, int index)
        {
            Target = target;
            Index = index;
        }
    }
}