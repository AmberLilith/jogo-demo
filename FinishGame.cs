using Godot;
using System;

public partial class FinishGame : CanvasLayer
{
    private CpuParticles2D _confetti;

    private Button _startNewGameButton;   
    
    private AudioStreamPlayer _audioPlayer;

    public override void _Ready()
    {
        // Referência ao nó que você vai criar no editor
        _confetti = GetNode<CpuParticles2D>("ColorRect/Confetti");
        _startNewGameButton = GetNode<Button>("ColorRect/VBoxContainer/StartNewGameButton");
        _audioPlayer = GetNode<AudioStreamPlayer>("FinishGameAudioPlayer");
        _confetti.Emitting = false; // Começa desligado
        
        // Faz com que esta tela funcione mesmo quando o jogo estiver pausado
        ProcessMode = ProcessModeEnum.Always;
        Visible = false;
        _startNewGameButton.Pressed += OnNewGameButtonPressed;
    }

    public void ShowFinishScreen()
    {
        _audioPlayer.Play();
        GetTree().Paused = true; // Pausa o gameplay
        Visible = true;
        _confetti.Restart(); // Reinicia as partículas para garantir que comecem do zero
        _confetti.Emitting = true;
    }

    private void OnNewGameButtonPressed()
	{
		var gameManager = GetNode<GameManager>("/root/GameManager");
		gameManager.StartNewGame();

		// 1. DESCONGELA O JOGO ANTES DE REINICIAR!
		// Se não fizer isso, a fase recém-carregada já começa pausada.
		GetTree().Paused = false;

		// 2. Recarrega a cena atual
		GetTree().ReloadCurrentScene();
	}
}
