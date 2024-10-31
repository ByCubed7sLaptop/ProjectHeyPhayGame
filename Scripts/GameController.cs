using Godot;
using System;

// Deals with the overall game, saving, loading, platforming, battles, pausing, ect.
public partial class GameController : Node
{
	public static GameController Instance { get; set; }

	[Export] public PackedScene LevelPackedScene;
	[Export] public PackedScene BattlePackedScene;

    public DebugDrawer DebugDrawer { get; set; }
    [Export] DebugDrawerNode DebugDrawerNode { get; set; }

    public EncounterResource CurrentEncounter { get; set; }

    public override void _Ready()
	{
		Instance = this;
        DebugDrawer = new DebugDrawer(DebugDrawerNode);
    }

    public void StartBattleWith(EncounterResource encounter)
    {
        // TODO:
        // Save/Hide/Unload current scene
        // Load battle scene
        // Transfer battle infomation from event to battle scene
        CurrentEncounter = encounter;
        GetTree().ChangeSceneToPacked(BattlePackedScene);
    }

    public void TransferToLevel()
    {
        GetTree().ChangeSceneToPacked(LevelPackedScene);
        //Level.Enable();
        //Battle.Disable();
    }
}
