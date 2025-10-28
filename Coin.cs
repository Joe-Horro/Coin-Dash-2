using Godot;
using System;
using System.Threading.Tasks;

public partial class Coin : Area2D
{
    public Vector2 Screensize = Vector2.Zero;
    CollisionShape2D collisionShape;

    public override void _Ready()
    {
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public async Task Pickup()
    {
        collisionShape.SetDeferred("disabled", true);
        var tw = CreateTween().SetParallel().SetTrans(Tween.TransitionType.Quad);
        tw.TweenProperty(this, "scale", this.Scale * 3, 0.3);
        tw.TweenProperty(this, "modulate:a", 0.0, 0.3);
        await ToSignal(tw, Tween.SignalName.Finished);
        QueueFree();
    }
}
