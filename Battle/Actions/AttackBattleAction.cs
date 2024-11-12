using Godot;
using System;

public abstract partial class AttackBattleAction : BattleActionResource
{
    [Export] public new string DisplayName => "Attack";

    public override void Run() => 
        ActionAttackBasic(Game.Battle.Turn.GetBattler(), Game.Battle.RequestChooseTarget());

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
