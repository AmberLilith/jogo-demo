using Godot;
using System;

public partial class GameManager : Node

{
    public static GameManager Instance { get; private set; }
    public int MaxLives = 2;
    public int CurrentLifes;
    public int Eggs = 0;
    public Vector2 LastCheckpointPos;

    private Player _player;

    private const int ScoresToGetExtraLife = 6;

    private AudioStreamPlayer _extraLifeSound;

    public override void _Ready()
    {
        Instance = this;
        CurrentLifes = MaxLives;
        // Começa o checkpoint na posição inicial
        LastCheckpointPos = Vector2.Zero;
        _extraLifeSound = new AudioStreamPlayer();
        _extraLifeSound.Stream = GD.Load<AudioStream>("res://audios/extra_life.mp3");
        AddChild(_extraLifeSound);
    }

    // Função para resetar tudo
    public void ResetGame()
    {
        CurrentLifes = MaxLives;
        Eggs = 0;
    }

    public void StartNewGame()
    {
        CurrentLifes = MaxLives;
        Eggs = 0;

        // Reseta o checkpoint para que o player comece na posição inicial da cena
        LastCheckpointPos = Vector2.Zero;
    }

    public void AddEgg(int value)
    {
        Eggs += value;

        // ✅ Verifica se ganhou vida extra
        while (Eggs >= ScoresToGetExtraLife)
        {
            Eggs -= ScoresToGetExtraLife;
            CurrentLifes++;
            _extraLifeSound.Play();
            AnimateExtraLifeLabel();
        }

        // ✅ Atualiza os labels
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        var scoreLabel = GetTree().Root.GetNodeOrNull<Label>("World/HUD/HBoxContainer/Score");
        var livesLabel = GetTree().Root.GetNodeOrNull<Label>("World/HUD/HBoxContainer/Lifes");

        if (scoreLabel != null)
            scoreLabel.Text = $"Ovos de Bebezinhas: {Eggs}";
        if (livesLabel != null)
            livesLabel.Text = $"Vidas: {CurrentLifes}";
    }

    public void AddLifes(int value)
    {
        var lifeLabel = GetTree().Root.GetNodeOrNull<Label>("World/HUD/HBoxContainer/Lifes");
        CurrentLifes += value;
        lifeLabel.Text = $"Vidas {CurrentLifes}";

    }

    private void AnimateExtraLifeLabel()
{
    var label = GetTree().Root.GetNodeOrNull<Label>("World/HUD/HBoxContainer/Lifes");
    if (label == null) return;

    // ✅ Reseta o estado antes de animar
    label.Scale = Vector2.One;
    label.Modulate = new Color(1, 1, 1, 1);

    Tween tween = CreateTween().SetParallel(true);

    // ✅ Cresce e volta ao tamanho normal
    tween.TweenProperty(label, "scale", new Vector2(1.8f, 1.8f), 0.2f)
         .SetTrans(Tween.TransitionType.Bounce)
         .SetEase(Tween.EaseType.Out);

    // ✅ Muda para dourado
    tween.TweenProperty(label, "modulate", new Color(0.92f, 0.04f, 0.04f, 0.8f), 0.2f);

    // ✅ Após crescer, volta ao normal
    tween.Chain();
    tween.TweenProperty(label, "scale", Vector2.One, 0.3f)
         .SetTrans(Tween.TransitionType.Bounce)
         .SetEase(Tween.EaseType.Out);
    tween.TweenProperty(label, "modulate", new Color(1, 1, 1, 1), 0.3f);
}
}