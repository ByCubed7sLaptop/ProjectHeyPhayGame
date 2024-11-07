using Godot;

public partial class Lever : Area2D, IInteractable
{
    private bool State = false;

    private Sprite2D spriteA;
    private Sprite2D spriteB;

    public override void _Ready()
    {
        spriteA = GetNode("Sprite2D") as Sprite2D;
        spriteB = GetNode("Sprite2D2") as Sprite2D;
        ChangeSprite();
        base._Ready();
    }
    public void OnInteract()
    {
        State = !State;
        ChangeSprite();
    }

    public void ChangeSprite()
    {
        spriteA.Visible = State;
        spriteB.Visible = !State;
    }
}
