using Godot;
using System;

// Controls aspects of the level, the player platforming
public partial class LevelController : Node2D
{
	// The current level being used
	public static LevelController Instance { get; set; }

	public Player Player { get; private set; }

	[Export] public LevelCamera Camera;
	[Export] public Node2D PlayerSpawnPosition;

	public override void _Ready()
	{
		Instance = this;

		// Create the player
		Player = Player.CreateAt(this, PlayerSpawnPosition.Position);
		Player.OnHitEncounter += OnPlayerHitEncounter;

		Camera.Target = Player;
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
    public void OnPlayerHitEncounter(object o, EncounterResource encounter)
    {
		GD.Print($"Hit Enemy: {encounter.Enemies}");

		// Battle should NOT start if the player is over X amount of levels over the enemy
		// This should be togglable

		// Tell the game controller to start the battle mode
		GameController.Instance.StartBattleWith(encounter);
	}
}
