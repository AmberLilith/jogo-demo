using Godot;

public partial class StartScreen : Control
{
    private Button _btnPlay;
    private CheckButton _btnSound;

    public override void _Ready()
    {
        _btnPlay = GetNode<Button>("TextureRect/VBoxContainer/BtnPlay");
        _btnSound = GetNode<CheckButton>("TextureRect/HBoxContainer/MarginContainer/BtnSound");

        _btnPlay.Pressed += OnPlayPressed;
        _btnSound.Toggled += OnSomToggled;

        // ✅ Inicia com som ligado
        _btnSound.ButtonPressed = true;
        AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), false);
    }

    public override void _Input(InputEvent @event)
    {
        // ✅ Qualquer tecla também inicia o jogo
        if (@event is InputEventKey key && key.Pressed)
            startGame();
    }

    private void OnPlayPressed()
    {
        startGame();
    }

    private void OnSomToggled(bool ligado)
    {
        // ✅ Muta/desmuta o barramento master
        AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), !ligado);
    }

    private void startGame()
    {
        GetTree().ChangeSceneToFile("res://forest_stage.tscn");
    }
}