using Godot;
using System;

public partial class FallingBlock : CharacterBody2D
{
    private Timer _timer;
    private Area2D _area;

    private bool _playerOnTop = false;
    private bool _falling = false;

    private Tween _shakeTween;
    private Vector2 _originalPosition;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _area = GetNode<Area2D>("Area2D");

        _timer.WaitTime = 2.0;
        _timer.OneShot = true;

        _area.BodyEntered += OnBodyEntered;
        _area.BodyExited += OnBodyExited;
        _timer.Timeout += OnTimerTimeout;

        _originalPosition = Position;
    }

    public override void _PhysicsProcess(double delta) { }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("Player") && !_falling)
        {
            _playerOnTop = true;
            _timer.Start();
            StartShake();
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body.IsInGroup("Player") && !_falling)
        {
            _playerOnTop = false;
            _timer.Stop();
            StopShake();
        }
    }

    private void OnTimerTimeout()
    {
        if (_playerOnTop)
        {
            StopShake();
            StartFall();
        }
    }

    private void StartFall()
    {
        _falling = true;

        // ✅ Tween de queda — cai 800px em 0.8s e depois QueueFree
        var fallTween = CreateTween();
        fallTween.TweenProperty(this, "position:y", Position.Y + 800f, 0.8f)
                 .SetTrans(Tween.TransitionType.Quad)
                 .SetEase(Tween.EaseType.In);

        // ✅ QueueFree quando a queda terminar
        fallTween.Finished += () => QueueFree();
    }

    private void StartShake()
    {
        StopShake();

        _shakeTween = CreateTween();
        _shakeTween.SetLoops();

        float shakeAmount = 3f;
        _shakeTween.TweenProperty(this, "position:x", _originalPosition.X - shakeAmount, 0.05f);
        _shakeTween.TweenProperty(this, "position:x", _originalPosition.X + shakeAmount, 0.05f);
    }

    private void StopShake()
    {
        if (_shakeTween != null)
        {
            _shakeTween.Kill();
            _shakeTween = null;
        }
        Position = _originalPosition;
    }
}