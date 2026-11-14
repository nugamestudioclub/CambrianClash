using Godot;
using System;

public partial class new_enemy : CharacterBody2D
{
	private float _knockbackPower = 150;
	[Export] private area_2d _enemy;
	// Called when the node enters the scene tree for the first time.
	private void Knockback()
	{
		Vector2 velocity = Velocity;
		velocity = new Vector2(-1, 0);
		Vector2 knockbackDirection = velocity.Normalized() * _knockbackPower;
		velocity = knockbackDirection;
		MoveAndSlide();
	}
	public override void _Ready()
	{
		_enemy.Hit += Knockback;
	}

	
}
