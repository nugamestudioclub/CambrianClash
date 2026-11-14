using Godot;
using System;

public partial class score : Node
{
	public static score Instance;
	public int _score = 0;
	public int _lives = 3;
	
	[Signal]
	public delegate void PlayerDiedEventHandler();
	
	[Signal]
	public delegate void SpawnPlayerEventHandler(Vector2 positon);

	
	public void _AddPoints(int points)
	{
		_score += points;
	}
	
	public void _LoseLife()
	{
		EmitSignal(SignalName.PlayerDied);
		_lives -= 1;
	}

	public void _SpawnPlayer(Vector2 position)
	{
		EmitSignal(SignalName.SpawnPlayer, position);
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_lives < 0)
		{
			GetTree().ChangeSceneToFile("res://scenes/menus/game_over.tscn");
		}
	}
	
}
