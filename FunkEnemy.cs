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
    private AudioStreamPlayer _audioPlayer;

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
            Vector2 direcao = _animacao.FlipH ? Vector2.Right : Vector2.Left;
            newBullet.Direction = direcao;

            GD.Print($"Bala criada em: {newBullet.GlobalPosition} | Direção: {newBullet.Direction}");

            GetTree().Root.AddChild(newBullet);
        }
        else
        {
            GD.PrintErr("ERRO: A cena Bullet.tscn não tem o script Bullet.cs anexado!");
        }
    }

    public override void _Ready()
    {
        _animacao = GetNode<AnimatedSprite2D>("FunkEnemyAnimation");
        _gunBarrel = GetNode<Marker2D>("Marker2D");
        _Player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
        _audioPlayer = GetNode<AudioStreamPlayer>("FunkEnemyAudioPlayer");

        // ✅ Remove o Timer — não precisamos mais dele
        // Conecta o sinal de fim de animação
        _animacao.AnimationFinished += OnAnimationFinished;

        _animacao.Play("idle");
    }

    // ✅ Dispara a bala quando a animação "fire" termina um ciclo
    private void OnAnimationFinished()
    {
        if (_animacao.Animation == "fire")
        {
            Fire();
            _animacao.Play("fire"); // Reinicia a animação para o próximo ciclo
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
                if (_animacao.Animation != "fire")
                {
                    _animacao.SpeedScale = 0.5f;
                    _animacao.Play("fire");
                }
            }
            else if (distancia < AtackDistance)
            {
                if (_animacao.Animation != "draw")
                {
                    _animacao.SpeedScale = 1.0f;
                    _animacao.Play("draw");
                }
            }
            else
            {
                _animacao.SpeedScale = 0.2f;
                _animacao.Play("idle");
            }

            _animacao.FlipH = _Player.GlobalPosition.X > this.GlobalPosition.X;
        }
    }
}
