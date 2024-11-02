using Godot;
using System.Collections;
using System.Collections.Generic;

public partial class EncounterResource : Resource, IEnumerable<BattlerResource>
{
    [Export] public Godot.Collections.Array<BattlerResource> Enemies = new();

    public IEnumerator<BattlerResource> GetEnumerator()
    {
        return Enemies.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    public int Count => Enemies.Count;

    public BattlerResource this[int i]
    {
        get { return Enemies[i]; }
        set { Enemies[i] = value; }
    }
}
