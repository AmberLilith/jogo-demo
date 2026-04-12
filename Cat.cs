using Godot;

public partial class Cat : CharacterBody2D
{
    [Export] public float Speed = 100f;
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private AnimatedSprite2D _anim;
    private RayCast2D _rayCastLeft;
    private RayCast2D _rayCastRight;
    private float _direcao = 1f;
    private bool _inverteu = false;

    public override void _Ready()
    {
        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _rayCastLeft = GetNode<RayCast2D>("RayCastLeft");
        _rayCastRight = GetNode<RayCast2D>("RayCastRight");
    }

  public override void _PhysicsProcess(double delta)
{
    Vector2 velocity = Velocity;

    if (!IsOnFloor())
        velocity.Y += Gravity * (float)delta;

    if (IsOnFloor())
    {
        // ✅ Só inverte se ainda não inverteu nesta borda
        if (_direcao > 0 && !_rayCastRight.IsColliding() && !_inverteu)
        {
            _direcao = -1f;
            _inverteu = true;
        }
        else if (_direcao < 0 && !_rayCastLeft.IsColliding() && !_inverteu)
        {
            _direcao = 1f;
            _inverteu = true;
        }

        // ✅ Reseta a trava quando ambos os raycasts detectam chão novamente
        if (_rayCastRight.IsColliding() && _rayCastLeft.IsColliding())
            _inverteu = false;
    }

    velocity.X = Speed * _direcao;
    Velocity = velocity;
    MoveAndSlide();

    _anim.FlipH = _direcao > 0;
    _anim.Play("walk");
}
}