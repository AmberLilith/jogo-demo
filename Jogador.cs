using Godot;
using System;

public partial class Jogador : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float RunSpeed = 400.0f;
	public const float JumpVelocity = -900.0f;
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimatedSprite2D _animacao;
	private bool _estaMorto = false;

	public override void _Ready()
	{
		_animacao = GetNode<AnimatedSprite2D>("AnimatedSpriteJogador");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		bool estaCorrendo = Input.IsActionPressed("run");
		
		// Define qual velocidade usar
		float velocidadeAtual = estaCorrendo ? RunSpeed : Speed;
		
		if (_estaMorto)
		{
			Vector2 velMorte = Velocity;
			if (!IsOnFloor())
				velMorte.Y += Gravity * (float)delta;
			velMorte.X = 0;
			Velocity = velMorte;
			MoveAndSlide();
			return;
		}

		Vector2 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y += Gravity * (float)delta;

		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;


		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			_animacao.FlipH = direction.X < 0;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		
		if (!IsOnFloor())
			_animacao.Play("pular");
		else if (direction != Vector2.Zero)
			if (estaCorrendo)
			{
				_animacao.SpeedScale = 2.0f;
				_animacao.Play("run");
			}
			else
			{
				_animacao.SpeedScale = 1.0f;
				_animacao.Play("walk");
			}
		else
		{
			_animacao.SpeedScale = 0.2f;
			_animacao.Play("idle");
		}
			

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var colisao = GetSlideCollision(i);
			var objetoTocado = colisao.GetCollider();
			if (objetoTocado is Node nodo && nodo.IsInGroup("inimigos"))
			{
				Morrer();
				break;
			}
		}
	}

	private async void Morrer()
	{
		if (_estaMorto) return;
		_estaMorto = true;
		_animacao.SpeedScale = 0.5f;
		_animacao.Play("die");
		await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
		GetTree().ReloadCurrentScene();
	}
}
