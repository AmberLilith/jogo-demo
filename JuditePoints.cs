using Godot;
using System;

public partial class JuditePoints : Area2D
{
	// Você pode definir um valor para o item, se quiser
	[Export] public int Value = 3;
	private bool _isEggsShown = false;

	private AudioStreamPlayer2D _juditeSound;

	public override void _Ready()
	{
		// Conecta o sinal de quando algo entra na área
		BodyEntered += OnBodyEntered;
		_juditeSound = GetNode<AudioStreamPlayer2D>("JuditeAudioPlayer2D");
		_juditeSound.Finished += () => showEggs(); // Mostra os ovos depois de tocar o som
	}

	private void OnBodyEntered(Node2D body)
{
	// A mágica do C#: Ele testa se quem entrou é o "Player" e já 
	// cria uma variável chamada 'Player' para acessarmos os métodos dele!
	if (body is Player player) 
	{
		_juditeSound.Play();
	}
}

	private void Collect()
	{		
		// Remove o item do jogo
		QueueFree();
	}

	private void showEggs()
{
    var eggs = GetParent().GetNode<HBoxContainer>("Eggs");
    eggs.Visible = true;

    // Para nós de UI dentro de containers, use offset
    eggs.Position = new Vector2(-300.0f, eggs.Position.Y);

    Tween tween = GetTree().CreateTween();
    tween.TweenProperty(eggs, "position", new Vector2(0.0f, eggs.Position.Y), 0.5f)
         .SetTrans(Tween.TransitionType.Quad)
         .SetEase(Tween.EaseType.Out)
		 .Finished += () => Collect(); // Remove o JuditePoints depois de mostrar os ovos

	_isEggsShown = true;
}

}
