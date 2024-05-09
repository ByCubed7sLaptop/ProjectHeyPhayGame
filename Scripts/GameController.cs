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
		//Debug_StartBattleWithExample();
	}

	public void StartBattleWith(EncounterResource encounter)
	{
		// TODO:
		// Save/Hide/Unload current scene
		// Load battle scene
		// Transfer battle infomation from event to battle scene

		TransferToBattle();

		Battle.Setup();

		foreach (var enemy in encounter)
			Battle.AddEnemy(enemy);

		Battle.Start();

		//TransferToLevel();
        //var tween = CreateTween();
        //tween.TweenInterval(3);
        //tween.TweenCallback(Callable.From(() =>
        //{
        //    TransferToLevel();
        //}));

    }

	public void TransferToBattle()
	{
		Battle.Enable();
		Level.Disable();
	}

	public void TransferToLevel()
	{
		Level.Enable();
		Battle.Disable();
	}


	[Export] EncounterResource Encounter;
	public void Debug_StartBattleWithExample()
	{
		StartBattleWith(Encounter);
	}
}
