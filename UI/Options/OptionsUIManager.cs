using Godot;
using System;
using System.IO;
using System.Text.Json;
using System.Reflection;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class OptionGroupAttribute : Attribute
{
    public string Group { get; }
    public OptionGroupAttribute(string group) { Group = group; }
}

[AttributeUsage(AttributeTargets.Property)]
class OptionDescAttribute : Attribute
{
    public string Description { get; }
    public OptionDescAttribute(string description) { Description = description; }
}

[AttributeUsage(AttributeTargets.Property)]
class OptionSliderAttribute : Attribute
{
    public float Min { get; }
    public float Max { get; }
    public float Step { get; }
    public OptionSliderAttribute(float min = 0, float max = 100, float step = 1) { Min = min; Max = max; Step = step; }
}

public interface IOptionNode<T> {
    T Value { get; set; }

    // When the checkbutton gets pressed, slider gets slid, ect.
    event EventHandler<OptionEventArgs<T>> ValueChanged;

    void SetOptionName(string name);
}

public class OptionEventArgs<T> : EventArgs
{
    public T Value { get; set; }
}


public static class OptionsManager
{
	// Save and load file by the given path
	static public void Save<T>(string filepath, T data) {
		string jsonString = JsonSerializer.Serialize(data);
		File.WriteAllText(filepath, jsonString);
	}

	static public T? Load<T>(string filepath) {
		using FileStream openStream = File.OpenRead(filepath);
		return JsonSerializer.Deserialize<T>(openStream);
	}
}

public partial class OptionsUIManager : Node
{
	[Export] public Node target;
	//[Export] public PackedScene IntPackedScene;
	[Export] public PackedScene GroupPackedScene;
	[Export] public PackedScene FloatPackedScene;
	[Export] public PackedScene VolumePackedScene;
	[Export] public PackedScene BoolPackedScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ShowOptions();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ShowOptions() {
		Options options = GameController.Instance.options;

		//options = OptionsManager.Load<Options>("C:\\Users\\ByCubed7\\Desktop\\File.json");

		// For each option
		foreach (var property in options.GetType().GetProperties()) {

            var sliderAttribute = property.GetCustomAttribute<OptionSliderAttribute>();

			// If should be seperated into seperate group:
			var groupName = property.GetCustomAttribute<OptionGroupAttribute>();
		    if (groupName != null)
		    {
                Label label = GroupPackedScene.Instantiate<Label>();
				label.Text = $"-- [ {groupName.Group} ] --";
				target.AddChild(label);
		    }

            Control control;
            if (property.PropertyType == typeof(double)) {
                control = VolumePackedScene.Instantiate<Control>();

                IOptionNode<double> optionNode = control as IOptionNode<double>;
                optionNode.SetOptionName(property.Name);
                optionNode.ValueChanged += (from, args) => {

                    if (null != property && property.CanWrite)
                        property.SetValue(options, args.Value, null);

                     GD.Print(args.Value);
                 };
            }

            else if (property.PropertyType == typeof(bool)) {
                control = BoolPackedScene.Instantiate<Control>();

                IOptionNode<bool> optionNode = control as IOptionNode<bool>;
                optionNode.SetOptionName(property.Name);
                optionNode.ValueChanged += (from, args) => {

                    if (null != property && property.CanWrite)
                        property.SetValue(options, args.Value, null);

                     GD.Print(args.Value);
                 };
            }
            else continue;

            target.AddChild(control);

		}

		OptionsManager.Save("C:\\Users\\ByCubed7\\Desktop\\File.json", options);

	}
}
