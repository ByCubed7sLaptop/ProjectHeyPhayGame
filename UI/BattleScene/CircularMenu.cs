using Godot;
using System;
using System.Collections.Generic;

public partial class CircularMenu : Control
{
    [Export] public float radius = 10.0f; // Radius of the circle layout
    [Export] public float angleOffset = 0.0f; // Initial offset angle
    [Export] public Vector2 positionScale = Vector2.One;
    [Export] public Vector2 positionOffset = Vector2.Zero;

    private Node2D target;
    private BattlerResource targetBattler;

    private readonly List<TextureButton> menuIcons = new();
    private int currentSelectionIndex = 0;

    private Control icons;
    private Sprite2D selectionHighlight;
    private Label label;

    public override void _Ready()
    {
        // Get child nodes
        // TODO: Change to expoted references
        icons = GetNode<Control>("Icons");
        selectionHighlight = GetNode<Sprite2D>("SelectionHighlight");
        label = GetNode<Label>("CenterContainer/Label");

        Hide();
    }

    public void Target(Node2D newTarget, BattlerResource newTargetBattler)
    {
        target = newTarget;
        targetBattler = newTargetBattler;

        // Remove all current icons
        foreach (var child in icons.GetChildren())
        {
            child.QueueFree();
            icons.RemoveChild(child);
        }

        // Add them back based off of the battlers action
        foreach (var action in targetBattler.Actions)
        {
            Texture2D texture = null;
            if (action is IHasIcon hasIcon)
                texture = hasIcon.Icon;

            TextureButton textureButton = new();
            textureButton.Size = Vector2.One * 18;
            textureButton.TextureNormal = texture;
            textureButton.Name = action.DisplayName;
            icons.AddChild(textureButton);
        }

        // Initialize icons in the circular layout
        menuIcons.Clear();
        foreach (TextureButton icon in icons.GetChildren())
            menuIcons.Add(icon);

        currentSelectionIndex %= menuIcons.Count;   

        ArrangeIconsInCircle();
        UpdateSelectionHighlight();
        Show();
    }

    private void ArrangeIconsInCircle()
    {
        int iconCount = menuIcons.Count;
        float angleStep = Mathf.Tau / iconCount;

        for (int i = 0; i < iconCount; i++)
        {
            float angle = (i - currentSelectionIndex + 3) * angleStep + angleOffset;
            Vector2 iconPosition = new Vector2(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius
            );
            iconPosition *= positionScale;
            menuIcons[i].Position = iconPosition;
        }
    }

    private void UpdateSelectionHighlight()
    {
        // Set highlight position to the currently selected icon
        selectionHighlight.Position = menuIcons[currentSelectionIndex].Position;
        label.Text = menuIcons[currentSelectionIndex].Name;
    }

    public override void _Process(double delta)
    {
        if (!Visible)
            return;

        if (target is null) 
            return;

        Position = target.Position + positionOffset;
        // Update logic for input
        if (Input.IsActionJustPressed("ui_right"))
        {
            currentSelectionIndex = (currentSelectionIndex + 1) % menuIcons.Count;
            ArrangeIconsInCircle();
            UpdateSelectionHighlight();
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            currentSelectionIndex = (currentSelectionIndex - 1 + menuIcons.Count) % menuIcons.Count;
            ArrangeIconsInCircle();
            UpdateSelectionHighlight();
        }

        // Handle confirmation (select the highlighted option)
        if (Input.IsActionJustPressed("ui_accept"))
        {
            ArrangeIconsInCircle();
            ActivateSelection();
        }


    }

    private void ActivateSelection()
    {
        Hide();
        targetBattler.Actions[currentSelectionIndex].Run();
    }
}