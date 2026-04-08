using Godot;
using System;

public partial class Virginia : Area2D
{
	// Você pode definir um valor para o item, se quiser
	[Export] public int Value = 3;
	private bool _isEggsShown = false;

	private AudioStreamPlayer2D _virginiaSound;

	public override void _Ready()
	{
		// Conecta o sinal de quando algo entra na área
		BodyEntered += OnBodyEntered;
		_virginiaSound = GetNode<AudioStreamPlayer2D>("VirginiaAudioPlayer2D");
		_virginiaSound.Finished += () => showEggs(); // Mostra os ovos depois de tocar o som
	}

	private void OnBodyEntered(Node2D body)
{
	// A mágica do C#: Ele testa se quem entrou é o "Player" e já 
	// cria uma variável chamada 'Player' para acessarmos os métodos dele!
	if (body is Player player) 
	{
		_virginiaSound.Play();
	}
}

	private void Collect()
	{		
		// Remove o item do jogo
		QueueFree();
	}

	private void showEggs()
{
    var eggs = GetParent().GetNode<VBoxContainer>("Eggs");
    eggs.Visible = true;

    // Para nós de UI dentro de containers, use offset
    eggs.Position = new Vector2(-50.0f, eggs.Position.Y);

    Tween tween = GetTree().CreateTween();
    tween.TweenProperty(eggs, "position", new Vector2(0.0f, eggs.Position.Y), 0.5f)
         .SetTrans(Tween.TransitionType.Quad)
         .SetEase(Tween.EaseType.Out)
		 .Finished += () => Collect(); // Remove Virginia depois de mostrar os ovos

	_isEggsShown = true;
}

}
