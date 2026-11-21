using Godot;
using System;

public partial class opabinia : Area2D
{
	[Export] private creature_player _player;
	[Export] private float _speed = 10f;
	private Vector2 _dir = Vector2.Right;
	private bool _active;
	private Timer _timer;
	private Vector2 _velocity = Vector2.Zero;
	private Random _random = new Random();

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		_timer = new Timer
		{
			WaitTime = 20, // Seconds between calls
			OneShot = false, // Repeat automatically
			Autostart = true // Start immediately when added to tree
		};

		_timer.Timeout += StartRoam;
		AddChild(_timer);
		StartRoam();
		
	}

	public void StartRoam()
	{
	    _dir = Vector2.Right;
		GlobalPosition = new Vector2(40, _random.NextSingle() * 200);
		_active = true;
		_velocity = _dir * _speed;

	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_active) return;
		GlobalPosition += _velocity;
		if (GlobalPosition.X < -60)
		{
			_dir = Vector2.Right;
			_velocity = _dir * _speed;
			GlobalPosition += new Vector2(0, 200);
		}

		if (GlobalPosition.X > 1200)
		{
			_dir = Vector2.Left;
			_velocity = _dir * _speed;
			GlobalPosition += new Vector2(0, 200);
		}
		
		if (GlobalPosition.Y > 700)
		{
			_velocity = Vector2.Zero;
			_active = false;
			GD.Print("Inactive");
		}
	}
	
	
	private void OnAreaEntered(Area2D area)
	{
		if (area.IsInGroup("player_hurtbox"))
		{
			_player.Die();
		}
		
	}
}
