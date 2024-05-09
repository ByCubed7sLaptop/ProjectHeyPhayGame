using Godot;
using System;

// Deals with the overall game, saving, loading, platforming, battles, pausing, ect.
public partial class GameController : Node
{
	public static GameController Instance { get; set; }

	[Export] public LevelController Level;
	[Export] public BattleController Battle;

	public override void _Ready()
	{
		Instance = this;
	}

	public void StartBattleWith(EncounterResource encounter)
	{
		// TODO:
		// Save/Hide/Unload current scene
		// Load battle scene
		// Transfer battle infomation from event to battle scene

		// Disabling a CollisionObject node during a physics callback is not allowed and will cause undesired behavior.
		// Disable with call_deferred() instead.
		CallDeferred(nameof(TransferToBattleScene));
	}

	public void TransferToBattleScene()
    {
		// TODO: Move to seperate enable and disable methods

		// Show and enable level node
		Level.ProcessMode = ProcessModeEnum.Disabled;
		Level.Hide();
		Level.Camera.Enabled = false;

		// Show and enable battle node
		Battle.ProcessMode = ProcessModeEnum.Inherit;
		Battle.Show();
		Battle.Camera.Enabled = true;
	}
}
