using Godot;

public partial class Lever : Area2D
{
    [Export] public string InteractionLabel { get; set; }

    private bool State = false;

    private Sprite2D spriteA;
    private Sprite2D spriteB;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        spriteA = GetNode("Sprite2D") as Sprite2D;
        spriteB = GetNode("Sprite2D2") as Sprite2D;
        ChangeSprite();
    }


    public void Interact()
    {
        State = !State;
        ChangeSprite();
    }

    private void ChangeSprite()
    {
        spriteA.Visible = State;
        spriteB.Visible = !State;
    }
}
