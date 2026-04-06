using Godot;

public partial class Checkpoint : Area2D
{
    private bool _activated = false;

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player && !_activated)
        {
            _activated = true;
            // Salva a posição global deste checkpoint no GameManager
            GameManager.Instance.LastCheckpointPos = GlobalPosition;
            
            GD.Print("Checkpoint Ativado em: " + GlobalPosition);
            
           
            GetNode<AnimatedSprite2D>("CheckpointAnimation").Play("activated");
        }
    }
}