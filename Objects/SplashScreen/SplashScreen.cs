using Godot;
public partial class SplashScreen : Control
{
	[Export] private PackedScene scene;
	[Export] private TextureRect splashTexture;

	private Tween tween;

	public override void _Ready()
	{
		tween = CreateTween();

		// Connect the 'tween_completed' signal to a method
		tween.Connect(Tween.SignalName.Finished, new Callable(this, nameof(OnTweenCompleted)));

		// Start the fade-in animation
		splashTexture.Modulate = splashTexture.Modulate with { A = 0 };
		tween.TweenProperty(splashTexture, "modulate:a", 1, 1.0f);
		tween.Play();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsPressed())
			ChangeScene();
	}

	private void OnTweenCompleted()
	{
		tween.Stop();

		// Connect the 'tween_completed' signal to a method to handle the scene change
		tween.Disconnect(Tween.SignalName.Finished, new Callable(this, nameof(OnTweenCompleted)));
		tween.Connect(Tween.SignalName.Finished, new Callable(this, nameof(ChangeScene)));

		tween.TweenInterval(2.0f);

		// Start the fade-out animation
		tween.TweenProperty(splashTexture, "modulate:a", 0, 0.5f);

		tween.TweenInterval(1.0f);

		tween.Play();
	}

	private void ChangeScene()
	{
		// Load and change to the next scene
		GetTree().ChangeSceneToPacked(scene);
	}
}
