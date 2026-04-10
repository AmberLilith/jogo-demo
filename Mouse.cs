using Godot;
using System;
using System.Collections.Generic;

public partial class Mouse : CharacterBody2D
{
    private AnimatedSprite2D _animation;
    private AudioStreamPlayer2D _audioPlayer;
    private VisibleOnScreenNotifier2D _notifier;
    private Node2D _Player;
    public Vector2 Direction = Vector2.Zero;

    [Export] public float Speed = -200.0f; // Negativo para ir para a esquerda
    [Export] public float AtackDistance = 400.0f;
    
    private Dictionary<int, float> _alturasPorFrame = new Dictionary<int, float>()
    {
        { 0, 0.0f },
        { 1, -5.0f },
        { 2, -15.0f },
        { 3, -20.0f },
        { 4, -20.0f },
        { 5, -10.0f },
        { 6, 0.0f }
    };

    private float _startY;

    public override void _Ready()
    {
        _animation = GetNode<AnimatedSprite2D>("MouseAnimation");
        _audioPlayer = GetNode<AudioStreamPlayer2D>("MouseAudioPlayer");
        _Player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
        float distance = GlobalPosition.DistanceTo(_Player.GlobalPosition);
        
        // Criamos o Notificador de tela via código (ou você pode adicionar no editor)
        _notifier = new VisibleOnScreenNotifier2D();
        AddChild(_notifier);
        
        // Conecta o evento de sair da tela ao QueueFree
        _notifier.ScreenExited += () => QueueFree();

        _startY = Position.Y;
        _animation.Play("idle");

        if(_animation.Animation.Equals("run"))
        {            
            _audioPlayer.Play();
            
        }  
        
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        Position += Direction * Speed * (float)delta;

        // 1. Define a velocidade horizontal (sempre para a esquerda)
        velocity.X = Speed;

        // 2. Aplica a velocidade ao corpo
        Velocity = velocity;
        MoveAndSlide();
        
       
        {
            float distance = GlobalPosition.DistanceTo(_Player.GlobalPosition);
            if (distance < AtackDistance)
            {
                if(_animation.Animation != "run")
                {
                    _animation.Play("run");
                    _audioPlayer.Play();
                }   
            }
        }
    }


}