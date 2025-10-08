using Godot;
using System;
using System.Collections.Generic;

public partial class InputRemapper : Node {
	private string listeningFor = String.Empty;

	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.Always;
		SetProcessInput(false);
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (listeningFor == String.Empty)
			throw new Exception("InputRemapper is not listening for anything but input was still called!");

		if (inputEvent is InputEventMouseMotion) return;
		if (!InputMap.HasAction(listeningFor)) return;

		//# Remap the action's input event.
		InputMap.ActionEraseEvents(listeningFor);
		InputMap.ActionAddEvent(listeningFor, inputEvent);

		StopListening();
	}


	public void StartListening(String action) {
		if (!InputMap.HasAction(action))
			return;

		listeningFor = action;
		SetProcessInput(true);
	}

	public void StopListening() {
		if (listeningFor == String.Empty)
			throw new Exception("InputRemapper was told to stop, but no remapping was in progress!");

		listeningFor = String.Empty;
		SetProcessInput(false);
	}

}
