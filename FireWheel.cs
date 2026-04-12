using Godot;
using System;

public partial class FireWheel : Area2D
{
    [Export] public float Amplitude = 200f;
    [Export] public float Speed = 5f;

    [Export] public float RotationSpeed = 5f; // velocidade de rotação

    private float _time = 0f;
    private float _startY;
    [Export] public float Damage = 10f;

    public override void _Ready()
    {
        _startY = Position.Y;
        BodyEntered += OnBodyEntered;
    }

    public override void _Process(double delta)
    {
        float d = (float)delta;

        // movimento vertical (seno)
        _time += d * Speed;
        float newY = _startY + Mathf.Sin(_time) * Amplitude;

        Position = new Vector2(Position.X, newY);

        // rotação contínua
        Rotation += RotationSpeed * d;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.Die();
        }
    }
}