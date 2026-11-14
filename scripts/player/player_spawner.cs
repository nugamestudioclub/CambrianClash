using Godot;
using System;

public partial class player_spawner : Marker2D
{
	[Export] private PackedScene _newEgg;
	private PlayerEgg _egg;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		score.Instance.PlayerDied += SpawnEgg;
		score.Instance.SpawnPlayer += DestroyEgg;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void DestroyEgg(Vector2 position)
	{
		_egg.QueueFree();
	}
	
	private void SpawnEgg()
	{
		if (_newEgg != null)
		{
			CallDeferred("GenerateEgg");
		}
	}
	
	

	private void GenerateEgg()
	{
		_egg = (PlayerEgg)_newEgg.Instantiate();
		_egg.GlobalPosition = GlobalPosition;
		GetTree().CurrentScene.AddChild(_egg);
	}
}
