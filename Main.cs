using Godot;
using System;

public partial class Main : Node
{
    [Export] PackedScene CoinScene;
    [Export] int PlayTime = 30;

    int level = 1;
    int score = 0;
    int timeLeft = 0;
    Vector2 screensize = Vector2.Zero;
    bool isPlaying = false;

    Player player;
    Timer gameTimer;
    Hud hud;

    public override void _Ready()
    {
        screensize = GetViewport().GetVisibleRect().Size;
        player = GetNode<Player>("Player");
        player.screensize = screensize;
        player.Hide();
        gameTimer = GetNode<Timer>("GameTimer");
        hud = GetNode<Hud>("HUD");

        //NewGame();
    }

    public override void _Process(double delta)
    {
        if(isPlaying && GetTree().GetNodesInGroup("coins").Count == 0)
        {
            level += 1;
            timeLeft += 5;
            SpawnCoins();
        }
    }

    public void NewGame()
    {
        isPlaying = true;
        level = 1;
        score = 0;
        hud.UpdateScore(score.ToString());
        timeLeft = PlayTime;
        hud.UpdateTime(timeLeft.ToString());
        player.Start();
        player.Show();
        gameTimer.Start();
        SpawnCoins();
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < level + 4; i++)
        {
            Coin c = (Coin)CoinScene.Instantiate();
            AddChild(c);
            c.Screensize = screensize;
            c.Position = new Vector2((float)GD.RandRange(0, screensize.X), (float)GD.RandRange(0, screensize.Y));
        }
    }

    void OnPlayerHurt()
    {
        GameOver();
    }

    void OnPlayerPickup(string pickupType)
    {
        score = +1;
        hud.UpdateScore(score.ToString());
    }

    public void OnHUDStartGame()
    {
        NewGame();
    }

    public void OnGameTimerTimeout()
    {
        timeLeft -= 1;
        hud.UpdateTime(timeLeft.ToString());
        if (timeLeft <= 0) GameOver();
    }

    void GameOver()
    {
        isPlaying = false;
        gameTimer.Stop();
        GetTree().CallGroup("coins", "queue_free");
        hud.ShowGameOver();
        player.Die();
    }

}
