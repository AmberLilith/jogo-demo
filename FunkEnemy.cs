using Godot;

using System;



public partial class FunkEnemy : CharacterBody2D

{

	// 1. Crie esta variável no topo (junto com as outras, se tiver).

	// O [Export] vai fazer aparecer um espaço no Inspetor para você colocar a bala!

	[Export] public PackedScene Bullet;

	private Marker2D _gunBarrel;



	[Export] public float AtackDistance = 400.0f;



	[Export] public float FireDistance = 200.0f;



	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();



	private AnimatedSprite2D _animacao;

	private Node2D _Player;



	private Timer _timerAtirar;





	public override void _Ready()

	{

		// Pega a referência correta

		_animacao = GetNode<AnimatedSprite2D>("FunkEnemyAnimation");



		_timerAtirar = GetNode<Timer>("Timer");

		_timerAtirar.Timeout += Atirar;



		// Garante que o inimigo sempre comece relaxado (idle)



		_animacao.Play("idle");



		// Busca o Player na cena pelo nome do grupo

		_Player = GetTree().GetFirstNodeInGroup("Player") as Node2D;



		// 2. Avisa o código onde está o Marker2D

		_gunBarrel = GetNode<Marker2D>("Marker2D");

	}



	private void Atirar()

	{

		// Verifica se você lembrou de colocar a bala lá no Inspetor

		if (Bullet != null)

		{

			// Cria um clone (uma instância) da bala

			Node2D newBullet = (Node2D)Bullet.Instantiate();



			// Coloca a bala exatamente na posição do Marker2D

			newBullet.GlobalPosition = _gunBarrel.GlobalPosition;



			// Adiciona a bala na tela do jogo (GetParent() joga a bala no nó "Mundo",

			// assim ela não fica presa ao corpo do inimigo)

			GetParent().AddChild(newBullet);

		}

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



			if (distancia < FireDistance)

			{

				_animacao.SpeedScale = 1.0f;

				_animacao.Play("fire");



				// Se o cronômetro estiver parado, o Player acabou de entrar na área!

				if (_timerAtirar.IsStopped())

				{

					Atirar(); // Dá o primeiro tiro na hora, sem esperar!

					_timerAtirar.Start(); // Liga o cronômetro (para atirar de novo a cada X segundos)

				}

			}

			// 2º TESTE: O Player está longe, mas dá pra ver? (Menor que 400)

			else if (distancia < AtackDistance)

			{

				_timerAtirar.Stop(); // O Player se afastou, MANDA PARAR DE ATIRAR!



				if (_animacao.Animation != "draw")

				{

					_animacao.SpeedScale = 1.0f;

					_animacao.Play("draw");

				}

			}

			// 3º TESTE: Tá longe demais. Fica de boa.

			else

			{

				_timerAtirar.Stop(); // MANDA PARAR DE ATIRAR!

				_animacao.SpeedScale = 0.2f;

				_animacao.Play("idle");

			}



			// Vira pra olhar pro Player USANDO A VARIÁVEL CORRETA

			_animacao.FlipH = _Player.GlobalPosition.X > this.GlobalPosition.X;

		}

	}

}
