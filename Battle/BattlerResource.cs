using Godot;

public partial class BattlerResource : Resource
{
    [Export] public string DisplayName;
    [Export] public StatsResource Stats { get; set; }
    [Export] public Texture2D Texture;

   

    public void Damage(int value)
    {
        Stats.Health -= value;
        Stats.Health = Mathf.Clamp(Stats.Health, 0, Stats.HealthMax);
    }

    /// <summary>
    /// Create the node to spawn in current battler
    /// </summary>
    public virtual Node2D GenerateBattler()
    {
        // TODO: This should probably generate from a packed scene
        Sprite2D sprite = new Sprite2D();
        sprite.Name = DisplayName;
        sprite.Texture = Texture;

        return sprite;
    }
}