using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class PartyController : Node, IEnumerable<PartyMemberResource>
{
	static public PartyController Instance;
	public PartyController()
    {
		Instance = this;
	}

	[Export] public Godot.Collections.Array<PartyMemberResource> CurrentParty { get; set; }

	public int Count => CurrentParty.Count;

    public IEnumerator<PartyMemberResource> GetEnumerator()
    {
        return CurrentParty.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public PartyMemberResource this[int i]
    {
        get { return CurrentParty[i]; }
        set { CurrentParty[i] = value; }
    }
}

// Utility class
public static class Party
{
	static public int Count => PartyController.Instance.Count;

	static public PartyMemberResource Get(int index)
    {
        return PartyController.Instance[index];
    }

    static public IEnumerable<PartyMemberResource> Members 
        => PartyController.Instance;

    static public bool Contains(PartyMemberResource partyMemberResource)
    {
        return PartyController.Instance.CurrentParty.Contains(partyMemberResource);
    }

    static public PartyMemberResource RandomMember(Random random = null)
    {
        random ??= new Random();
        return Get(random.Next(Count));
    }

    static public void DamageRandomMember(int value, Random random = null)
    {
        random ??= new Random();
        var member = Get(random.Next(Count));
        member.Damage(value);
    }

    static public void DamageAllMembers(int value)
    {
        foreach (var member in Members)
            member.Damage(value);
    }

    static public bool IsDead()
    {
        foreach (var member in Members)
            if (member.Stats.Health > 0)
                return false;

        return true;
    }

    static public void FullHeal()
    {
        foreach (var member in Members)
            member.Stats.Health = member.Stats.HealthMax;
    }
}