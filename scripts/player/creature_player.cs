using Godot;
using System;

public partial class creature_player : CharacterBody2D 
{
    [Export] private Area2D _hitbox;
    [Export] private Marker2D _up;
    [Export] private Marker2D _down;
    [Export] private Marker2D _left;
    [Export] private Marker2D _right;
    [Export] private Sprite2D _sprite;

    private float _gravityFactor = 0.1f;
    private float _velocityCap = 170f;
    private float _jumpVelocity = -5f;
    private float _speed = 190f;
    private float _accel = 5f;
    private bool _cooldown = true;

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
    }

    private async void PerformAttack()
    {
        _cooldown = false;
        _hitbox.Visible = true;
    
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

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        Vector2 hitDirection = Input.GetVector("a", "d", "w", "s");

        if (hitDirection == new Vector2(0, -1))
        {
            _hitbox.Position = _up.Position;
            _sprite.Rotation = 0;
        } 
        else if (hitDirection == new Vector2(0, 1))
        {
            _hitbox.Position = _down.Position;
            _sprite.Rotation = (float)Math.PI;
        } 
        else if (hitDirection == new Vector2(1, 0))
        {
            _hitbox.Position = _right.Position;
            _sprite.Rotation = (float)Math.PI / 2;
        } 
        else if (hitDirection == new Vector2(-1, 0))
        {
            _hitbox.Position = _left.Position;
            _sprite.Rotation = (float)-Math.PI / 2;
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
}