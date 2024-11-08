using Godot;
using System;

public partial class BattleActionResource : Resource
{
    [Export] string DisplayName { get; set; } = string.Empty;
    
    /// <summary>
    /// Chance for it to be picked when picking randomly
    /// </summary>
    [Export] int Weight { get; set; } = 1;
    
    public enum AttackType {
        None,
        Defend,
        RunAway,

        // TODO: Probably will change this to be a seperate resource
        // for each attack, insteaad of calling static methods
        AttackBasic
    }

    /// <summary>
    /// The action to take.
    /// </summary>
    [Export] AttackType Action { get; set; } = AttackType.None;

    // TODO: Insert more attacks here

    static public Tween ActionAttackBasic(BattlerResource attack, BattlerResource defender)
    {
        Tween tween = Game.Battle.CreateTween();

        // Play animation
        //tween.TweenCallback()

        // Do attack damage
        tween.TweenCallback(Callable.From(() =>
            Game.Battle.Attack(attack, defender))
        );

        // This allows us to add actions to do after the attack has finished
        return tween;
    }
}
