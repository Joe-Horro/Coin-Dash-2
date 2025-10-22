using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class Player : Area2D
{
    [Export] int speed = 350;

    [Signal] public delegate void PickupEventHandler();
    [Signal] public delegate void HurtEventHandler();

    Vector2 velocity = Vector2.Zero;
    Vector2 screensize = new Vector2(480, 720);
    AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _Process(double delta)
    {
        velocity = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        this.Position += velocity * speed * (float)delta;

        if (velocity.Length() > 0)
            _animatedSprite.Animation = "run";
        else
            _animatedSprite.Animation = "idle";

        if (velocity.X != 0) 
            _animatedSprite.FlipH = velocity.X < 0;


    }

    public void Start()
    {
        SetProcess(true);
        Position = screensize / 2;
        _animatedSprite.Animation = "idle";
    }

    public void Die()
    {
        _animatedSprite.Animation = "hurt";
        SetProcess(false);
    }

    public void OnAreaEntered(Area2D area2D)
    {
        if(area2D.IsInGroup("coins"))
        {
            if (area2D.GetType() != typeof(Coin))
            {
                GD.PrintErr("Non coin type object in coin group.");
                throw new Exception("Non coin type object in coin group.");
            }

            ((Coin)area2D).Pickup();
            EmitSignal(SignalName.Pickup, "coin");
        }

        if(area2D.IsInGroup("obstacles"))
        {
            EmitSignalHurt();
            Die();
        }
    }

}
