using Godot;
using System;

public partial class PlayerEgg : CharacterBody2D
{
	[Export] private PackedScene _newPlayer;
	private float _gravityFactor = 0.01f;
	private int _velocityCap = 150;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 velocity = Velocity;
		velocity += GetGravity() * (float)delta * _gravityFactor;
		
		velocity.Y = Mathf.Clamp(velocity.Y, -_velocityCap, _velocityCap);
		Velocity = velocity;
		MoveAndSlide();
	}
}
