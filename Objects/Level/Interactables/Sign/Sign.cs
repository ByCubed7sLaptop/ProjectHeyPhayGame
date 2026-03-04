using Godot;

public partial class Sign : Area2D, IInteractable
{
    //Todo read from some json maybe?
    [Export] public string Message;

    public void OnInteract()
    {
        OS.Alert(Message);
    }
}
