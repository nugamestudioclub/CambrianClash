using Godot;
using System;

public partial class creature_player : CharacterBody2D 
{
	[Export] private Area2D _hitbox;
	[Export] private Area2D _hurtbox;
	[Export] private Marker2D _up;
	[Export] private Marker2D _down;
	[Export] private Marker2D _left;
	[Export] private Marker2D _right;
	[Export] private Sprite2D _sprite;
	[Export] private AudioStreamPlayer _sound;
	[Export] private NodePath _attackSFX;
	[Export] private float _invincibilityTime;

	private float _gravityFactor = 0.1f;
	private float _velocityCap = 170f;
	private float _jumpVelocity = -5f;
	private float _speed = 190f;
	private float _accel = 5f;
	private float _kbMultiplier = 0.8f;
	private bool _cooldown = true;
	private bool _dead = false;
	private float _invincibilityTimer = 0;

	
	public override void _Ready()
	{
		
		_hitbox.Visible = false;
	
		// Completely turn off collision detection both ways when not attacking
		_hitbox.SetDeferred(Area2D.PropertyName.Monitoring, false);
		_hitbox.SetDeferred(Area2D.PropertyName.Monitorable, false);

		if (!_hitbox.IsInGroup("player_hitbox"))
		{
			_hitbox.AddToGroup("player_hitbox");
		}
		
		_hurtbox = GetNode<Area2D>("hurtbox");
		_hurtbox.AreaEntered += _OnArea2DAreaEntered;

		_hurtbox.BodyEntered += _OnArea2DBodyEntered;
		
		score.Instance.SpawnPlayer += Spawn;
	}

	private async void PerformAttack()
	{
		if (_dead) return;

		_cooldown = false;
		_hitbox.Visible = true;
		
		// Play sound effect
		_sound.Play();
	
		// Enable collision detection in both directions during the swing
		_hitbox.SetDeferred(Area2D.PropertyName.Monitoring, true);
		_hitbox.SetDeferred(Area2D.PropertyName.Monitorable, true);

		await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);

		// Disable collision detection immediately after swing ends
		_hitbox.SetDeferred(Area2D.PropertyName.Monitoring, false);
		_hitbox.SetDeferred(Area2D.PropertyName.Monitorable, false);
		_hitbox.Visible = false;
		_cooldown = true;
	}
	
	private void Knockback()
	{
		if (_dead) return;

		Vector2 velocity = Velocity;
		Vector2 knockbackDirection = -velocity * _kbMultiplier;
		velocity.X = knockbackDirection.X;
		Velocity = velocity;
		MoveAndSlide();
		GD.Print("Bump!");
		GD.Print($"[{Name}] knockback: '{knockbackDirection}'");
	}
	
	public void Die()
	{
		GD.Print(_invincibilityTimer > 0, "Invincibility");
		if (_invincibilityTimer > 0) return;
		_dead = true;
		Velocity = Vector2.Zero;
		Position = new Vector2(-100, -100);
		score.Instance._LoseLife();
	}

	private void Spawn(Vector2 position)
	{
		GlobalPosition = position;
		Vector2 randomDir = Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau));
		Velocity = randomDir * 100f;
		_dead = false;
		_invincibilityTimer = _invincibilityTime;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (_dead) return;
		_invincibilityTimer -= (float)delta;
		if (_invincibilityTimer <= 0)
			_invincibilityTimer = 0.0f;
		Vector2 velocity = Velocity;
		Vector2 hitDirection = Input.GetVector("a", "d", "w", "s");

		if (hitDirection == new Vector2(0, -1))
		{
			_hitbox.Position = _up.Position;
			_sprite.Rotation = (float)Math.PI / 2;
			_sprite.FlipH = true;
			_sprite.FlipV = true;
		} 
		else if (hitDirection == new Vector2(0, 1))
		{
			_hitbox.Position = _down.Position;
			_sprite.Rotation = (float)Math.PI / 2;
			_sprite.FlipH = false;
			_sprite.FlipV = true;
		} 
		else if (hitDirection == new Vector2(1, 0))
		{
			_hitbox.Position = _right.Position;
			_sprite.Rotation = (float)0;
			_sprite.FlipH = false;
			_sprite.FlipV = false;
		} 
		else if (hitDirection == new Vector2(-1, 0))
		{
			_hitbox.Position = _left.Position;
			_sprite.Rotation = (float)0;
			_sprite.FlipH = true;
			_sprite.FlipV = false;
		}

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta * _gravityFactor;
			_speed = 190f;
			_accel = 0.5f;
		}
		else
		{
			_speed = 150f;
			_accel = 10f;
		}

		if (Input.IsActionPressed("jump"))
		{
			velocity.Y += _jumpVelocity;
			_accel = 10f;
		}

		if (Input.IsActionJustReleased("jump"))
		{
			_accel = 0.5f;
		}

		if (Input.IsActionPressed("s"))
		{
			_gravityFactor = 0.2f;
			_velocityCap = 300f;
		} 
		else
		{
			_gravityFactor = 0.1f;
			_velocityCap = 170f;
		}

		// Trigger attack off-thread from physics processing
		if (Input.IsActionJustPressed("shift") && _cooldown)
		{
			PerformAttack();
		}

		float direction = Input.GetAxis("a", "d");
		velocity.X = Mathf.MoveToward(velocity.X, _speed * direction, _accel);

		velocity.Y = Mathf.Clamp(velocity.Y, -_velocityCap, _velocityCap);
		Velocity = velocity;
		MoveAndSlide();
	}
	
	private void _OnArea2DAreaEntered(Area2D area)
	{
		if (_dead) return;
		if (area.IsInGroup("wall"))
		{
			Knockback();
		}
		if (area.IsInGroup("predator"))
		{
			Die();
		}
	}

	private void _OnArea2DBodyEntered(Node2D body)
	{
		if (_dead) return;
		if (body.IsInGroup("wall"))
		{
			GD.Print($"[{Name}] AreaEntered by '{body.Name}' | Groups: {string.Join(", ", body.GetGroups())}");
			Knockback();
		}
	}
}
