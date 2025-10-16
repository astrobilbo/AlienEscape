using System;

public interface IScoreSystem
{
    event Action ScoreChanged;
    event Action HighScoreChanged;
    
    void AddScore(int amount = 1);
    void ResetScore();
    void LoadHighScore();
    int GetScore();
    int GetHighScore();
}