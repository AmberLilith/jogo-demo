using Godot;
using System;

public partial class Hud : CanvasLayer
{
    public override void _Ready()
{
    // ✅ Pega a área segura do dispositivo
    var safeArea = DisplayServer.ScreenGetUsableRect();
    var screenSize = DisplayServer.ScreenGetSize();

    // Calcula as margens
    float leftMargin = safeArea.Position.X;
    float topMargin = safeArea.Position.Y;

    // ✅ Aplica margem no container do HUD
    var hud = GetNode<Control>("HBoxContainer");
    hud.Position = new Vector2(
        hud.Position.X + leftMargin + 20,
        hud.Position.Y + topMargin + 10
    );
}
}
