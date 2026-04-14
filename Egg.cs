using Godot;
using System;

public partial class Egg : Area2D
{
	private AudioStreamPlayer _audioPlayer;
    	public override void _Ready()
	{
		_audioPlayer = GetNode<AudioStreamPlayer>("EggAudioPlayer");
		 GetNode<CollisionShape2D>("EggCollision").SetDeferred("disabled", true);
		// Conecta o sinal de quando algo entra na área
		BodyEntered += OnBodyEntered;
	}

private async void OnBodyEntered(Node2D body)
{
	if (body is Player player)
	{
		_audioPlayer.Play();

		// 1. Pegamos a Label
		Label scoreLabel = GetTree().Root.GetNode<Label>("World/HUD/HBoxContainer/MarginContainerScore/Score");

		// 2. A MÁGICA: Pegamos a posição da Label na TELA (Canvas)
		// e convertemos para a posição exata onde ela parece estar no MUNDO agora.
		Vector2 screenPos = scoreLabel.GlobalPosition; 
		Vector2 targetWorldPos = GetViewport().GetCanvasTransform().AffineInverse() * screenPos;

		// 3. O Tween agora usa a posição calculada do mundo
		var flyTween = CreateTween().SetParallel(true);
		
		flyTween.TweenProperty(this, "global_position", targetWorldPos, 0.6f)
			.SetTrans(Tween.TransitionType.Quad)
			.SetEase(Tween.EaseType.Out); // "Out" faz ele começar rápido e frear na chegada
			
		flyTween.TweenProperty(this, "scale", new Vector2(0.01f, 0.01f), 0.6f);

		await ToSignal(flyTween, "finished");

        GameManager.Instance.AddEgg(1);

		QueueFree();
	}
}

public void EnableCollision()
{
    GetNode<CollisionShape2D>("EggCollision").Disabled = false;
}

}
