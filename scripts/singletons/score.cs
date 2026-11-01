using Godot;
using System;

public partial class score : Node
{
	public static score Instance;
	public int _score = 0;
	public int _lives = 3;
	
	[Signal]
	public delegate void PlayerDiedEventHandler();
	
	public void _AddPoints(int points)
	{
		_score += points;
	}
	
	public void _LoseLife()
	{
		EmitSignal(SignalName.PlayerDied);
		_lives -= 1;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
}
