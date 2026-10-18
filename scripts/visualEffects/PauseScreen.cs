using Godot;
using System;

public partial class PauseScreen : CanvasLayer
{
	private void Resume()
	{
		GetTree().Paused = false;
		Visible = false;
	}
	
	private void Pause()
	{
		GetTree().Paused = true;
		Visible = true;
	}
	
	private void Toggle()
	{
		if (GetTree().Paused == true)
		{
			Resume();
		}
		else if (GetTree().Paused == false)
		{
			Pause();
		}
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			Toggle();
		}
	}
}
