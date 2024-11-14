using Godot;
using System;

public partial class BattlerResource : Resource
{
    [Export] public string DisplayName;
    [Export] public StatsResource Stats { get; set; }
    [Export] public Texture2D Texture;
    [Export] public Godot.Collections.Array<BattleActionResource> Actions;

    public void Damage(int value)
    {
        Stats.Health -= value;
        Stats.Health = Mathf.Clamp(Stats.Health, 0, Stats.HealthMax);
    }

    public bool IsDead()
    {
        return Stats.Health == 0;
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

    /// <summary>
    /// When it's this object turn to make a move.
    /// Use Game.Battle.Turn.End() to end the turn
    /// </summary>
    public virtual void OnBattleTurnStart()
    {
        // Pick between our actions

        // Play the animation, run the attack ect.

        // For now, just attack a random party member

        if (Actions is null)
        {
            GD.PushWarning($"{DisplayName} actions is null! Ending turn.");
            Game.Battle.Turn.End();
            return;
        }

        if (Actions.Count == 0)
        {
            GD.PushWarning($"{DisplayName} actions are empty! Ending turn.");
            Game.Battle.Turn.End();
            return;
        }

        Random random = new();
        BattleActionResource action = Actions[random.Next(Actions.Count)];
        action.Run();

        //Game.Battle.Attack(this, Party.RandomMember());

        //Tween tween = Game.Controller.CreateTween();
        //tween.TweenInterval(0.5);
        //tween.TweenCallback(Callable.From(() =>
        //    Game.Battle.Turn.End()
        //));
    }
}