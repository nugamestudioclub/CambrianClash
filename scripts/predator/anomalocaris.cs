using Godot;

public partial class anomalocaris : Area2D
{
	[Export] private creature_player _player;

	[Export] private float _speed = 250;

	private bool _active;
	private bool _movingDown = true;

	private bool _caught;
	private Timer _timer;
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

		_timer.Timeout += CatchPlayer;
		AddChild(_timer);
		CatchPlayer();
	}

	private void CatchPlayer()
	{
		Vector2 playerPosition = _player.GlobalPosition;
		GlobalPosition = new Vector2(playerPosition.X - GetNode<Sprite2D>("Sprite2D").GetRect().Size.X / 2, -100);
		_active = true;
		_movingDown = true;
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (!_active) return;
		Vector2 newPos = GlobalPosition;
		Sprite2D child = GetNode<Sprite2D>("Sprite2D");
		if (_movingDown && GlobalPosition.Y < 550)
		{
			newPos.Y += (float)delta * _speed;
			if (_caught)
			{
				Vector2 playerPosition = new Vector2(_player.GlobalPosition.X, newPos.Y + child.GetRect().Size.Y);
				_player.GlobalPosition = playerPosition;
				_player.Caught = true;
			}
		} else if (_movingDown && GlobalPosition.Y > 550)
		{
			_movingDown = false;
		} else if (!_movingDown && GlobalPosition.Y > -100)
		{
			newPos.Y -= (float)delta *  _speed;
			if (_caught)
			{
				Vector2 playerPosition = new Vector2(_player.GlobalPosition.X, newPos.Y + child.GetRect().Size.Y);
				_player.GlobalPosition = playerPosition;
			}
			
			
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
