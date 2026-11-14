using Godot;
using System;

public partial class PlayerEgg : CharacterBody2D
{
	private float _gravityFactor = 0.01f;
	private int _velocityCap = 150;
	private bool _spawned = false;
	private int _presses = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_StartSpawnPlayer();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		velocity += GetGravity() * (float)delta * _gravityFactor;
		
		velocity.Y = Mathf.Clamp(velocity.Y, -_velocityCap, _velocityCap);
		Velocity = velocity;
		MoveAndSlide();
		if (Input.IsActionJustPressed("jump"))
		{
			_presses++;
		}
		else if (Input.IsActionJustPressed("a"))
		{
			_presses++;
		}
		else if (Input.IsActionJustPressed("d"))
		{
			_presses++;
		}

		if (_presses == 10)
		{
			_SpawnPlayer();
		}
	}

	private void _SpawnPlayer()
	{
		if (_spawned) return;
		_spawned = true;
		score.Instance._SpawnPlayer(GlobalPosition);
	}
	
	private async void _StartSpawnPlayer()
	{
		Godot.GD.Print("Spawn Player");
		await ToSignal(GetTree().CreateTimer(8f), SceneTreeTimer.SignalName.Timeout);
		_SpawnPlayer();
	}
}
