using Godot;
using System;

public partial class border_1 : Area2D
{

	[Export] private bool _LeftSide = true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			Vector2 position = body.Position;
			if (_LeftSide == true)
			{
				position.X = 1160.0f;
			} else
			{
				position.X = -8.0f;
			}
			body.Position = position;
		}
	}
}
