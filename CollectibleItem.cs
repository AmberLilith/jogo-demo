using Godot;
using System;

public partial class CollectibleItem : Area2D
{
	// Você pode definir um valor para o item, se quiser
	[Export] public int Value = 3;

	public override void _Ready()
	{
		// Conecta o sinal de quando algo entra na área
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
{
	// A mágica do C#: Ele testa se quem entrou é o "Jogador" e já 
	// cria uma variável chamada 'jogador' para acessarmos os métodos dele!
	if (body is Jogador jogador) 
	{
		// Envia o valor (1) lá para a conta do jogador
		jogador.AddPoints(Value); 
		
		// Destrói a galinha
		Collect(); 
	}
}

	private void Collect()
	{		
		// Remove o item do jogo
		QueueFree();
	}
}
