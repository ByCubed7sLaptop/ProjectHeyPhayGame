using Godot;

public partial class EnemyResource : Resource
{
    [Export] public string Name;
    [Export] public int Health = 20;
    [Export] public int Difficulty = 1;
    [Export] public Texture2D Texture;
}