using Godot;
using System;

public partial class GameManager : Node

{
    public static GameManager Instance { get; private set; }
    public int MaxLives = 2;
    public int CurrentLives;
    public int Score = 0;
    public Vector2 LastCheckpointPos;

    private Player _player;

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

    public void StartNewGame()
    {
        CurrentLives = MaxLives;
        Score = 0;
        
        // Reseta o checkpoint para que o player comece na posição inicial da cena
        LastCheckpointPos = Vector2.Zero;
    }
}