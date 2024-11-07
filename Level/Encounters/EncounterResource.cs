using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class EncounterResource : Resource, IEnumerable<BattlerResource>
{
    [Export] public Godot.Collections.Array<BattlerResource> Enemies = new();

    // BUG: Godot Arrays and Dictionarys do not get duplicated the base
    //      method when called
    // See: https://github.com/godotengine/godot/issues/74918
    // Solution: Override the duplicate method and manually move the array
    new public Resource Duplicate(bool subresources = false)
    {
        // First duplicate the resource
        var resource = base.Duplicate(subresources) as EncounterResource;

        // Then set the new enemies array
        // Could probably dedicate a utility method for this but oh well
        Godot.Collections.Array<BattlerResource> newEnemies = new();
        foreach (var item in Enemies)
            newEnemies.Add(item.Duplicate(true) as BattlerResource);
        resource.Enemies = newEnemies;

        return resource;
    }


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

    public BattlerResource GetRandom(Random random = null)
    {
        if (random is null) random = new Random();
        return Enemies[random.Next(Enemies.Count)];
    }
}
