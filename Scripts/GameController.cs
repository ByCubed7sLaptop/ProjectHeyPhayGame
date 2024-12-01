using Godot;
using System;
using System.Collections.Generic;

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

    public ScreenFade screenFade;

    public override void _Ready()
	{
		Instance = this;
        DebugDrawer.Initialization();
        screenFade = GetNode<ScreenFade>("ScreenFade");
    }

    public BattleController StartBattleWith(EncounterBody encounter)
    {
        // TODO: Assumes level controller is loaded and is the main scene

        // Remove the level controller from the root node
        // This keeps it in memory but stops processing
        GetTree().Root.RemoveChild(LevelController.Instance);

        // Set up the battle scene
        battle = BattlePackedScene.Instantiate<BattleController>();

        //battle.currentEncounter = encounter.Resource.Duplicate(true) as EncounterResource;
        battle.currentEncounter = encounter.Resource.Duplicate(true) as EncounterResource;

        GetTree().Root.AddChild(battle); 

        // TODO: Move to EncounterBody destroy method to add effects / ect
        battle.OnEnd += (e, o) => encounter.QueueFree();
        battle.OnEnd += (e, o) => TransferToLevel();

        // TODO: Change to game over scene or respawn at room enterence, ect.
        battle.OnLose += (e, o) => Party.FullHeal();

        return battle;
    }

    public void TransferToLevel()
    {
        // TODO: Assumes we're on the battle scene
        GetTree().Root.RemoveChild(battle);
        GetTree().Root.AddChild(LevelController.Instance);
    }



    public void LoadLevel(PackedScene scene)
    {
        GetTree().ChangeSceneToPacked(scene);
    }
    public void LoadLevel(string path)
    {
        var scene = ResourceLoader.Load<PackedScene>(path);
        LoadLevel(scene);
    }
}


// Quick access Utitlity class
static public class Game
{
    public static GameController Controller => GameController.Instance;
    public static BattleController Battle => GameController.Battle;
    public static LevelController Level => LevelController.Instance;
    public static CanvasLayer HUD => Level.HUD;
}