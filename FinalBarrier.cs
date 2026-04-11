using Godot;
using System;

public partial class FinalBarrier : Area2D
{
    private AnimatedSprite2D _finalFlag;
    private AudioStreamPlayer _audioPlayer;
    public override void _Ready()
    {
        // Conecta o evento de colisão
        BodyEntered += OnBodyEntered;
        _finalFlag = GetNode<AnimatedSprite2D>("FinalFlag");
        _audioPlayer = GetNode<AudioStreamPlayer>("AudioPlayerFinalBarrier");
        _finalFlag.Play("deactivated");

    }

    private void OnBodyEntered(Node2D body)
    {
        // Verifica se quem entrou na barreira foi o Player
        if (body is Player)
        {
            var finishScreen = GetTree().CurrentScene.GetNode<FinishGame>("FinishGame");
            _finalFlag.Play("activated");
            _finalFlag.AnimationFinished += () => 
            {
                _audioPlayer.Play();
                finishScreen.ShowFinishScreen();
            };
            
        }
    }
}