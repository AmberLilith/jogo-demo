using Godot;
using System;

public partial class CrentelhaEnemy : CharacterBody2D
{
	[Export] public float AtackDistance = 400.0f; // distância pra detectar o Player

	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimatedSprite2D _animation ;
	private Node2D _Player;

	public override void _Ready()
	{
		// Pega a referência correta
		_animation = GetNode<AnimatedSprite2D>("CrentelhaEnemyAnimation");
		
		// Garante que o inimigo sempre comece relaxado (idle)
		_animation.Play("idle");

		// Busca o Player na cena pelo nome do grupo
		_Player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 vel = Velocity;

		// Gravidade (pra não flutuar)
		if (!IsOnFloor())
			vel.Y += Gravity * (float)delta;

		Velocity = vel;
		MoveAndSlide();

		// Detecta distância até o Player
		if (_Player != null)
		{
			float distancia = GlobalPosition.DistanceTo(_Player.GlobalPosition);

			if (distancia < AtackDistance)
			{
				_animation.Play("atack"); // levanta a bíblia
			}
			else
			{
				_animation.Play("idle"); // fica de boa
			}

			// Vira pra olhar pro Player USANDO A VARIÁVEL CORRETA
			_animation.FlipH = _Player.GlobalPosition.X > this.GlobalPosition.X;
		}
	}
}
