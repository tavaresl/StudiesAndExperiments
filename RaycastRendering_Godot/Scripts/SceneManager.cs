using Godot;
using System;

namespace Scripts;

public partial class SceneManager : Node
{
	private bool _paused = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent)
		{
			if (keyEvent.Pressed && keyEvent.Keycode == Key.Escape)
			{
				_paused = !_paused;
			}
		}

		if (_paused)
		{
			DisplayServer.MouseSetMode(DisplayServer.MouseMode.Visible);
		}
		else
		{
			DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
