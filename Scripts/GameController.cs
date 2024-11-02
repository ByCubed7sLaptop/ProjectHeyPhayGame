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

    private BattleController Battle;
    public EncounterResource CurrentEncounter { get; set; }

    public override void _Ready()
	{
		Instance = this;
        DebugDrawer = new DebugDrawer(DebugDrawerNode);
    }

    public BattleController StartBattleWith(EncounterResource encounter)
    {
        // TODO:
        // Save/Hide/Unload current scene
        // Load battle scene
        // Transfer battle infomation from event to battle scene

        // TODO: Assumes level controller is loaded and is the main scene

        // Remove the level controller from the root node
        // This keeps it in memory but stops processing
        GetTree().Root.RemoveChild(LevelController.Instance);

        // Set up the battle scene
        CurrentEncounter = encounter;
        Battle = BattlePackedScene.Instantiate<BattleController>();
        GetTree().Root.AddChild(Battle);

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
