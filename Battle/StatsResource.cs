using Godot;
using System;

public partial class StatsResource : Resource
{
    [Export] public int Health;
    [Export] public int Cutie; // qt points / Special points
    [Export] public int Power; // Attack power
    [Export] public int Defence;
    [Export] public int Speed;
    //[Export] public int Friendship;
    [Export] public int Luck;
    [Export] public int Exp; // Experience

    // Decides the amount of exp required to move to the next level
    // 3 would be mean the first level requires 7 xp, then 16 xp, 27, 40, ect.
    const int LEVEL_EXPONENT = 3;
    const int LEVEL_EXPONENT_SQD = LEVEL_EXPONENT * LEVEL_EXPONENT;
    public int GetLevel()
    {
        return (int)Math.Sqrt(Exp + LEVEL_EXPONENT_SQD) - LEVEL_EXPONENT;
    }

    public void SetLevel(int value)
    {
        Exp = (int)Math.Pow(value + LEVEL_EXPONENT, 2) - LEVEL_EXPONENT_SQD;
    }

}


