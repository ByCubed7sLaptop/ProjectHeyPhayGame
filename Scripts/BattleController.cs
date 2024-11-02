using Godot;
using System;
using System.Collections.Generic;

public partial class BattleController : Node2D
{
	[Export] public Camera2D Camera;
	[Export] public Node2D phayGeneralPosition;
	[Export] public Node2D enemyGeneralPosition;

	public List<Sprite2D> Party { get; private set; } = new();
	public List<Sprite2D> enemies = new();

	public List<PartyMemberResource> PartyResources { get; private set; }

	public EventHandler OnWin;
	public EventHandler OnLose;

	public override void _Ready()
    {
		Start();

		// TEMP		
		var tween = CreateTween();
		tween.TweenInterval(3);
		tween.TweenCallback(Callable.From(() =>
		{
			Win();
		}));
	}

	public override void _Process(double delta)
	{
		for (int i = 0; i < Party.Count; i++)
		{
			Sprite2D sprite = Party[i];
			Vector2 targetPosition = phayGeneralPosition.Position
				+ GetVectorInSpiral(i, Party.Count, 2, 40);

			sprite.Position = sprite.Position.MoveToward(targetPosition, 120 * 4 * (float)delta);
		}
		//phay.Position = phay.Position.MoveToward(phayGeneralPosition.Position, 20 * (float)delta);

		for (int i = 0; i < enemies.Count; i++)
		{
			Sprite2D sprite = enemies[i];
			Vector2 targetPosition = enemyGeneralPosition.Position
				+ GetVectorInSpiral(i, enemies.Count, 2, 40);

			sprite.Position = sprite.Position.MoveToward(targetPosition, 120 * 4 * (float)delta);
		}
	}

	public void Start()
    {

		// Place all the party
		for (int i = 0; i < GameController.Instance.CurrentParty.Count; i++)
		{
			PartyMemberResource partyMember = GameController.Instance.CurrentParty[i];
			Sprite2D sprite = new Sprite2D();

			sprite.Name = partyMember.Name;
			sprite.Texture = partyMember.Texture;
			sprite.Position = phayGeneralPosition.Position + Vector2.Left * 100 + Vector2.Left * 20 * i;

			AddChild(sprite);
			Party.Add(sprite);
		}

		// Place all the enemies
		for (int i = 0; i < GameController.Instance.CurrentEncounter.Count; i++)
        {
			BattlerResource enemyResource = GameController.Instance.CurrentEncounter[i];
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



	private void Win()
	{
		var handler = OnWin;
		OnWin?.Invoke(this, EventArgs.Empty);
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
