using Godot;

public partial class Checkpoint : Area2D
{
    private bool _activated = false;

    private AudioStreamPlayer _audioPlayer;

    public override void _Ready()
    {
        _audioPlayer = GetNode<AudioStreamPlayer>("CheckpointAudioPlayer");
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player && !_activated)
        {
            _activated = true;
            _audioPlayer.Play();
            // Salva a posição global deste checkpoint no GameManager
            GameManager.Instance.LastCheckpointPos = GlobalPosition;          
            GetNode<AnimatedSprite2D>("CheckpointAnimation").Play("activated");
        }
    }
}