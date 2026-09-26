using Godot;
using System;

public partial class area_2d : Area2D
{
    private enum States {ACTIVE, VUNERBLE}

    [Export] private States _enemyState; 
    [Export] private int speed = 50;
    [Export] private bool stop = false;
    [Export] private Sprite2D _sprite2d;

    [Export] private PackedScene PointsScene { get; set; }

    private float originalY;
    
    [Signal] public delegate void HitEventHandler();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
        BodyEntered += OnBodyEntered;
        originalY = Position.Y;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (GetOverlappingAreas().Count != 0 && GetOverlappingAreas()[0].IsInGroup("player_hitbox"))
        {
            _enemyState = States.VUNERBLE;
            EmitSignal(SignalName.Hit);
        }

        if (_enemyState == States.VUNERBLE)
        {
            if (stop == false)
            {
                Position += (Vector2.Down * speed) * (float)delta;
            }

            _sprite2d.FlipV = true;
        }
        else if (_enemyState == States.ACTIVE)
        {
            _sprite2d.FlipV = false;
            if (Position.Y > originalY)
            {
                Position += (Vector2.Up * speed) * (float)delta;
            }
        }
    }

    public void OnAreaEntered(Area2D area)
    {
        if (area.IsInGroup("player_hitbox"))
        {
            _enemyState = States.VUNERBLE;
        }
    }

    public async void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("floor"))
        {
            stop = true;
            await ToSignal(GetTree().CreateTimer(7.0f), SceneTreeTimer.SignalName.Timeout);
            if (IsInstanceValid(this))
            {
                _enemyState = States.ACTIVE;
                stop = false;
            }
        }

        if ((body.IsInGroup("player") || body.GetParent().IsInGroup("player")) && _enemyState == States.VUNERBLE)        {
            Label _points = PointsScene.Instantiate<Label>();
            _points.Position = Position; 
            AddSibling(_points);
            QueueFree();
        }
    }
}