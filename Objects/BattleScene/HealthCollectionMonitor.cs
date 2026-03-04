using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Monitors a collection of stats and displays them.
/// </summary>
public partial class HealthCollectionMonitor : Control
{
	private List<BattlerResource> battlersToMonitor = new();
	[Export] public VBoxContainer Container { get; private set; }

	public override void _Process(double delta)
	{
		// TODO: This should only be called after each round
		//		Or damage/health/battler update
		Update();
	}

	public void Monitor(BattlerResource battler)
    {
		battlersToMonitor.Add(battler);

		// Add progress bar
		ProgressBar bar = new();
		Container.AddChild(bar);

		Update();
	}

	public void Update()
    {
		// Assumes the order of the stats to monitor is the same
		// as the order of the children

		for (int i = 0; i < battlersToMonitor.Count; i++)
        {
			StatsResource stat = battlersToMonitor[i].Stats;
			ProgressBar bar = Container.GetChild<ProgressBar>(i);

			bar.Value = stat.Health;
			bar.MaxValue = stat.HealthMax;
		}
    }
}
