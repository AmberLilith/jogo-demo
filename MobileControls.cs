using Godot;

public partial class MobileControls : CanvasLayer
{
    [Export] public bool ForcarMobile = false; // ✅ marque no Inspetor para testar

    private Texture2D CriarTexturaCirculo(Color cor)
{
    var image = Image.Create(80, 80, false, Image.Format.Rgba8);
    for (int x = 0; x < 80; x++)
        for (int y = 0; y < 80; y++)
        {
            float dx = x - 40, dy = y - 40;
            if (dx * dx + dy * dy <= 40 * 40)
                image.SetPixel(x, y, cor);
        }
    return ImageTexture.CreateFromImage(image);
}


    public override void _Ready()
    {
        
        bool isMobile = DisplayServer.IsTouchscreenAvailable() || ForcarMobile;
        
        if (!isMobile)
        {
            QueueFree();
            return;
        }
    }
}