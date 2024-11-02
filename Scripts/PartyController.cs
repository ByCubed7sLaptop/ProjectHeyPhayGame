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

    static public IEnumerator<PartyMemberResource> Members 
        => PartyController.Instance.GetEnumerator();
}