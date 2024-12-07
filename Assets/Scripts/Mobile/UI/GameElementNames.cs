public static class SceneNames
{
    public const string MenuScene           = "MenuScene";

    public const string Level1Scene         = "Level 1";
    public const string Level2Scene         = "Level 2";
    public const string Level3Scene         = "Level 3";
}

public static class UiElementNames
{
    public static class Canvas
    {
        public const string Mobile          = "Mobile Canvas";
    }

    public static class Panels
    {
        public const string MainMenu        = "Main Menu Panel";
        public const string InGame          = "In Game Panel";
    }

    public static class Buttons
    {
        // main menu
        public const string StartGame       = "Solo Game Button";
        public const string QuitGame        = "Quit Game Button";

        // in game
        public const string BackToMenu      = "Back To Menu Button";
        public const string SwitchPlayer    = "Player Switch Button";
        public const string Interact        = "Player Interact Button";
        public const string RotateCamera    = "Rotate Camera Button";
        public const string UseTool         = "Use Tools Button";
    }

    public static class Texts
    {
        // in game
        public const string CountdownText   = "Countdown Text";
    }
}
