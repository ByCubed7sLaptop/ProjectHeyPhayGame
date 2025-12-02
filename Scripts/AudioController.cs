using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;


/// <summary>
/// Manages the current audio tracks being played.
/// Audios are played in groups, ie IEnumerables. This is so we can play audio
/// tracks and sfx backgrounds together easily.
/// </summary>
public partial class AudioController : Node
{
	[Export] public string BusName { get; set; } = "Music";

	/// <summary>Fadeout audio not passed in audioStreams. Fade in new audio, if any.</summary>
	public void TransitionUsing(IEnumerable<AudioStream> audioStreams, float fadeTime = 2.0f)
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
				AddAudioStream(audioStream, fadeTime, true);
	}

	/// <summary>Instantly fade to a different group.</summary>
	public void TransitionInstantlyUsing(IEnumerable<AudioStream> audioStreams)
		=> TransitionUsing(audioStreams, 0.0f);

	public void FadeOutAll()
		=> TransitionUsing(null);

	/// <summary>Add an audio track to the current group.</summary>
	public void AddAudioStream(AudioStream audioStream, float fadeTime = 2.0f, bool loop = true)
	{
		AudioStreamPlayer player = new AudioStreamPlayer();
		player.Stream = audioStream;
		player.VolumeDb = -80;
		player.Autoplay = true;
		if (loop) player.Finished += ()=>player.Play(); // Loop
		player.Bus = BusName;

		AddChild(player);

		Tween tween = null;
		FadeInPlayer(player, tween, fadeTime);
	}

	public void AddAudioStreamInstantly(AudioStream audioStream, bool loop = true)
		=> AddAudioStream(audioStream, 0.0f, loop);


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
