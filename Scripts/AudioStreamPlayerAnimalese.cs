using Godot;
using System;
using System.Linq;

public partial class AudioStreamPlayerAnimalese : AudioStreamPlayer
{
    [Export] public string text;
    [Export] public bool shorten;
    [Export] public double maxLetterSpan = 0.15;

    private double libraryLetterSecs = 0.15;

    private int currentLetter;
    private double playtime = 0;

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (text is null) return;

        if (playtime > Mathf.Min(maxLetterSpan, libraryLetterSecs / PitchScale))
        {
            if (currentLetter >= text.Length)
            {
                Stop();
                return;
            }

            PlayLetter(text[currentLetter]);
            currentLetter++;
            playtime = 0;
        }

        playtime += delta;
    }


    private void PlayLetter(char letter)
    {
        letter = char.ToUpper(letter);
        if (letter >= 'A' && letter <= 'Z')
        {
            float position = (float)libraryLetterSecs * (letter - 'A');
            Play(position);

        }
        else
        {
            Stop();
        }
        GD.Print($"playing: {letter}");
    }
}