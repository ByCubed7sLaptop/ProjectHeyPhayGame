using Godot;
using System;

// Controls aspects of the level, the player platforming, platforming camera behavour, ect.
public partial class LevelController : Node2D
{
	// The current level being used
	public static LevelController Instance { get; set; }

	public PlayerController Player { get; private set; }

	[Export] public CameraController Camera;
	[Export] public Node2D PlayerSpawnPosition;

	public override void _Ready()
	{
		Instance = this;

		// Create the player
		Player = PlayerController.CreateAt(this, PlayerSpawnPosition.Position);
		Player.OnHitEnemy += OnPlayerHitEnemy;

		Camera.Target = Player;
	} 

	// Called when an Enemy hits a Player
	public void OnPlayerHitEnemy(object o, IEnemy enemy)
    {
		GD.Print($"Hit Enemy: {enemy.Resource.Name}");

		//GetTree().Paused = true;

		// Battle should NOT start if the player is over X amount of levels over the enemy


		//GameController.Instance.StartBattleWith(enemy);
		GameController.Instance.StartBattleWith(enemy);
	}
}
