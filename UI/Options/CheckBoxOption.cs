using Godot;
using System;

public partial class CheckBoxOption : BoxContainer, IOptionNode<bool>
{
	[Export] public Label label;
	[Export] public CheckBox checkbox;

	public bool Value { get { return _value; } set { _value = value; checkbox.ButtonPressed = _value; } }
	private bool _value = false;
	
	public event EventHandler<OptionEventArgs<bool>>? ValueChanged;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		checkbox.Toggled += OnCheckBoxToggled;
	}

	public void OnCheckBoxToggled(bool value) {
		ValueChanged?.Invoke(this, new () { Value = value});
	}

	public void SetOptionName(string name)
	{
		label.Text = name;
	}
}
