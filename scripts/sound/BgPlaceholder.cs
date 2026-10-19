using Godot;
using System;

public partial class BgPlaceholder : AudioStreamPlayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GetTree().Paused == true)
		{
			VolumeDb = -15;
		}
		else if (GetTree().Paused == false)
		{
			VolumeDb = 0;
		}
	}
}
