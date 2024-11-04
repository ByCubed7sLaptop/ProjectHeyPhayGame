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

    public BattleController battle;


    public override void _Ready()
	{
		Instance = this;
        DebugDrawer.Initialization();
    }

    public BattleController StartBattleWith(EncounterBody encounter)
    {
        // TODO: Assumes level controller is loaded and is the main scene

        // Remove the level controller from the root node
        // This keeps it in memory but stops processing
        GetTree().Root.RemoveChild(LevelController.Instance);

        // Deep copy the encounter data
        
        // Set up the battle scene
        battle = BattlePackedScene.Instantiate<BattleController>();

        //battle.currentEncounter = encounter.Resource.Duplicate(true) as EncounterResource;
        battle.currentEncounter = encounter.Resource.Duplicate(true) as EncounterResource;

        GetTree().Root.AddChild(battle); 

        // TODO: Move to EncounterBody destroy method to add effects / ect
        battle.OnWin += (e, o) => encounter.QueueFree();
        
        battle.OnWin += (e, o) => TransferToLevel();

        return battle;
    }

    public void TransferToLevel()
    {
        // TODO: Assumes we're on the battle scene
        GetTree().Root.RemoveChild(battle);
        GetTree().Root.AddChild(LevelController.Instance);
    }
}
