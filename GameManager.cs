using Godot;
using System;

public partial class GameManager : Node
{
    // Variáveis globais
    public int MaxLives = 3;
    public int CurrentLives;
    public int Score = 0;

    public override void _Ready()
    {
        // Inicializa as vidas quando o jogo começa
        CurrentLives = MaxLives;
    }

    // Função para resetar tudo
    public void ResetGame()
    {
        CurrentLives = MaxLives;
        Score = 0;
    }
}