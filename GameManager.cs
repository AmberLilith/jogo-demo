using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public int MaxLives = 3;
    public int CurrentLives;
    public int Score = 0;
    public Vector2 LastCheckpointPos;

    public override void _Ready()
    {
        Instance = this;
        CurrentLives = MaxLives;
        // Começa o checkpoint na posição inicial
        LastCheckpointPos = Vector2.Zero;
    }

    // Função para resetar tudo
    public void ResetGame()
    {
        CurrentLives = MaxLives;
        Score = 0;
    }
}