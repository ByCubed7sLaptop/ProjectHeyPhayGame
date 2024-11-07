using Godot;
using ThePhayGame.Level.Interactables.Lever;

public partial class EntityInteractionHitbox : Node
{
    [Export] public string InteractionLabelText { get; set; }
    //Todo read from some json maybe?
    [Export] public string InteractionMessage { get; set; }
    [Export] public InteractableType InteractableType { get; set; }

    private bool State = false;

    private Sprite2D spriteA;
    private Sprite2D spriteB;

    private Label InteractLabel { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        InteractLabel = GetNode($"InteractLabel") as Label;

        switch (InteractableType)
        {
            case InteractableType.Switch:
                spriteA = GetParent().GetNode("Sprite2D") as Sprite2D;
                spriteB = GetParent().GetNode("Sprite2D2") as Sprite2D;
                ChangeSprite();
                break;
            case InteractableType.NPC:
                break;
        }
    }

    public void Interact()
    {
        switch (InteractableType)
        {
            case InteractableType.Switch:
                State = !State;
                ChangeSprite();
                break;
            case InteractableType.NPC:
                DisplayMessage();
                break;
        }
    }

    private void ChangeSprite()
    {
        spriteA.Visible = State;
        spriteB.Visible = !State;
    }

    private void DisplayMessage()
    {
        //Todo display message box
        OS.Alert(InteractionMessage);
    }
}
