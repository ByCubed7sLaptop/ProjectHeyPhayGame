using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using static System.Formats.Asn1.AsnWriter;

// Deals with the overall game, saving, loading, platforming, battles, pausing, ect.
public partial class GameController : Node
{
    static public GameController Instance { get; set; }
    static public BattleController Battle => Instance.battle;

	[Export] public PackedScene LevelPackedScene;
	[Export] public PackedScene BattlePackedScene;
	[Export] public PackedScene LevelHudPackedScene;
	[Export] public PackedScene PlayerPackedScene;

    public BattleController battle;
    public LevelController level;

    [Export] public ScreenFade screenFade;

    public override void _Ready()
	{
		Instance = this;
        DebugDrawer.Initialization();
        screenFade ??= GetNode<ScreenFade>("ScreenFade");
    }

    public void StartBattleWith(EncounterBody encounter)
    {
        // TODO: Assumes level controller is loaded and is the main scene

        // Clear out the old battle cache
        if (battle is not null)
        {
            GetTree().Root.RemoveChild(battle);
            battle.QueueFree();
            battle = null;
        }

        // Set up the battle scene
        battle = BattlePackedScene.Instantiate<BattleController>();
        battle.currentEncounter = encounter.GetResource();

        // TODO: Move to EncounterBody destroy method to add effects / ect
        battle.OnEnd += (e, o) => encounter.QueueFree();
        battle.OnEnd += (e, o) => TransferToLevel();

        // TODO: Change to game over scene or respawn at room enterence, ect.
        battle.OnLose += (e, o) => Party.FullHeal();

        TransferToBattle();
    }

    /// <summary>
    /// Transfer to the battle scene from the level scene
    /// </summary>
    public void TransferToBattle()
    {
        if (battle is null)
        {
            GD.PushError("Transfer to battle called but Battle is null!");
            throw new Exception("Transfer to battle called but Battle is null!");
        }

        GetTree().Paused = true;

        // TODO: Assumes we're on the level scene
        screenFade.UseBlocky();
        screenFade.TransitionThen(() => {
            GetTree().Root.RemoveChild(level);
            GetTree().Root.AddChild(battle);
            GetTree().Paused = false;
            screenFade.UseCut();
        }, 1).SetPauseMode(Tween.TweenPauseMode.Process);
    }

    /// <summary>
    /// Transfer to the level scene from the battle scene
    /// </summary>
    public void TransferToLevel()
    {
        GetTree().Paused = true;

        // TODO: Assumes we're on the battle scene
        screenFade.UseBlocky();
        screenFade.TransitionThen(() => {
            GetTree().Root.RemoveChild(battle);
            GetTree().Root.AddChild(level);
            GetTree().Paused = false;
            screenFade.UseDiamonds();
        }, 2.0f).SetPauseMode(Tween.TweenPauseMode.Process);
    }


    private bool isLoadingInProgress = false;
    public void LoadLevel(PackedScene scene)
    {
        if (isLoadingInProgress)
            return;

        // TODO: We'll pause for now jic a battle happens AS we transition.
        GetTree().Paused = true;
        isLoadingInProgress = true;

        screenFade.UseDiamonds();
        screenFade.TransitionThen(() => {

            if (level is not null)
            {
                GetTree().Root.RemoveChild(level);
                level.QueueFree();
                level = null;
            }

            level = scene.Instantiate<LevelController>();
            GetTree().Root.AddChild(level);

            isLoadingInProgress = false;
            GetTree().Paused = false;
        }, 1.0f).SetPauseMode(Tween.TweenPauseMode.Process);
    }
    public void LoadLevel(string path) => LoadLevel(ResourceLoader.Load<PackedScene>(path));


    public void GoToLevelFromMenu(PackedScene scene)
    {
        // TODO: We'll pause for now jic a battle happens AS we transition.
        GetTree().Paused = true;

        screenFade.UseDiamonds();
        screenFade.TransitionThen(() => {

            GetTree().UnloadCurrentScene();

            if (level is not null)
            {
                GetTree().Root.RemoveChild(level);
                level.QueueFree();
                level = null;
            }

            level = scene.Instantiate<LevelController>();
            GetTree().Root.AddChild(level);

            GetTree().Paused = false;
        }, 1.5f).SetPauseMode(Tween.TweenPauseMode.Process);
    }
}


// Quick access Utitlity class
static public class Game
{
    public static GameController Controller => GameController.Instance;
    public static BattleController Battle => GameController.Battle;
    public static LevelController Level => GameController.Instance.level;
    public static CanvasLayer HUD => Level.HUD;
}