using Godot;
using System;

public partial class player_spawner : Marker2D
{
	[Export] private PackedScene _newEgg;
	private bool _spawning = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(_spawning == false)
		{
			score.Instance.PlayerDied += SpawnEgg;
		}
	}
	
	private void SpawnEgg()
	{
		_spawning = true;
		if (_newEgg != null)
		{
			PlayerEgg egg = (PlayerEgg)_newEgg.Instantiate();
			egg.GlobalPosition = GlobalPosition;
			GetTree().CurrentScene.AddChild(egg);
		}
		_spawning = false;
	}
}
