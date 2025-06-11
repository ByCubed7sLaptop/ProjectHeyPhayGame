using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

public partial class AudioController : Node
{
    public void TransitionUsing(IEnumerable<AudioStream> audioStreams)
    {
        if (audioStreams is null)
        {
            foreach (AudioStream audioStream in CurrentAudioStreams())
                RemoveAudioStream(audioStream);
            return;
        }

        // First, check what tracks to remove
        foreach (AudioStream audioStream in CurrentAudioStreams())
            if (!audioStreams.Contains(audioStream))
                RemoveAudioStream(audioStream);

        // Add any tracks that arent playing
        var current = CurrentAudioStreams().ToList();
        foreach (AudioStream audioStream in audioStreams)
            if (!current.Contains(audioStream))
                AddAudioStream(audioStream);
    }


    public void AddAudioStream(AudioStream audioStream)
    {
        AudioStreamPlayer player = new AudioStreamPlayer();
        player.Stream = audioStream;
        player.VolumeDb = -80;
        player.Autoplay = true;
        player.Finished += ()=>player.Play(); // Loop
        AddChild(player);

        Tween tween = null;
        FadeInPlayer(player, tween, 2.0f);
    }

    private void Player_Finished()
    {
        throw new NotImplementedException();
    }

    public void RemoveAudioStream(AudioStream audioStream)
    {
        var player = FindPlayerOrNull(audioStream);

        Tween tween = null;
        FadeOutPlayer(player, tween, 2.0f);
        FreePlayer(player, tween, 2.0f);
    }


    static public void FadeInPlayer(AudioStreamPlayer audioStreamPlayer, Tween tween = null, float duration = 0.0f)
    {
        tween ??= audioStreamPlayer.CreateTween();
        tween.TweenProperty(audioStreamPlayer, "volume_db", 0, duration);
    }

    static public void FadeOutPlayer(AudioStreamPlayer audioStreamPlayer, Tween tween = null, float duration = 0.0f)
    {
        tween ??= audioStreamPlayer.CreateTween();
        tween.TweenProperty(audioStreamPlayer, "volume_db", -80, duration);
    }

    static public void FreePlayer(AudioStreamPlayer audioStreamPlayer, Tween tween = null, float duration = 0.0f)
    {
        tween ??= audioStreamPlayer.CreateTween();
        tween.TweenInterval(duration);
        tween.TweenCallback(new Callable(audioStreamPlayer, "queue_free"));
    }


    /// <summary>
    /// Return an IEnumerable of all of the audio streams
    /// </summary>
    private IEnumerable<AudioStream> CurrentAudioStreams()
    {
        Godot.Collections.Array<Node> children = GetChildren();

        foreach (var child in children)
        {
            if (child is not AudioStreamPlayer audioStreamPlayer)
                continue;

            if (audioStreamPlayer.Stream is null)
                continue;

            yield return audioStreamPlayer.Stream;
        }
    }


    /// <summary>
    /// Finds the AudioStreamPlayer that is playing the given audio. Or null if none is found.
    /// </summary> 
    private AudioStreamPlayer FindPlayerOrNull(AudioStream audioStream)
    {
        Godot.Collections.Array<Node> children = GetChildren();

        foreach (var child in children)
        {
            if (child is not AudioStreamPlayer audioStreamPlayer)
                continue;

            if (audioStreamPlayer.Stream == audioStream)
                return audioStreamPlayer;
        }

        return null;
    }
}
