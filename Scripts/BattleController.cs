using Godot;
using System;
using System.Collections.Generic;

public partial class BattleController : Node2D
{
	[Export] public Camera2D Camera;
	[Export] public Node2D phayGeneralPosition;
	[Export] public Node2D enemyGeneralPosition;

	public List<Sprite2D> PartySprites { get; private set; } = new();
	public List<Sprite2D> EnemySprites { get; private set; } = new();

	public EventHandler OnWin;
	public EventHandler OnLose;

	public EncounterResource currentEncounter;

	public List<BattlerResource> turnOrder = new();
	public int turn = 0;

	public override void _Ready()
	{
		// Place all the party
		PlaceParty();
		PlaceEnemies();

		// Decide who goes first
		turnOrder.AddRange(Party.Members);
		turnOrder.AddRange(currentEncounter);

		// Play animations
	}

	public override void _Process(double delta)
    {
        MoveParty(delta);
        MoveEnemies(delta);
	}

	private void PlaceParty()
	{
		for (int i = 0; i < Party.Count; i++)
		{
			var partyMember = Party.Get(i);
			Sprite2D sprite = partyMember.GenerateBattler() as Sprite2D;
			sprite.Position = phayGeneralPosition.Position + Vector2.Left * 100 + Vector2.Left * 20 * i;

			AddChild(sprite);
			PartySprites.Add(sprite);
		}
	}

	private void PlaceEnemies()
    {
        for (int i = 0; i < currentEncounter.Count; i++)
        {
            BattlerResource enemyResource = currentEncounter[i];
            Sprite2D sprite = enemyResource.GenerateBattler() as Sprite2D;
            sprite.Position = enemyGeneralPosition.Position + Vector2.Right * 100 + Vector2.Right * 20 * i;

            AddChild(sprite);
            EnemySprites.Add(sprite);
        }
	}

	private void MoveParty(double delta)
	{
		for (int i = 0; i < PartySprites.Count; i++)
		{
			Sprite2D sprite = PartySprites[i];
			Vector2 targetPosition = phayGeneralPosition.Position
				+ GetVectorInSpiral(i, PartySprites.Count, 2, 40);

			sprite.Position = sprite.Position.MoveToward(targetPosition, 120 * 4 * (float)delta);
		}
	}

	private void MoveEnemies(double delta)
	{
		for (int i = 0; i < EnemySprites.Count; i++)
		{
			Sprite2D sprite = EnemySprites[i];
			Vector2 targetPosition = enemyGeneralPosition.Position
				+ GetVectorInSpiral(i, EnemySprites.Count, 2, 40);

			sprite.Position = sprite.Position.MoveToward(targetPosition, 120 * 4 * (float)delta);
		}
	}



	// Idealy should be its own Attack class, so that statues effects, ect can be implemented easier
	public void Attack(BattlerResource attacker, BattlerResource defender, int amount)
    {
		defender.Damage(amount);
		
		if (defender.IsDead())
		{
			if (currentEncounter.Enemies.Contains(defender))
            {
				int index = currentEncounter.Enemies.IndexOf(defender);
				currentEncounter.Enemies.RemoveAt(index);
				EnemySprites[index].QueueFree();
				EnemySprites.RemoveAt(index);
			}
			
			else if (Party.Contains(defender as PartyMemberResource)) {
				// TODO: Logic of party member fainting
            }
		}

		// No more enemies left
		if (currentEncounter.Count == 0)
			Win();
	}


	// Move to the next turn
	public void NextTurn()
    {

    }




	public void PlayerAction(string actionName)
	{
		GD.Print(actionName);

		// Can only be called on players turn

		if (actionName == "Attack")
        {
			Attack(turnOrder[turn], currentEncounter.Enemies[0], turnOrder[turn].Stats.Power);
		}

		if (currentEncounter[0].IsDead())
        {
			currentEncounter.Enemies.RemoveAt(0);
			EnemySprites[0].QueueFree();
			EnemySprites.RemoveAt(0);

		}

		if (currentEncounter.Count == 0)
			Win();
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
