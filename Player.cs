using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float RunSpeed = 400.0f;
	public const float JumpVelocity = -600.0f;

    private bool _dubleJump = false;
    
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private CanvasLayer _gameOverScreen;


	private AnimatedSprite2D _animation;

	private CollisionShape2D _collisionShape;
	private bool _isDead = false;

	public int Points = 0;
	private Label _ScoreText;
	private Label _LifesText;

	private CollisionShape2D _colisaoEmPe;
	private CollisionShape2D _colisaoAgachado;

	private bool _isInvincible = false;

	public override void _Ready()
	{
		if (GameManager.Instance.LastCheckpointPos == Vector2.Zero)
		{
			GameManager.Instance.LastCheckpointPos = GlobalPosition;
		}
		_gameOverScreen = GetParent().GetNode<CanvasLayer>("GameOverScreen");
		_animation = GetNode<AnimatedSprite2D>("PlayerAnimation");
		_colisaoEmPe = GetNode<CollisionShape2D>("PlayerDefaultCollision");
		_colisaoAgachado = GetNode<CollisionShape2D>("PlayerSquattingCollision");

		_ScoreText = GetNode<Label>("../HUD//HBoxContainer/Score");
		_LifesText = GetNode<Label>("../HUD/HBoxContainer/Lifes");
		_gameOverScreen = GetParent().GetNode<CanvasLayer>("GameOverScreen");
		UpdateScore();
		UpdateLifes();
	}

public override void _PhysicsProcess(double delta)
{
    Vector2 direction = new Vector2(Input.GetAxis("ui_left", "ui_right"), 0);
    bool isRunning = Input.IsActionPressed("run");
    bool isSquatting = Input.IsActionPressed("squat");

    float currentSpeed = isRunning ? RunSpeed : Speed;

	_animation.Offset = Vector2.Zero;

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
    if (Input.IsActionJustPressed("ui_accept"))
{
    if (IsOnFloor())
    {
        // ✅ Primeiro pulo — normal
        velocity.Y = JumpVelocity;
        _dubleJump = true; // libera o segundo pulo
    }
    else if (_dubleJump)
    {
        // ✅ Segundo pulo — dobro da força
        velocity.Y = JumpVelocity * 2;
        _dubleJump = false; // consome o segundo pulo
    }
}



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
    {
        if (!isSquatting)
        {
            _animation.Play("jump");
            _colisaoEmPe.SetDeferred("disabled", false);
            _colisaoAgachado.SetDeferred("disabled", true);
        }
    }
    else if (isSquatting && direction != Vector2.Zero)
    {
        // Andando agachado
        _animation.SpeedScale = 1.0f;
		_animation.Play("walkSquatting");
		_animation.Offset = new Vector2(0, 475); // Ajusta o offset para alinhar a animação agachada, pois não consegui ajustar no editor
		_colisaoEmPe.SetDeferred("disabled", true);
		_colisaoAgachado.SetDeferred("disabled", false);
    }
    else if (isSquatting)
    {
        // Agachado parado
        if (_animation.Animation != "squatting")
        {
            _animation.SpeedScale = 2.0f;
            _animation.Play("squatting");
            _colisaoEmPe.SetDeferred("disabled", true);
            _colisaoAgachado.SetDeferred("disabled", false);
        }
    }
    else if (direction != Vector2.Zero)
    {
        _colisaoEmPe.SetDeferred("disabled", false);
        _colisaoAgachado.SetDeferred("disabled", true);

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
    }
    else
    {
        // Parado no chão
        _animation.SpeedScale = 0.2f;
        _animation.Play("idle");
        _colisaoEmPe.SetDeferred("disabled", false);
        _colisaoAgachado.SetDeferred("disabled", true);
    }

    for (int i = 0; i < GetSlideCollisionCount(); i++)
    {
        var collison = GetSlideCollision(i);
        var tockedObject = collison.GetCollider();
        if (tockedObject is Node nodo && nodo.IsInGroup("enemies"))
        {
            Die();
            break;
        }
    }
}
public async void Die()
{
    if (_isDead || _isInvincible) return; // ✅ Invencível também bloqueia morte
    _isDead = true;
    GameManager.Instance.CurrentLives--;
    UpdateLifes();
    _animation.SpeedScale = 0.5f;
    _animation.Play("die");
    _colisaoEmPe.SetDeferred("disabled", true);
    _colisaoAgachado.SetDeferred("disabled", true);
    Velocity = new Vector2(0, -400);

    await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);

    if (GameManager.Instance.CurrentLives > 0)
        Respawn();
    else
        ShowGameOver();
}

private async void Respawn()
{
    _isDead = false;
    _isInvincible = true; // ✅ Ativa invencibilidade

    _colisaoEmPe.SetDeferred("disabled", false);
    _colisaoAgachado.SetDeferred("disabled", true);
    _animation.SpeedScale = 1.0f;
    Velocity = Vector2.Zero;
    GlobalPosition = GameManager.Instance.LastCheckpointPos;

    // ✅ Pisca o sprite para indicar invencibilidade
    for (int i = 0; i < 6; i++)
    {
        _animation.Modulate = new Color(1, 1, 1, 0.3f); // transparente
        await ToSignal(GetTree().CreateTimer(0.2f), SceneTreeTimer.SignalName.Timeout);
        _animation.Modulate = new Color(1, 1, 1, 1.0f); // visível
        await ToSignal(GetTree().CreateTimer(0.2f), SceneTreeTimer.SignalName.Timeout);
    }

    _isInvincible = false; // ✅ Desativa invencibilidade após piscar
    _animation.Play("idle");
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
		GameManager.Instance.CurrentLives += value;
		UpdateLifes();
	}

	private void UpdateScore()
	{
		if (_ScoreText != null)
		{
			_ScoreText.Text = "Ovos de Bebezinhas coletados: " + Points;
		}
	}

	private void UpdateLifes()
	{
		if (_LifesText != null)
		{
			GD.Print("Atualizando label para: " + GameManager.Instance.CurrentLives);
			_LifesText.Text = "Vidas: " + GameManager.Instance.CurrentLives;
		}
		else
		{
			GD.Print("Erro: _LifesText é nulo!");
		}
	}
}
