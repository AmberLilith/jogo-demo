using Godot;

public partial class Precipice : Area2D
{

	private AnimatedSprite2D _animation;
	public override void _Ready()
	{
		_animation = GetNode<AnimatedSprite2D>("DeathZoneAnimation");
		_animation.Play("water");
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		// Check if the body that fell is the Player
		// Note: Make sure your Player class is now named "Player"
		if (body is Player player)
		{
			player.Die();
		}
        else if (!(body is TileMap) && !(body is TileMapLayer)) 
    {
        body.QueueFree();
    }
	}
}
