using Godot;
using System;

public partial class Bullet : Area2D
{
	// Velocidade do tiro (você pode mudar no Inspetor depois)
	[Export] public float Speed = 200.0f;
	
	// A direção padrão (Vector2.Left faz o tiro ir para a esquerda)
	public Vector2 Direction = Vector2.Left;

	public override void _Ready()
	{
		// Liga o sensor de colisão
		BodyEntered += OnBodyEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		// Faz a bala andar sozinha o tempo todo na direção e velocidade certas
		Position += Direction * Speed * (float)delta;
	}

	private void OnBodyEntered(Node2D body)
	{
		// Se a bala bater no próprio inimigo que atirou, ignora e não faz nada
		if (body.IsInGroup("inimigos")) return;

		// Se bater no Player...
		if (body is Player Player)
		{
			// O Player morre!
			Player.Die();
		}

		// Independente de bater no Player, no chão ou na parede de vidro... a bala se destrói!
		QueueFree();
	}
}
