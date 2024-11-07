using Godot;
using System;

public partial class PartyMemberResource : BattlerResource
{
    public override Node2D GenerateBattler()
    {
        // TODO: Override with the generation of the party members packed scene
        return base.GenerateBattler();
    }

    public override void OnBattleTurnStart()
    {
        // Pick between our actions

        // Play the animation, run the attack ect.

        // For now, just attack a random party member


        Game.Battle.CircularMenu.Target(Game.Battle.AllSprites[this]);
    }


}
