using Godot;
using System.Collections;
using System.Collections.Generic;

public partial class EncounterResource : Resource, IEnumerable<EnemyResource>
{
    [Export] public Godot.Collections.Array<EnemyResource> Enemies = new();

    public IEnumerator<EnemyResource> GetEnumerator()
    {
        return Enemies.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}
