using Godot;
using System;

public partial class StatsResource : Resource
{
    [Export] public int Health { get; set; } = 12; // Health
    [Export] public int HealthMax { get; set; } = 12;
    [Export] public int Cutie { get; set; } = 10; // QT points / Special points
    [Export] public int CutieMax { get; set; } = 10;
    [Export] public int Power { get; set; } = 2; // Attack power
    [Export] public int Defence { get; set; } = 3;
    [Export] public int Speed { get; set; } = 1;
    //[Export] public int Friendship;
    [Export] public int Luck { get; set; } = 2;
    [Export] public int Exp { get; set; } = 0; // Experience

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


