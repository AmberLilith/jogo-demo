using Godot;
using System;

public partial class Amber : CharacterBody2D
{
    [Export] public String animationToShow  = "idle";
    
    private AnimatedSprite2D _animation;    

    public override void _Ready()
    {
        _animation = GetNode<AnimatedSprite2D>("AmberAnimation");
        _animation.Play(animationToShow);
       
    }

/*     public override void _PhysicsProcess(double delta)
    {
        if (_player != null)
        {
            float distance = GlobalPosition.DistanceTo(_player.GlobalPosition);

            if (distance < 400.0f)
            {
                if (_animation.Animation != "celebrate")
                {
                    _animation.Play("celebrate");                   
                }
            }
            else
            {
                if (_animation.Animation != "idle")
                {
                    _animation.Play("idle");
                }
            }
        }
    }
 */
 }
