using Godot;

public partial class Inimigo : CharacterBody2D
{
	[Export] public float Velocidade = 80.0f;
	[Export] public float DistanciaPatrulha = 150.0f; // pixels que ele anda pra cada lado

	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimatedSprite2D _animacao;
	private float _posicaoInicial;
	private int _direcao = 1; // 1 = direita, -1 = esquerda

	public override void _Ready()
	{
		_animacao = GetNode<AnimatedSprite2D>("Sprite2D"); // ajuste o nome do nó se necessário
		_posicaoInicial = GlobalPosition.X;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 vel = Velocity;

		// Gravidade
		if (!IsOnFloor())
			vel.Y += Gravity * (float)delta;

		// Movimento de patrulha
		vel.X = _direcao * Velocidade;

		// Inverte ao atingir o limite da patrulha
		float distancia = GlobalPosition.X - _posicaoInicial;
		if (distancia > DistanciaPatrulha || distancia < -DistanciaPatrulha)
		{
			_direcao *= -1;
		}

		// Vira o sprite conforme a direção
		_animacao.FlipH = _direcao < 0;

		Velocity = vel;
		MoveAndSlide();

		// Toca animação de andar
		_animacao.Play("andar"); // use o nome da animação que você criou
	}
}
