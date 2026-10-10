using Godot;
using System;

public partial class base_prey : CharacterBody2D
{
    private enum States
    {
        Active,
        Vulnerable
    }

    [Signal] public delegate void HitEventHandler();

    [Export] private Vector2 _dir = Vector2.Right;
    [Export] private int _pointVal = 100;
    [Export] private PackedScene _pointsScene;

    private States _enemyState = States.Active;
    private bool _canEat = false;
    private int _speed = 50;
    private int _velocityCap = 150;
    private float _gravityFactor = 0.1f;

    private Sprite2D _sprite2D;
    private Area2D _area2D;
    private RayCast2D _leftCast;
    private RayCast2D _rightCast;

    public override void _Ready()
    {
        _sprite2D = GetNode<Sprite2D>("Sprite2D");
        _area2D = GetNode<Area2D>("Area2D");
        _leftCast = GetNode<RayCast2D>("leftCast");
        _rightCast = GetNode<RayCast2D>("rightCast");

        // Ensure safe signal connection without editor/code conflicts
        _area2D.AreaEntered += _OnArea2DAreaEntered;

        _area2D.BodyEntered += _OnArea2DBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Direction updates based on raycasts
        if (_leftCast.IsColliding())
        {
            _dir = Vector2.Right;
        }
        if (_rightCast.IsColliding())
        {
            _dir = Vector2.Left;
        }

        Vector2 velocity = Velocity;

        if (_enemyState == States.Vulnerable)
        {
            velocity += GetGravity() * (float)delta * _gravityFactor;
            _sprite2D.FlipV = true;
            velocity.X = 0; // Freeze horizontal movement while vulnerable
        }
        else if (_enemyState == States.Active)
        {
            _sprite2D.FlipV = false;
            velocity.X = _dir.X * _speed;
        }

        velocity.Y = Mathf.Clamp(velocity.Y, -_velocityCap, _velocityCap);
        Velocity = velocity;

        MoveAndSlide();
    }

    private async void TriggerVulnerableState()
    {
        // Guard: Avoid restarting if already vulnerable
        if (_enemyState == States.Vulnerable) return;

        _enemyState = States.Vulnerable;
        _canEat = false;
        EmitSignal(SignalName.Hit);

        // Wait 0.2s before prey can be eaten
        await ToSignal(GetTree().CreateTimer(0.2f), SceneTreeTimer.SignalName.Timeout);
        if (!IsInstanceValid(this) || _enemyState != States.Vulnerable) return;

        _canEat = true;

        // 7 seconds vulnerable window
        await ToSignal(GetTree().CreateTimer(7.0f), SceneTreeTimer.SignalName.Timeout);
        if (!IsInstanceValid(this) || _enemyState != States.Vulnerable) return;

        // Reset back to active state if surviving
        _enemyState = States.Active;
        _canEat = false;
    }

    private void _OnArea2DAreaEntered(Area2D area)
    {
        GD.Print($"[{Name}] AreaEntered by '{area.Name}' | Groups: {string.Join(", ", area.GetGroups())}");
        if (area.IsInGroup("player_hitbox"))
        {
            TriggerVulnerableState();
        }
    }

    private void _OnArea2DBodyEntered(Node2D body)
    {
        GD.Print($"[{Name}] BodyEntered by '{body.Name}' | State: {_enemyState} | CanEat: {_canEat}");
        if (body.IsInGroup("player") && _enemyState == States.Vulnerable && _canEat)
        {
            Eaten();
        }
    }

    private void Eaten()
    {
        _enemyState = States.Active;
        _canEat = false;

        if (_pointsScene != null)
        {
            label pointsInstance = (label)_pointsScene.Instantiate();

            pointsInstance.PointVal = _pointVal;
            GetTree().CurrentScene.AddChild(pointsInstance);

            if (pointsInstance is label label2)
            {
                label2.GlobalPosition = GlobalPosition;
            }
            else if (pointsInstance is Control control)
            {
                control.GlobalPosition = GlobalPosition;
            }

        }

        // 5. Remove the enemy cleanly on the next frame
        CallDeferred(Node.MethodName.QueueFree);
    }
}