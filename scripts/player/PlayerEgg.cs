using Godot;
using System;

public partial class PlayerEgg : CharacterBody2D
{
	private float _gravityFactor = 0.01f;
	private int _velocityCap = 150;
	public bool canSpawn = false;
	
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
		_SpawnPlayer();
	}

	private async void _SpawnPlayer()
	{
		if (!canSpawn) return;
		canSpawn = false;
		Godot.GD.Print("Spawn Player");
		await ToSignal(GetTree().CreateTimer(10f), SceneTreeTimer.SignalName.Timeout);
		score.Instance._SpawnPlayer(GlobalPosition);
	}
}
