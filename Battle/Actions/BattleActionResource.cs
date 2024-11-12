using Godot;
using System;

public partial class BattleActionResource : Resource
{
    [Export] public string DisplayName { get; set; } = "Battle Action";
    
    /// <summary>
    /// Chance for it to be picked when picking randomly
    /// </summary>
    [Export] int Weight { get; set; } = 1;

    /// <summary>
    /// Run the given action.
    /// </summary>
    public virtual void Run()
    {

    }
}
