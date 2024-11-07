using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Incharge of keeping record of turn orders.
/// </summary>
public class BattleTurn
{
	public enum StartOrder
	{
		PlayerGoesFirst,
		EnemyGoesFirst,
		Random
	}
	public StartOrder startOrder = StartOrder.PlayerGoesFirst;
	private List<BattlerResource> order;
	private int turn = 0;

	public int Count { get => turn; }

	public void Setup(IEnumerable<BattlerResource> partyMembers, IEnumerable<BattlerResource> encounterMembers)
	{
		order = new List<BattlerResource>();
		// Decide who goes first
		if (startOrder is StartOrder.PlayerGoesFirst)
		{
			order.AddRange(partyMembers);
			order.AddRange(encounterMembers);
		}
		else if (startOrder is StartOrder.EnemyGoesFirst)
		{
			order.AddRange(encounterMembers);
			order.AddRange(partyMembers);
		}
		else // Randomize
		{
			order.AddRange(encounterMembers);
			order.AddRange(partyMembers);

			// Shuffle
			// TODO: Random should be passed to
			Random random = new Random();
			order.Sort((a, b) => random.Next());
		}
	}

	public BattlerResource GetBattler()
	{
		return order[turn % order.Count];
	}

	public void End()
	{
		turn++;
		InvokeOnNextTurn();
	}

	// Events

	public event EventHandler OnNextTurn;
	private void InvokeOnNextTurn()
	{
		var handler = OnNextTurn;
		handler?.Invoke(this, EventArgs.Empty);
	}
}
