using Godot;
using System;
public partial class Judite : Area2D
{
	// Você pode definir um valor para o item, se quiser 
	[Export] public int Value = 3;
	 private bool _isEggsShown = false; 
	 private AudioStreamPlayer2D _juditeSound;
	 private Node2D _eggs;
	  public override void _Ready()
	{
		// Conecta o sinal de quando algo entra na área 
		BodyEntered += OnBodyEntered;
		_juditeSound = GetNode<AudioStreamPlayer2D>("JuditeAudioPlayer2D");
		_eggs = GetNode<Node2D>("Eggs");
		_juditeSound.Finished += () => showEggs();
		// Mostra os ovos depois de tocar o som 
	}
	private void OnBodyEntered(Node2D body)
	{ // A mágica do C#: Ele testa se quem entrou é o "Player" e já 
	  // cria uma variável chamada 'Player' para acessarmos os métodos dele!
		if (body is Player player)
		{
			_juditeSound.Play();
	
		}
	}
	private void Collect()
	{
		// Remove o item do jogo
		//QueueFree();
	}
	private void showEggs()
{
    var currentEggsPosY = _eggs.Position.Y;
    _eggs.Position = new Vector2(5.0f, currentEggsPosY);
    _eggs.Visible = true;

    Tween tween = GetTree().CreateTween();
	
    tween.TweenProperty(_eggs, "position", new Vector2(40.0f, currentEggsPosY), 1.0f)
         .SetTrans(Tween.TransitionType.Quad)
         .SetEase(Tween.EaseType.Out);

    tween.Finished += () => {
        foreach (var egg in _eggs.GetChildren())
        {	
			foreach(var area in egg.GetChildren())
			{
				if(egg is Egg eggArea)
					{
					eggArea.EnableCollision();
					}
				
			}			
            
        }
        Collect();
    };

    _isEggsShown = true;
}
}