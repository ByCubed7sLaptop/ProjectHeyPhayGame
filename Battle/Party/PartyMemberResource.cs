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
        // Request the cicular menu to ask the player for an action to play

        // TODO: Give the circular menu a list of the actions
        // TODO: Actions should have the option to add an icon
        // CircularMenu will run the action
        Game.Battle.CircularMenu.Target(Game.Battle.AllSprites[this], this);
    }


}
