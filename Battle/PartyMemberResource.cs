using Godot;
using System;

public partial class PartyMemberResource : BattlerResource
{
    [Export] public StatsResource Stats { get; set; }

    public override Node2D GenerateBattler()
    {
        // TODO: Override with the generation of the party members
        return base.GenerateBattler();
    }
}
