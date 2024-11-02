using Godot;

public partial class BattlerResource : Resource
{
    [Export] public string Name;
    [Export] public int HealthCurrent = 20;
    [Export] public int Difficulty = 1;
    [Export] public Texture2D Texture;

    /// <summary>
    /// Create the node to spawn in current battler
    /// </summary>
    public virtual Node2D GenerateBattler()
    {
        // TODO: This should probably generate from a packed scene
        Sprite2D sprite = new Sprite2D();
        sprite.Name = Name;
        sprite.Texture = Texture;

        return sprite;
    }
}