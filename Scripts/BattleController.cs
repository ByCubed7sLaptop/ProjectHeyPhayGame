using Godot;
using System;
using System.Collections.Generic;

public partial class BattleController : Node2D
{
	[Export] public Camera2D Camera;
	[Export] public Node2D phayGeneralPosition;
	[Export] public Node2D enemyGeneralPosition;

	[Export] public Sprite2D phay;
	public List<Sprite2D> enemies = new();

	public List<EnemyResource> EnemyResources { get; private set; }

    public override void _Ready()
    {
		BattleController Battle = GetTree().CurrentScene as BattleController;
		Battle.Setup();

		foreach (var enemy in GameController.Instance.CurrentEncounter)
			Battle.AddEnemy(enemy);

		Battle.Start();

		var tween = CreateTween();
		tween.TweenInterval(3);
		tween.TweenCallback(Callable.From(() =>
		{
			GameController.Instance.TransferToLevel();
		}));
	}

    public void Setup()
    {
		EnemyResources = new List<EnemyResource>();
	}

	public void AddEnemy(EnemyResource enemy)
    {
		EnemyResources.Add(enemy);
	}

	public void Start()
    {
		// Place all the enemies
		for (int i = 0; i < EnemyResources.Count; i++)
        {
			EnemyResource enemyResource = EnemyResources[i% EnemyResources.Count];
			Sprite2D sprite = new Sprite2D();

			sprite.Name = enemyResource.Name;
			sprite.Texture = enemyResource.Texture;
			sprite.Position = enemyGeneralPosition.Position + Vector2.Right*100 + Vector2.Right * 20 * i;

			GD.Print(sprite.Position);

			AddChild(sprite);
			enemies.Add(sprite);
        }

		// Decide who goes first

		// Play animations
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

		phay.Position = phay.Position.MoveToward(phayGeneralPosition.Position, 20 * (float)delta);

		for (int i = 0; i < enemies.Count; i++)
		{
			Sprite2D sprite = enemies[i];
			Vector2 targetPosition = enemyGeneralPosition.Position 
				+ GetVectorInSpiral(i, enemies.Count, 2, 40);

			sprite.Position = sprite.Position.MoveToward(targetPosition, 120 * 4 * (float)delta);
		}
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









    private static Vector2 GetVectorInSpiral(float i, float max, float angleMulti = 2, float distance = 20)
    {
		float amount = MathF.Sqrt(i / (float)max) ;
		float angle = amount * MathF.PI * angleMulti;
		float radius = amount;

        Vector2 vector = new Vector2(
            MathF.Cos(angle), 
            MathF.Sin(angle)
        );
        return vector * radius * distance;
    }

    private static Vector2 GetVectorInCCurve(float i, float max, float distance = 20)
    {
		float angle = MathF.PI * i / max;
		angle += MathF.PI / 2;
		//angle -= i / max / 2;

		Vector2 vector = new Vector2(
			MathF.Cos(angle), 
			MathF.Sin(angle)
		);

        return vector * distance;
    }

    private static Vector2 GetVectorInRightCurve(float i, float max)
    {
		float angle = MathF.PI * i / (2 * max);
		float x = MathF.Sin(angle);
		float y = MathF.Cos(angle);
        Vector2 vector = new Vector2(x, y);

		return vector;
	}
}
