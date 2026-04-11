using Godot;
using System;

public partial class FinalBarrier : StaticBody2D
{
    private AnimatedSprite2D _finalFlag;
    private Area2D _triggerArea;

    public override void _Ready()
    {
        _finalFlag = GetNode<AnimatedSprite2D>("FinalFlag");
        
        // Procuramos a Area2D que você deve adicionar como filha no Editor
        _triggerArea = GetNode<Area2D>("TriggerArea");
        _triggerArea.BodyEntered += OnBodyEntered;

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
                GD.Print("Player aqui");
                finishScreen.ShowFinishScreen();
            };
            
        }
    }
}