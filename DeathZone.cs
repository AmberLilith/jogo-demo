using Godot;

public partial class DeathZone : Area2D
{
	public override void _Ready()
	{
		// Connect the signal to detect when a body enters the pit
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
		else
		{
			// If an enemy or item falls, just remove it from memory
			body.QueueFree();
		}
	}
}
