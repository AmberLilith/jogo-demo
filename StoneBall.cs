using Godot;
using System;

public partial class StoneBall : Area2D
{
    [Export] public float Amplitude = 180f;
    [Export] public float Speed = 3f;
    [Export] public float RotationSpeed = 5f;

    // 👇 escolha no inspector
    [Export] public bool startAtRight = true;

    private float _time = 0f;
    private float _startX;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered; //<-- Necessário em Area2D para a colisão acontencer
        _startX = Position.X;

        // define o ponto inicial da onda
        _time = startAtRight ? Mathf.Pi / 2f : -Mathf.Pi / 2f;
    }

    public override void _Process(double delta)
{
    float d = (float)delta;
    _time += d * Speed;

    float newX = _startX + Mathf.Sin(_time) * Amplitude;
    
    // ✅ Verifica se está indo para direita ou esquerda
    float direcao = newX > Position.X ? 1f : -1f;

    Position = new Vector2(newX, Position.Y);

    // ✅ Gira no sentido da direção
    // horário = positivo, anti-horário = negativo
    Rotation += RotationSpeed * direcao * d;
    
}

private void OnBodyEntered(Node2D body)
{
    if (body is Player player)
    {
        player.Die();
    }
}
}
