using Godot;
using System;

public partial class SliderOption : BoxContainer, IOptionNode<double>
{
	[Export] public Label label;
	[Export] public Slider slider;

	public double Value { get { return _value; } set { _value = value; slider.Value = _value; } }
	private double _value = 0;
	
	public event EventHandler<OptionEventArgs<double>>? ValueChanged;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		slider.ValueChanged += OnSliderValueChanged;
	}

	public void OnSliderValueChanged(double value) {
		ValueChanged?.Invoke(this, new () { Value = value});
	}

	public void SetOptionName(string name)
	{
		label.Text = name;
	}
}
