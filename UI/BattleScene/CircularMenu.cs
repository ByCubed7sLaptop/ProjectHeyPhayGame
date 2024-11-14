using Godot;
using System;
using System.Collections.Generic;

public partial class CircularMenu : Control
{
    private Control icons;
    private Sprite2D selectionHighlight;
    private Label label;

    private List<TextureButton> menuIcons = new List<TextureButton>();
    private int currentSelectionIndex = 0;

    [Export] public float radius = 10.0f; // Radius of the circle layout
    [Export] public float angleOffset = 0.0f; // Initial offset angle
    [Export] public Vector2 positionScale = Vector2.One;
    [Export] public Vector2 positionOffset = Vector2.Zero;

    [Export] public Node2D target;
    public BattlerResource targetBattler;

    public override void _Ready()
    {
        // Get child nodes
        // TODO: Change to expoted references
        icons = GetNode<Control>("Icons");
        selectionHighlight = GetNode<Sprite2D>("SelectionHighlight");
        label = GetNode<Label>("CenterContainer/Label");

        // Initialize icons in the circular layout
        foreach (TextureButton icon in icons.GetChildren())
            menuIcons.Add(icon);

        Hide();
    }

    public void Target(Node2D newTarget, BattlerResource newTargetBattler)
    {
        target = newTarget;
        targetBattler = newTargetBattler;

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
            float angle = (i + currentSelectionIndex) * angleStep + angleOffset;
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

        Position = target.Position + positionOffset;
        // Update logic for input
        if (Input.IsActionJustPressed("ui_right"))
        {
            currentSelectionIndex = (currentSelectionIndex + 1) % menuIcons.Count;
            UpdateSelectionHighlight();
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            currentSelectionIndex = (currentSelectionIndex - 1 + menuIcons.Count) % menuIcons.Count;
            UpdateSelectionHighlight();
        }

        // Handle confirmation (select the highlighted option)
        if (Input.IsActionJustPressed("ui_accept"))
        {
            ActivateSelection();
        }
    }

    private void ActivateSelection()
    {
        // TODO: get the action associated with the selection
        Hide();
        targetBattler.Actions[currentSelectionIndex].Run();
        

        // Tell the battle controller the actio you've chosen
        //string action = menuIcons[currentSelectionIndex].Name;

        //// TODO: This should tell the UI element to pick an enemy to then attack
        //if (action == "Attack")
        //{
        //    // TODO: Request to pick an enemy to attack

        //    Game.Battle.Attack(Game.Battle.Turn.GetBattler(), Game.Battle.currentEncounter.GetRandom());
        //    Hide();

        //    Game.Battle.Turn.End();
        //}

    }
}