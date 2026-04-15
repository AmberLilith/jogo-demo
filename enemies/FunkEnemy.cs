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
    private AnimatedSprite2D _animation;
    private Node2D _Player;
    private Timer _timerAtirar;
    private AudioStreamPlayer _audioPlayer;

    private CollisionShape2D _collision;

    private void Fire()
    {
        if (Bullet == null || _Player == null) return;

        _audioPlayer.Play();

        var instance = Bullet.Instantiate();

        if (instance is Bullet newBullet)
        {
            newBullet.GlobalPosition = _gunBarrel.GlobalPosition;

            // ✅ Direção horizontal fixa baseada em para onde o inimigo está virado
            // FlipH = true significa que está virado para a DIREITA
            Vector2 direcao = _animation.FlipH ? Vector2.Right : Vector2.Left;
            newBullet.Direction = direcao;

            GetTree().Root.AddChild(newBullet);
        }
        else
        {
            GD.PrintErr("ERRO: A cena Bullet.tscn não tem o script Bullet.cs anexado!");
        }
    }

    public override void _Ready()
    {
        _animation = GetNode<AnimatedSprite2D>("FunkEnemyAnimation");
        _gunBarrel = GetNode<Marker2D>("Marker2D");
        _Player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
        _audioPlayer = GetNode<AudioStreamPlayer>("FunkEnemyAudioPlayer");
        _collision = GetNode<CollisionShape2D>("FunkEnemyCollision");

        // ✅ Remove o Timer — não precisamos mais dele
        // Conecta o sinal de fim de animação
        _animation.AnimationFinished += OnAnimationFinished;

        _animation.Play("idle");
    }

    // ✅ Dispara a bala quando a animação "fire" termina um ciclo
    private void OnAnimationFinished()
    {
        if (_animation.Animation == "fire")
        {
            Fire();
            _animation.Play("fire"); // Reinicia a animação para o próximo ciclo
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 vel = Velocity;
        if (!IsOnFloor())
            vel.Y += Gravity * (float)delta;
        Velocity = vel;
        MoveAndSlide();

        if (_Player != null)
        {
            float distancia = GlobalPosition.DistanceTo(_Player.GlobalPosition);

            if (distancia < FireDistance)
            {
                // ✅ Só inicia "fire" se ainda não estiver tocando
                if (_animation.Animation != "fire")
                {
                    _animation.SpeedScale = 0.5f;
                    _animation.Play("fire");
                }
            }
            else if (distancia < AtackDistance)
            {
                if (_animation.Animation != "draw")
                {
                    _animation.SpeedScale = 1.0f;
                    _animation.Play("draw");
                }
            }
            else
            {
                _animation.SpeedScale = 0.2f;
                _animation.Play("idle");
            }

            this.Scale = new Vector2(1, 1);
        }
    }
}
