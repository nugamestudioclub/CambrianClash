using Godot;
using System;

public partial class label : Label
{
	[Export] public int PointVal;
	[Export] private AudioStreamPlayer _sound;
	
	private async void _PlaySound()
	{
		_sound.Play();
	}
	
	public override async void _Ready()
	{
		//play sound
		_PlaySound();
		
		// Sets score value to the score value of enemy eaten
		Text = PointVal.ToString();
		// Start a 1-second timer once when the node enters the tree
		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
		
		// Ensure the node wasn't freed while waiting before calling QueueFree
		if (IsInstanceValid(this))
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
