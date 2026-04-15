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
 }
