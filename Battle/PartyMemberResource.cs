using Godot;
using System;

public partial class PartyMemberResource : BattlerResource
{
    [Export] public StatsResource Stats { get; set; }
}
