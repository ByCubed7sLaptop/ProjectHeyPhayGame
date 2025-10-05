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

    static public void SetValueOnNode<T, N>(N target, string property, T value) {
        // Find node
        // Iterate/walk down node tree to find the node type from the parent


        // Set value
        

    }
}

public partial class OptionsUIManager : Node
{
	[Export] public Node target;
	//[Export] public PackedScene IntPackedScene;
	[Export] public PackedScene GroupPackedScene;
	[Export] public PackedScene FloatPackedScene;
	[Export] public PackedScene VolumePackedScene;

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
		Options options;// = GameController.Instance.options;

		options = OptionsManager.Load<Options>("C:\\Users\\ByCubed7\\Desktop\\File.json");

		// For each option
		foreach (var property in options.GetType().GetProperties()) {
			//GD.Print(property.Name, " ", property.PropertyType, " ", property.GetValue(options, null));

            // Get Attributes
            var sliderAttribute = property.GetCustomAttribute<OptionSliderAttribute>();


			// If should be seperated into seperate group:
			var groupName = property.GetCustomAttribute<OptionGroupAttribute>();
		    if (groupName != null)
		    {
				GD.Print($"Property {property.Name} has: {groupName.Group}");
				Label label = new();
				label.Text = $"-- [ {groupName.Group} ] --";
				target.AddChild(label);
		    }


            // FLOAT
			if (property.PropertyType == typeof(float))
			{
    		    if (sliderAttribute != null)
    		    {
    				GD.Print($"Adding Slider");
    				Control control = VolumePackedScene.Instantiate<Control>();
    				//label.Text = $"-- [ {groupName.Group} ] --";
    				target.AddChild(control);
                    continue;
    		    }


				Label label = new();
				label.Text = $"{property.Name}  [float]  {property.GetValue(options, null)}";
				target.AddChild(label);
			}

		}

		OptionsManager.Save("C:\\Users\\ByCubed7\\Desktop\\File.json", options);

	}
}
