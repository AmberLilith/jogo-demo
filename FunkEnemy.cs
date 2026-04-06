using Godot;

using System;



public partial class FunkEnemy : CharacterBody2D

{

	// 1. Crie esta variável no topo (junto com as outras, se tiver).

	// O [Export] vai fazer aparecer um espaço no Inspetor para você colocar a bala!

	[Export] public PackedScene Bullet;

	private Marker2D _gunBarrel;



	[Export] public float AtackDistance = 500.0f;



	[Export] public float FireDistance = 400.0f;



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
    if (Bullet == null || _Player == null) return;

    // 1. Instancia como Node2D primeiro para não dar erro de conversão
    var instance = Bullet.Instantiate();
    GD.Print("Instanciei um nó do tipo: " + instance.GetType().Name);
    // 2. Tenta encontrar o script Bullet dentro dessa instância
    if (instance is Bullet newBullet) 
    {
        newBullet.GlobalPosition = _gunBarrel.GlobalPosition;

		//Em vez de apontar para a GlobalPosition (pés), 
        // atinge o meio do corpo (subtraindo uns 30 pixels no Y)
        Vector2 alvo = _Player.GlobalPosition + new Vector2(0, -30);

        // Calcula a direção
        Vector2 direcao = (_Player.GlobalPosition - _gunBarrel.GlobalPosition).Normalized();
        newBullet.Direction = direcao;

		GD.Print($"Bala criada em: {newBullet.GlobalPosition} | Direção: {newBullet.Direction}");

        GetTree().Root.AddChild(newBullet);
    }
    else 
    {
        GD.PrintErr("ERRO: A cena Bullet.tscn não tem o script Bullet.cs anexado!");
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
