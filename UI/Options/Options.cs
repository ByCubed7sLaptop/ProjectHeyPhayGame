using Godot;
using System;
using System.Collections.Generic;

public enum OptionDifficulty { Easy, Medium, Hard }
public enum OptionWindowDisplay { FullScreen, Windowed, Borderless }
public enum OptionGraphicsQuality { Low, Medium, High, Ultra }
public enum OptionColorblindMode { None, Protanopia, Deuteranopia, Tritanopia }

public partial class Options
{
	[OptionGroup("Gameplay & Accessibility")]
	public OptionDifficulty Difficulty {get; set;} = OptionDifficulty.Easy;
	public OptionColorblindMode ColorBlindMode {get; set;} = OptionColorblindMode.None;
	public double SpeedModifier {get; set;} = 1.0f;
	[OptionDesc("Extra time slowdown for speed critical moments")]
	public double SpeedCritModifier {get; set;} = 1.0f;
	public bool GodMode {get; set;} = false;

	[OptionGroup("Display")]
	public Vector2I ScreenResolution {get; set;} = new(1152, 648);
	public OptionWindowDisplay WindowDisplay {get; set;} = OptionWindowDisplay.Windowed;
	public int FrameRate {get; set;} = 60;
	public bool ScreenShake {get; set;} = true;
	public double Brightness {get; set;} = 1.0f;

	[OptionGroup("Volume")]
	[OptionSlider] public double MasterVolume {get; set;} = 0.0f;
	[OptionSlider] public double MusicVolume {get; set;} = 1.0f;
	[OptionSlider] public double UIVolume {get; set;} = 1.0f;
	[OptionSlider] public double PlayerVolume {get; set;} = 1.0f;
	[OptionSlider] public double AmbientVolume {get; set;} = 1.0f;

	[OptionGroup("Video")]
	public OptionGraphicsQuality GraphicsQuality {get; set;} = OptionGraphicsQuality.Medium;
	public bool Vsync {get; set;} = false;
	public bool Bloom {get; set;} = true;
	public bool PostProcessing {get; set;} = true;

	[OptionGroup("Controls")]
	// Null gets (default) controls from inputmap
	public List<Object> ControlPlayerJump {get; set;} = null;

}
