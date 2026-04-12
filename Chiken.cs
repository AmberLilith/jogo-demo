using Godot;
using System;
using System.Collections;

public abstract partial class Chiken : Area2D
{
    // Você pode definir um valor para o item, se quiser 
    [Export] public int eggsAmount = 3;
    private bool _isEggsShown = false;
    private Node2D _eggs;
    public const float RunSpeed = 200.0f;
    [Export] public CollisionShape2D collisonNode; //Arraste o nó correspondente da galinha aqui no inspetor

    [Export] public AnimatedSprite2D AnimationNode; //Arraste o nó correspondente da galinha aqui no inspetor
    [Export] public AudioStreamPlayer2D SoundNode; //Arraste o nó correspondente da galinha aqui no inspetor
    [Export] public Node2D EggsNode; //Arraste o nó correspondente da galinha aqui no inspetor

    public override void _Ready()
    {
        // Agora você não precisa de GetNode com nomes! 
        // Você arrasta o nó direto no Inspetor.
        
        if (AnimationNode != null && AnimationNode.SpriteFrames.HasAnimation("idle"))
        {
            AnimationNode.Play("idle");
        }

        BodyEntered += OnBodyEntered;
        
        if (SoundNode != null)
            SoundNode.Finished += () => showEggs();
        _eggs = GetNode<Node2D>("Eggs");
        SoundNode.Finished += () => showEggs();
        // Mostra os ovos depois de tocar o som 
    }
   private void OnBodyEntered(Node2D body)
{
    if (body is Player player)
    {
        SoundNode.Play();

        if (AnimationNode.SpriteFrames.HasAnimation("run"))
        {
            // CALCULA A DIREÇÃO:
            // Se a posição X do player for menor que a da galinha, o player vem da ESQUERDA.
            // Então a galinha deve fugir para a DIREITA (valor positivo).
            float runAwayTowards = (player.GlobalPosition.X < GlobalPosition.X) ? 1.0f : -1.0f;

            // Gira o sprite: Se for para a esquerda (-1), FlipH = true.
            // Se o seu sprite original já olha para a esquerda, inverta o sinal abaixo:
            AnimationNode.FlipH = runAwayTowards > 0;

            AnimationNode.Play("run");

            Tween tween = GetTree().CreateTween();
            
            // Move baseado na direção calculada (fugirParaDirecao será 1 ou -1)
            float distanciaFuga = 100.0f * runAwayTowards;

            tween.TweenProperty(AnimationNode, "position:x", AnimationNode.Position.X + distanciaFuga, 0.5f)
                 .SetTrans(Tween.TransitionType.Quad)
                 .SetEase(Tween.EaseType.Out);

            tween.Finished += () => 
            {
                if (AnimationNode.SpriteFrames.HasAnimation("scratch"))
                {
                    AnimationNode.Play("scratch");
                }
            };
        }
    }
}
public override void _PhysicsProcess(double delta)
{
    Vector2 direction = new Vector2(Input.GetAxis("ui_left", "ui_right"), 0);
}
    private async void Collect()
    {
        // 1. Desativa a colisão imediatamente para o player não interagir mais
        collisonNode.SetDeferred("disabled", true);
        
        // 2. Torna o visual invisível (opcional, caso queira que ela suma da tela mas espere o tempo)
        // AnimationNode.Visible = false;

        // 3. Cria um timer de 10 segundos e aguarda o sinal de término (timeout)
        // O await permite que o código "pare" aqui sem travar o resto do jogo
        await ToSignal(GetTree().CreateTimer(60.0f), SceneTreeTimer.SignalName.Timeout);

        // 4. Após 10 segundos, remove a galinha do jogo
        AnimationNode.QueueFree();
    }

    private void showEggs()
    {
        _eggs.Visible = true;

        foreach (var egg in _eggs.GetChildren())
        {
            foreach (var area in egg.GetChildren())
            {
                if (egg is Egg eggArea)
                {
                    eggArea.EnableCollision();
                }

            }

        }
        Collect();

        _isEggsShown = true;
    }
}