using Godot;

public partial class Cat : CharacterBody2D
{
    [Export] public float Speed = 100f;
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private AnimatedSprite2D _anim;
    private RayCast2D _rayCastLeft; //Funciona como um sensor para detectar se há chão
    private RayCast2D _rayCastRight; //Funciona como um sensor para detectar se há chão
    private float _direcao = 1f;
    private bool _reversed = false;
    private AudioStreamPlayer _audioPlayer;

    public override void _Ready()
    {
        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _rayCastLeft = GetNode<RayCast2D>("RayCastLeft");
        _rayCastRight = GetNode<RayCast2D>("RayCastRight");
        _audioPlayer = GetNode<AudioStreamPlayer>("CatAudioPlayer");
    }

  public override void _PhysicsProcess(double delta)
{
    Vector2 velocity = Velocity;

    if (!IsOnFloor())
        velocity.Y += Gravity * (float)delta;

    if (IsOnFloor())
    {
        // ✅ Só inverte se ainda não inverteu nesta borda
        if (_direcao > 0 && !_rayCastRight.IsColliding() && !_reversed)
        {
            _direcao = -1f;
            _reversed = true;
        }
        else if (_direcao < 0 && !_rayCastLeft.IsColliding() && !_reversed)
        {
            _direcao = 1f;
            _reversed = true;
        }

        // ✅ Reseta a trava quando ambos os raycasts detectam chão novamente
        if (_rayCastRight.IsColliding() && _rayCastLeft.IsColliding())
            _reversed = false;
    }

    for (int i = 0; i < GetSlideCollisionCount(); i++) //Como Cat é um CharacterBody2D, não usamos método onBodyEntered. A detecção é feita via GetSlideCollision
    {
        var collision = GetSlideCollision(i);
        if (collision.GetCollider() is Player)
        {
            _audioPlayer.Play(); // toca o som do gato ao acertar o player
        }
    }

    velocity.X = Speed * _direcao;
    Velocity = velocity;
    MoveAndSlide();

    _anim.FlipH = _direcao > 0;
    _anim.Play("walk");
}
}