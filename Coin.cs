using Godot;
using System;

public partial class Coin : Area2D
{
    public Vector2 Screensize = Vector2.Zero;

    public void Pickup()
    {
        QueueFree();
    }
}
