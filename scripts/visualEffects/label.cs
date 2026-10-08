using Godot;
using System;

public partial class label : Label
{
	[Export] private int _point_val;
	
	public override async void _Ready()
	{
		// Sets score value to the score value of enemy eaten
		
		// Start a 1-second timer once when the node enters the tree
		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
		
		// Ensure the node wasn't freed while waiting before calling QueueFree
		if (GodotObject.IsInstanceValid(this))
		{
			QueueFree();
		}
	}

	public override void _Process(double delta)
	{
		// Move upward smoothly frame-by-frame (frame-rate independent)
		Position += Vector2.Up * 50.0f * (float)delta;
	}
}
