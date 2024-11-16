using Godot;
using System;

public partial class PlayerHealthbar : Node
{
    [Export]
    public Label HpDisplay { get; set; }

    private int PlayerHp {  get; set; }
    private int PlayerMaxHp { get; set; }
    

    public override void _Process(double delta)
    {
        PlayerHp = Party.GetLeaderHP();
        PlayerMaxHp = Party.GetLeaderMaxHP();

        HpDisplay.Text = $"{PlayerHp} / {PlayerMaxHp}";
    }
}
