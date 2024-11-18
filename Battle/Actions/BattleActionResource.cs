using Godot;
using System;

public partial class BattleActionResource : Resource
{
    [Export] public string DisplayName { get; set; } = "Battle Action Display Name";
    
    /// <summary>
    /// Chance for it to be picked when picking randomly
    /// </summary>
    [Export] int Weight { get; set; } = 1;

    /// <summary>
    /// Run the given action.
    /// </summary>
    public virtual void Run()
    {
        // End the turn
        Game.Battle.Turn.End();
    }
}

public interface IHasIcon
{
    public Texture2D Icon { get; }
}
