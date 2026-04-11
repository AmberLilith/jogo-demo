using Godot;
using System;

public partial class FinalBarrer : Area2D
{
    public override void _Ready()
    {
        // Conecta o evento de colisão
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        // Verifica se quem entrou na barreira foi o Player
        if (body is Player)
        {
            var finishScreen = GetTree().CurrentScene.GetNode<FinishGame>("FinishGame");
            finishScreen.ShowFinishScreen();
        }
    }
}
