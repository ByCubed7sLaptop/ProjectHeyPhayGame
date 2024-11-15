using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BattleController : Node2D
{
	[Export] public Camera2D Camera;
	[Export] public Node2D phayGeneralPosition;
	[Export] public Node2D enemyGeneralPosition;
	[Export] public CircularMenu CircularMenu { get; set; }
	[Export] public TargetSelector TargetSelector { get; set; }
	[Export] public HealthCollectionMonitor PartyHealthMonitor { get; set; }
	[Export] public HealthCollectionMonitor EnemyHealthMonitor { get; set; }

	public List<Sprite2D> PartySprites { get; private set; } = new();
	public List<Sprite2D> EnemySprites { get; private set; } = new();
	public Dictionary<BattlerResource, Sprite2D> AllSprites { get; private set; } = new();

	public EventHandler OnWin;
	public EventHandler OnLose;

	public EncounterResource currentEncounter;

	[Export] public BattleTurn.StartOrder turnOrderStart = BattleTurn.StartOrder.PlayerGoesFirst;
	public BattleTurn Turn { get; private set; } = new BattleTurn();
    public EncounterResource Encounter => currentEncounter;

    public override void _Ready()
    {
		Turn.startOrder = turnOrderStart;
		Turn.Setup(Party.Members, currentEncounter);

		PlaceParty();
        PlaceEnemies();

		// Monitor the health
		foreach (var member in Party.Members)
			PartyHealthMonitor.Monitor(member);

		foreach (var enemy in currentEncounter.Enemies)
			EnemyHealthMonitor.Monitor(enemy);


		Turn.OnNextTurn += Turn_OnNextTurn;
		Turn.GetBattler().OnBattleTurnStart();

		// Play animations

	}

	public void Turn_OnNextTurn(object e, EventArgs args)
	{
		// Check enemy wellbeing
		List<BattlerResource> enemiesToRemove = new ();
		foreach (var enemy in currentEncounter.Enemies)
		{ 
			if (enemy.IsDead())
			{
				enemiesToRemove.Add(enemy);
				GD.Print($"and died");
			}
		}
		foreach (var enemy in enemiesToRemove)
		{
			int index = currentEncounter.Enemies.IndexOf(enemy);
			currentEncounter.Enemies.RemoveAt(index);
			AllSprites.Remove(enemy);
			EnemySprites[index].QueueFree();
			EnemySprites.RemoveAt(index);

			// Remove from turn order
			Turn.Remove(enemy);
		}

		// Check party wellbeing
		foreach (var member in Party.Members)
        {
			if (member.IsDead())
			{
				// TODO: Logic of party member fainting
				GD.Print($"and fainted");

				// Remove from turn order
				//Turn.Remove(member);
			}
        }

		// TODO: What happens if both the party and enemy dies?
		if (Party.IsDead())
		{
            GD.Print("Player lost!");

            // Win after 2 seconds
            Tween tween = CreateTween();
            tween.TweenInterval(2);
            tween.TweenCallback(Callable.From(() =>
                Lose()
            ));
        }

		// No more enemies left
		else if (currentEncounter.Count == 0)
		{
			GD.Print("Player won!");

			// Win after 2 seconds
			Tween tween = CreateTween();
			tween.TweenInterval(2);
			tween.TweenCallback(Callable.From(() =>
				Win()
			));
		}
		else
        {
			// Tell the battler to do their turn
			Turn.GetBattler().OnBattleTurnStart();
		}
	}


	public override void _Process(double delta)
    {
		// Move the sprites to their targetted location
        MoveParty(delta);
        MoveEnemies(delta);
	}

	/// <summary>
	/// Place down the party's sprites
	/// </summary>
	private void PlaceParty()
	{
		for (int i = 0; i < Party.Count; i++)
		{
			var partyMember = Party.Get(i);
			Sprite2D sprite = partyMember.GenerateBattler() as Sprite2D;
			sprite.Position = phayGeneralPosition.Position + Vector2.Left * 100 + Vector2.Left * 20 * i;

			AddChild(sprite);
			PartySprites.Add(sprite);
			AllSprites[partyMember] = sprite;
		}
	}


	/// <summary>
	/// Place down the enemy's sprites
	/// </summary>
	private void PlaceEnemies()
    {
        for (int i = 0; i < currentEncounter.Count; i++)
        {
            BattlerResource enemyResource = currentEncounter[i];
            Sprite2D sprite = enemyResource.GenerateBattler() as Sprite2D;
            sprite.Position = enemyGeneralPosition.Position + Vector2.Right * 100 + Vector2.Right * 20 * i;

            AddChild(sprite);
            EnemySprites.Add(sprite);
			AllSprites[enemyResource] = sprite;

		}
	}

	/// <summary>
	/// Move the party's sprite to their target location
	/// </summary>
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


	/// <summary>
	/// Move the enemy's sprite to their target location
	/// </summary>
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

	/// <summary>
	/// Tells the current turn owner to attack the given defender
	/// </summary>
	public void Attack(BattlerResource attacker, BattlerResource defender)
	{
		// TODO: Create attack amount equation
		// TODO: Implement crits
		int amount = attacker.Stats.Power;

		defender.Damage(amount);

		GD.Print($"{attacker.DisplayName} attacks {defender.DisplayName} and deals {amount} damage ({defender.Stats.Health} left)");
    }

    /// <summary>
    /// Can be called on either turn
    /// </summary>
    public void RequestChooseTarget(Action<BattlerResource> callback)
    {
        if (Game.Battle.Turn.IsPartysTurn())
		{
            // Request the ui targeter and the player to target an enemy
            TargetSelector.Setup(EnemySprites);
            TargetSelector.Show();

            // Subscribe temporarily using a lambda to handle callback and clean up automatically
            void TargetSelector_OnTargetSelected(object o, TargetSelector.TargetElement element)
            {
                TargetSelector.OnTargetSelected -= TargetSelector_OnTargetSelected;
                BattlerResource resource = Game.Battle.Encounter[element.Index];
                TargetSelector.Hide();
                callback(resource);
            }

            TargetSelector.OnTargetSelected += TargetSelector_OnTargetSelected;
        }

        else
		{
			BattlerResource resource = Game.Battle.RandomOpponent();
            // On choose target, call the callback method
            callback(resource);
        }
    }


    public IEnumerator<BattlerResource> Enemies
    {
        get
        {
            foreach (var enemy in currentEncounter.Enemies)
                yield return enemy;
        }
    }


    public IEnumerator<BattlerResource> Opponents
    {
        get
        {
			if (Turn.IsPartysTurn())
				foreach (var enemy in currentEncounter.Enemies)
					yield return enemy;
            else
                foreach (var member in Party.Members)
                    yield return member;
        }
    }


    public BattlerResource RandomOpponent(Random random = null)
    {
		if (Turn.IsPartysTurn())
			return currentEncounter.GetRandom(random);
		
		// Is enemies turn, return member
		return Party.RandomMember(random);
    }





    private void Win()
	{
		var handler = OnWin;
		handler?.Invoke(this, EventArgs.Empty);
	}

	private void Lose()
	{
		var handler = OnLose;
		handler?.Invoke(this, EventArgs.Empty);
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
