using Godot;
using System;
using System.Collections.Generic;

public partial class InputRemapper : Node {
	private string listeningFor = String.Empty;
	//private InputEvent listeningEvent = null;

	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.Always;
		SetProcessInput(false);
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (listeningFor == String.Empty) return;
		if (inputEvent is InputEventMouseMotion) return;
		if (!InputMap.HasAction(listeningFor)) return;

		//# Remap the action's input event.
		//InputMap.ActionEraseEvent(listeningFor, listeningEvent);
		InputMap.ActionEraseEvents(listeningFor);
		InputMap.ActionAddEvent(listeningFor, inputEvent);

		StopListening();
	}


	public void StartListening(String action) {
		if (!InputMap.HasAction(action))
			//push_warning("Cannot remap action '%s': it does not exist in the InputMap." % action);
			return;

		//if (!InputMap.ActionHasEvent(action, inputEvent))
			//push_warning("Cannot remap action '%s': the event '%s' is not assigned to this action." % [action, event.as_text()])
			//return;
		GD.Print("Start Listening Called");

		listeningFor = action;
		//listeningEvent = inputEvent;
		SetProcessInput(true);
	}

	public void StopListening() {
		if (listeningFor == "")
			//push_warning("listen_stop() was called, but no remapping was in progress.")
			return;

		GD.Print("Stop Listening Called");

		listeningFor = "";
		//listeningEvent = null;
		SetProcessInput(false);
	}

}
