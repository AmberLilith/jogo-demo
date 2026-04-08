using Godot;
using System;

public partial class JuditePoints : Area2D
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
	// A mágica do C#: Ele testa se quem entrou é o "Player" e já 
	// cria uma variável chamada 'Player' para acessarmos os métodos dele!
	if (body is Player player) 
	{
		showEggs();
		Collect(); 
	}
}

	private void Collect()
	{		
		// Remove o item do jogo
		QueueFree();
	}

	private void showEggs(){
		var eggs = GetParent().GetNode<HBoxContainer>("Eggs");
		eggs.Visible = true;
	}
}
