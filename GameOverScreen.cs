using Godot;
using System;

public partial class GameOverScreen : CanvasLayer
{
	public override void _Ready()
	{
		// O sinal 'pressed' do botão já foi conectado via Editor, 
		// então não precisamos conectar aqui no _Ready.
	}

	// A função que é chamada quando o botão é clicado
	private void OnRestartButtonPressed()
	{
		var gameManager = GetNode<GameManager>("/root/GameManager");
		gameManager.ResetGame();

		// 1. DESCONGELA O JOGO ANTES DE REINICIAR!
		// Se não fizer isso, a fase recém-carregada já começa pausada.
		GetTree().Paused = false;

		// 2. Recarrega a cena atual
		GetTree().ReloadCurrentScene();
	}
}
