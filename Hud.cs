using Godot;
using System;
using System.Threading.Tasks;

public partial class Hud : CanvasLayer
{
    [Signal] public delegate void StartGameEventHandler();

    Label score;
    Label timeLabel;
    Label message;
    Button startButton;
    Timer waitTimer;

    public override void _Ready()
    {
        score = GetNode<Label>("MarginContainer/Score");
        timeLabel = GetNode<Label>("MarginContainer/Time");
        message = GetNode<Label>("Message");
        startButton = GetNode<Button>("StartButton");
        waitTimer = GetNode<Timer>("WaitTimer");
    }

    public void UpdateScore(string newScore)
    {
        score.Text = newScore;
    }

    public void UpdateTime(string newTime) 
    {
        timeLabel.Text = newTime; 
    }

    void ShowMessage(string message)
    {
        this.message.Text = message;
        this.message.Show();
        waitTimer.Start();
    }

    public void OnTimerTimeout()
    {
        message.Hide();
    }

    public void OnStartButtonPressed()
    {
        startButton.Hide();
        message.Hide();
        EmitSignal(SignalName.StartGame);
    }

    public async void ShowGameOver()
    {
        ShowMessage("Game Over!");
        await ToSignal(waitTimer, Timer.SignalName.Timeout);
        startButton.Show();
        message.Text = "Coin Dash!";
        message.Show();
    }

}
