using Godot;
using System;

public partial class FireWheel : Node2D
{
    [Export] public float Amplitude = 180f;
    [Export] public float Speed = 3f;
    [Export] public float RotationSpeed = 5f;

    // 👇 escolha no inspector
    [Export] public bool StartAtTop = true;

    private float _time = 0f;
    private float _startY;

    public override void _Ready()
    {
        _startY = Position.Y;

        // define o ponto inicial da onda
        _time = StartAtTop ? Mathf.Pi / 2f : -Mathf.Pi / 2f;
    }

    public override void _Process(double delta)
    {
        float d = (float)delta;

        _time += d * Speed;

        float newY = _startY + Mathf.Sin(_time) * Amplitude;
        Position = new Vector2(Position.X, newY);

        Rotation += RotationSpeed * d;
    }
}