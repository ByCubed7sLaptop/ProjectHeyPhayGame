using Godot;

public partial class BattlerResource : Resource
{
    [Export] public string Name;
    [Export] public int HealthCurrent = 20;
    [Export] public int Difficulty = 1;
    [Export] public Texture2D Texture;
}