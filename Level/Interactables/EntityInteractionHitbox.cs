using Godot;
using ThePhayGame.Level.Interactables;

public partial class EntityInteractionHitbox : Node
{
    [Export] public string InteractionLabelText { get; set; }
    [Export] public InteractableType InteractableType { get; set; }

    public void Interact()
    {
        var interactableParent = GetParent() as IInteractable;
        interactableParent.OnInteract();
    }
}
