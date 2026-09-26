using Godot;
using System;

public partial class creature_player : CharacterBody2D {
	[Export] private Area2D _hitbox;
	[Export] private Marker2D _up;
	[Export] private Marker2D _down;
	[Export] private Marker2D _left;
	[Export] private Marker2D _right;
	const float JumpVelocity = -100.0f;
	private const float Speed = 100.0f;
	private float _accel = 10.0f;
	private bool _cooldown = true;
	public override void _Ready()
	{
		_hitbox.Visible = false;
	}

	public override async void _PhysicsProcess(double delta)
	{
		Vector2 hitDirection = Input.GetVector("a", "d", "w", "s");
		Vector2 velocity = Velocity;
		if (hitDirection.Equals(new Vector2(0, -1)))
		{
			_hitbox.Position = _up.Position;
		} else if (hitDirection.Equals(new Vector2(0, 1)))
		{
			_hitbox.Position = _down.Position;
		} else if (hitDirection.Equals(new Vector2(1, 0)))
		{
			_hitbox.Position = _right.Position;
		} else if (hitDirection.Equals(new Vector2(-1, 0)))
		{
			_hitbox.Position = _left.Position;
		}

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta * 0.1f;
		}
		
		if (Input.IsActionJustPressed("jump"))
		{
			velocity.Y = JumpVelocity;
		}
		if (Input.IsActionJustReleased("shift") && _cooldown)
		{
			_cooldown = false;
			_hitbox.Visible = true;
			_hitbox.AddToGroup("player_hitbox");
			await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
			_hitbox.RemoveFromGroup("player_hitbox");
			_hitbox.Visible = false;
			_cooldown = true;
		}

		float direction = Input.GetAxis("a", "d");
		velocity.X = Mathf.MoveToward(velocity.X, Speed * direction, _accel);
		Velocity = velocity;
		MoveAndSlide();
	}
}
