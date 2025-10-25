using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class Player : Area2D
{
    [Export] int speed = 350;

    [Signal] public delegate void PickupEventHandler();
    [Signal] public delegate void HurtEventHandler();

    Vector2 velocity = Vector2.Zero;
    public Vector2 screensize = new(480, 720);
    AnimatedSprite2D animatedSprite;

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _Process(double delta)
    {
        velocity = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        this.Position += velocity * speed * (float)delta;
        this.Position = new Vector2(Math.Clamp(Position.X, 0, screensize.X),
                                    Math.Clamp(Position.Y, 0, screensize.Y));


        if (velocity.Length() > 0)
            animatedSprite.Animation = "run";
        else
            animatedSprite.Animation = "idle";

        if (velocity.X != 0) 
            animatedSprite.FlipH = velocity.X < 0;


    }

    public void Start()
    {
        SetProcess(true);
        Position = screensize / 2;
        animatedSprite.Animation = "idle";
    }

    public void Die()
    {
        animatedSprite.Animation = "hurt";
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
