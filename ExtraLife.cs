using Godot;
using System;

public partial class ExtraLife : Area2D
{
	// Você pode definir um valor para o item, se quiser
	[Export] public int Value = 1;

	private AudioStreamPlayer _audioPlayer;

	public override void _Ready()
	{
		_audioPlayer = GetNode<AudioStreamPlayer>("ExtraLifeAudioPlayer");
		// Conecta o sinal de quando algo entra na área
		BodyEntered += OnBodyEntered;
		Spin();
	}

private async void OnBodyEntered(Node2D body)
{
	if (body is Player)
	{
		_audioPlayer.Play();
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);

		// 1. Pegamos a Label
		Label lifesLabel = GetParent().GetNode<Label>("../HUD/HBoxContainer/Lifes");

		// 2. A MÁGICA: Pegamos a posição da Label na TELA (Canvas)
		// e convertemos para a posição exata onde ela parece estar no MUNDO agora.
		Vector2 screenPos = lifesLabel.GlobalPosition; 
		Vector2 targetWorldPos = GetViewport().GetCanvasTransform().AffineInverse() * screenPos;

		// 3. O Tween agora usa a posição calculada do mundo
		var flyTween = CreateTween().SetParallel(true);
		
		flyTween.TweenProperty(this, "global_position", targetWorldPos, 0.6f)
			.SetTrans(Tween.TransitionType.Quad)
			.SetEase(Tween.EaseType.Out); // "Out" faz ele começar rápido e frear na chegada
			
		flyTween.TweenProperty(this, "scale", new Vector2(0.01f, 0.01f), 0.6f);

		await ToSignal(flyTween, "finished");
		
		GameManager.Instance.AddLifes(1);
		QueueFree();
	}
}

	private void Collect()
	{		
		// Remove o item do jogo
		QueueFree();
	}

	private void Spin()
	{
		var sprite = GetNode<Sprite2D>("ExtraLifeSprite");
		var tween = CreateTween().SetLoops();

	// 1. Achata o coração (parece que girou de lado)
	tween.TweenProperty(sprite, "scale:x", 0.0f, 0.5f)
		 .SetTrans(Tween.TransitionType.Sine)
		 .SetEase(Tween.EaseType.InOut);

	// 2. Expande de volta (completando a meia volta)
	// Usamos -1.0f para ele "espelhar", simulando o verso do item
	tween.TweenProperty(sprite, "scale:x", 0.122f, 0.5f)
		 .SetTrans(Tween.TransitionType.Sine)
		 .SetEase(Tween.EaseType.InOut);

	}
}
