using Godot;
using System;

public partial class enemy : Area2D
{
	[Export] private creature_player _player;

	[Export] private float _speed = 250;

	private bool _active;
	private bool _movingDown = true;

	private bool _caught = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CallDeferred(nameof(CatchPlayer));
		AreaEntered += OnAreaEntered;
	}

	public void CatchPlayer()
	{
		Vector2 playerPosition = _player.GlobalPosition;
		GlobalPosition = new Vector2(playerPosition.X, -100);
		_active = true;
		_movingDown = true;
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (!_active) return;
		Vector2 newPos = GlobalPosition;
		if (_movingDown && GlobalPosition.Y < 550)
		{
			newPos.Y += (float)delta * _speed;
			if (_caught) _player.Position = newPos;
		} else if (_movingDown && GlobalPosition.Y > 550)
		{
			_movingDown = false;
		} else if (!_movingDown && GlobalPosition.Y > -100)
		{
			newPos.Y -= (float)delta *  _speed;
			if (_caught) _player.Position = newPos;
		} else if (!_movingDown && GlobalPosition.Y <= -100)
		{
			_active = false;
			if (_caught)
			{
				_caught = false;
				_player.Die();
			}
		}
		GlobalPosition = newPos;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area.IsInGroup("player_hurtbox"))
		{
			_caught = true;
		}
	}
}
