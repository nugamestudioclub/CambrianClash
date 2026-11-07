using Godot;
using System;

public partial class player_spawner : Marker2D
{
	[Export] private PackedScene _newEgg;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		score.Instance.PlayerDied += SpawnEgg;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
			
		
	}
	
	private void SpawnEgg()
	{
		GD.Print("Invoked");
		if (_newEgg != null)
		{
			CallDeferred("GenerateEgg");
		}
	}

	private void GenerateEgg()
	{
		PlayerEgg egg = (PlayerEgg)_newEgg.Instantiate();
		egg.GlobalPosition = GlobalPosition;
		GetTree().CurrentScene.AddChild(egg);
	}
}
