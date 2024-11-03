using Godot;
using System;
using System.Collections.Generic;

// Deals with the overall game, saving, loading, platforming, battles, pausing, ect.
public partial class GameController : Node
{
	public static GameController Instance { get; set; }

	[Export] public PackedScene LevelPackedScene;
	[Export] public PackedScene BattlePackedScene;

    private BattleController Battle;
    
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
        Battle = BattlePackedScene.Instantiate<BattleController>();
        Battle.currentEncounter = encounter.Resource.Duplicate(true) as EncounterResource;
        GetTree().Root.AddChild(Battle);

        // TODO: Move to EncounterBody destroy method to add effects / ect
        Battle.OnWin += (e, o) => encounter.QueueFree();
        
        Battle.OnWin += (e, o) => TransferToLevel();

        return Battle;
    }

    public void TransferToLevel()
    {
        // TODO: Assumes we're on the battle scene
        GetTree().Root.RemoveChild(Battle);
        GetTree().Root.AddChild(LevelController.Instance);
    }
}
