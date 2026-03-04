using Godot;
using System;

public partial class GuardBattleAction : BattleActionResource, IHasIcon
{
    [Export] public Texture2D Icon { get; set; }

    public override void Run()
    {
        ActionDefend(Game.Battle.Turn.GetBattler());
    }

    static public Tween ActionDefend(BattlerResource battler)
    {
        Tween tween = Game.Battle.CreateTween();

        // Play animation
        //tween.TweenCallback()

        tween.TweenCallback(Callable.From(() => {
            Game.Battle.Defend(battler);
            Game.Battle.Turn.End();
        }));

        // This allows us to add actions to do after the attack has finished
        return tween;
    }
}
