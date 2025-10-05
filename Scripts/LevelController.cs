using Godot;
using System;

// Controls aspects of the level, the player platforming
public partial class LevelController : Node2D
{
	// The current level being used
	private static Vector2? PlayerSpawnPositionOverride = null;

	public Player Player { get; private set; }
	public CanvasLayer HUD { get; private set; }

	[Export] public LevelCamera Camera;
	[Export] public Node2D PlayerSpawnPosition;
	[Export] public Godot.Collections.Array<AudioStream> BackgroundTracks;

	public override void _EnterTree()
	{
		// Create the player
		ConstructPlayer();

		// Load level HUD
		if (HUD is null)
		{
			HUD = Game.Controller.LevelHudPackedScene.Instantiate<CanvasLayer>();
			AddChild(HUD);
		}

		Game.AudioController.TransitionUsing(BackgroundTracks);
	}

	//public override void _ExitTree()
	//{
	//    GD.Print("Level controller: _ExitTree");

	//    Player?.QueueFree();
	//    HUD?.QueueFree();

	//    Player = null;
	//    HUD = null;

	//    base._ExitTree();
	//}

	private void ConstructPlayer()
	{
		if (Player is not null)
			return;

		if (PlayerSpawnPositionOverride is null)
			Player = Player.CreateAt(this, PlayerSpawnPosition.Position);
		else
			Player = Player.CreateAt(this, (Vector2)PlayerSpawnPositionOverride);
		
		PlayerSpawnPositionOverride = null;

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
	public void OnPlayerHitEncounter(object o, EncounterBody encounterBody)
	{
		//GD.Print($"Hit Enemy: {encounterBody.Resource.Enemies}");

		// Battle should NOT start if the player is over X amount of levels over the enemy
		// This should be togglable

		// Tell the game controller to start the battle mode
		Game.Controller.StartBattleWith(encounterBody);
	}

	/// <summary>
	/// Set the player spawn position so that on next level load, the player's position is set.
	/// </summary>
	public void OverrideNextPlayerSpawnPosition(Vector2 position)
	{
		PlayerSpawnPositionOverride = position;
	}
}
