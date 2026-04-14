using Godot;
using System;
public partial class Bullet : Area2D
{
	// Velocidade do tiro (você pode mudar no Inspetor depois)
	[Export] public float Speed = 300.0f;
	
	// A direção padrão (Vector2.Left faz o tiro ir para a esquerda)
	public Vector2 Direction = Vector2.Zero;

	public override void _Ready()
	{
		// Liga o sensor de colisão
		BodyEntered += OnBodyEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		// Faz a bala andar sozinha o tempo todo na direção e velocidade certas
		Position += Direction * Speed * (float)delta;
		// Se a sua imagem original aponta para a ESQUERDA:
    	//Pega o ângulo e somamos PI (180 graus em radianos)
		// Se a imagem estivesse para cima, seria + Mathf.Pi/2
		Rotation = Direction.Angle() + Mathf.Pi;
	}

	private void OnBodyEntered(Node2D body)
	{
		// Se a bala bater no próprio inimigo que atirou, ignora e não faz nada
		if (body.IsInGroup("enemies")) return;

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
