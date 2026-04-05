using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float RunSpeed = 400.0f;
	public const float JumpVelocity = -900.0f;
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private GameManager _gameManager;
	private CanvasLayer _gameOverScreen;


	private AnimatedSprite2D _animation;
	private bool _isDead = false;

	public int Points = 0;
	private Label _ScoreText;
	private Label _LifesText;

	public override void _Ready()
	{
		_gameOverScreen = GetParent().GetNode<CanvasLayer>("GameOverScreen");
		_animation = GetNode<AnimatedSprite2D>("PlayerAnimation");
		_ScoreText = GetNode<Label>("../HUD//HBoxContainer/Score");
		_LifesText = GetNode<Label>("../HUD/HBoxContainer/Lifes");
		_gameManager = GetNode<GameManager>("/root/GameManager");
		_gameOverScreen = GetParent().GetNode<CanvasLayer>("GameOverScreen");
		UpdateScore();
		UpdateLifes();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		bool isRunning = Input.IsActionPressed("run");

		// Define qual velocidade usar
		float currentSpeed = isRunning ? RunSpeed : Speed;

		if (_isDead)
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
			velocity.X = direction.X * currentSpeed;
			_animation.FlipH = direction.X < 0;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, currentSpeed);
		}

		Velocity = velocity;
		MoveAndSlide();


		if (!IsOnFloor())
			_animation.Play("jump");
		else if (direction != Vector2.Zero)
			if (isRunning)
			{
				_animation.SpeedScale = 2.0f;
				_animation.Play("run");
			}
			else
			{
				_animation.SpeedScale = 1.0f;
				_animation.Play("walk");
			}
		else
		{
			_animation.SpeedScale = 0.2f;
			_animation.Play("idle");
		}


		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collison = GetSlideCollision(i);
			var tockedObject = collison.GetCollider();
			if (tockedObject is Node nodo && nodo.IsInGroup("enemies"))//Garantir que todo inimigo tenha o grupo "inimigos" no Inspetor
			{
				Die();
				break;
			}
		}
	}

	public async void Die()
	{
		if (_isDead) return;
		_isDead = true;
		_gameManager.CurrentLives--;
		_animation.SpeedScale = 0.5f;
		_animation.Play("die");
	GetNode<CollisionShape2D>("PlayerCollision").SetDeferred("disabled", true);
	Velocity = new Vector2(0, -400);
	// 2. Espera o tempo da queda dramática
	await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);

	// 3. DECISÃO: Reaparecer ou Game Over?
	if (_gameManager.CurrentLives > 0)
	{
		Respawn();
	}
	else
	{
		ShowGameOver();
	}
	}

	private void Respawn()
{
	_isDead = false;
	// Reativa a colisão
	GetNode<CollisionShape2D>("PlayerCollision").SetDeferred("disabled", false);
	
	// Volta para o início da fase ou um checkpoint
	// Para simplificar agora, vamos apenas recarregar a cena, 
	// mas precisamos salvar as vidas atuais!
	GetTree().ReloadCurrentScene(); 
	
	// NOTA: Se você der ReloadCurrentScene, as vidas voltam para 3. 
	// Para manter as vidas entre cenas, precisaríamos de um script "Global". 
	// Quer que eu te ensine a fazer esse Global (Autoload) agora?
}

private void ShowGameOver()
{
	GetTree().Paused = true;
	if (_gameOverScreen != null)
	{
		_gameOverScreen.Visible = true;
	}
}

	public void AddPoints(int value)
	{
		Points += value;
		UpdateScore();
	}

	public void AddLife(int value)
	{
		_gameManager.CurrentLives += value;
		UpdateLifes();
	}

	private void UpdateScore()
	{
		if (_ScoreText != null)
		{
			_ScoreText.Text = "Bebezinhas coletadas: " + Points;
		}
	}

	private void UpdateLifes()
	{
		if (_LifesText != null)
	{
		GD.Print("Atualizando label para: " + _gameManager.CurrentLives);
		_LifesText.Text = "Vidas: " + _gameManager.CurrentLives;
	}
	else {
		GD.Print("Erro: _LifesText é nulo!");
	}
	}		
}
