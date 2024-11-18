using Godot;
using System;

public partial class AttackBattleAction : BattleActionResource, IHasIcon
{
    [Export] public Texture2D Icon { get; set; }

    public override void Run()
    {
        Game.Battle.RequestChooseTarget((target) =>
            ActionAttackBasic(Game.Battle.Turn.GetBattler(), target)
        );
    }

    static public Tween ActionAttackBasic(BattlerResource attack, BattlerResource defender)
    {
        Tween tween = Game.Battle.CreateTween();

        // Play animation
        //tween.TweenCallback()

        // Do attack damage
        tween.TweenCallback(Callable.From(() => {
            Game.Battle.Attack(attack, defender);
            Game.Battle.Turn.End();
        }));

        // This allows us to add actions to do after the attack has finished
        return tween;
    }
}
