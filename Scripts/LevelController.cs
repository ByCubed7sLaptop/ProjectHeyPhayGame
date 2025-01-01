using Godot;
using System;

// Controls aspects of the level, the player platforming
public partial class LevelController : Node2D
{
	// The current level being used
	public static LevelController Instance { get; private set; }
	private static Vector2I? PlayerSpawnPositionOverride = null;

    public Player Player { get; private set; }
	public CanvasLayer HUD { get; private set; }

    [Export] public LevelCamera Camera;
	[Export] public Node2D PlayerSpawnPosition;

	public override void _Ready()
	{
		Instance = this;

		// Create the player
		if (PlayerSpawnPositionOverride is null)
			Player = Player.CreateAt(this, PlayerSpawnPosition.Position);
		else
		{
			Player = Player.CreateAt(this, (Vector2I)PlayerSpawnPositionOverride);
			PlayerSpawnPositionOverride = null;
        }

		Player.OnHitEncounter += OnPlayerHitEncounter;

		Camera.Target = Player;

        // Load level HUD
        HUD = Game.Controller.LevelHudPackedScene.Instantiate<CanvasLayer>();
        AddChild(HUD);

    }

	public void Enable()
	{
		// Toggle level node
		ProcessMode = ProcessModeEnum.Inherit;
		Show();
		Camera.Enabled = true;
	}

    public void Disable()
	{
		// Toggle level node
		ProcessMode = ProcessModeEnum.Disabled;
        Hide();
        Camera.Enabled = false;
    }

    // Called when an Enemy hits a Player
    public void OnPlayerHitEncounter(object o, EncounterBody encounterBody)
    {
		GD.Print($"Hit Enemy: {encounterBody.Resource.Enemies}");

		// Battle should NOT start if the player is over X amount of levels over the enemy
		// This should be togglable

		// Tell the game controller to start the battle mode
		Game.Controller.StartBattleWith(encounterBody);
	}

	public void OverrideNextPlayerSpawnPosition(Vector2I position)
	{
        PlayerSpawnPositionOverride = position;
    }
}
