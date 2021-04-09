using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager _instance;

    public static GameManager GetInstance()
    {
        if(_instance == null)
            _instance = new GameManager();
        
        return _instance;
    }

    public delegate void ChangeStateDelegate();
    public static ChangeStateDelegate changeStateDelegate;

    public enum GameState {MENU, GAME, PAUSE, ENDGAME};

    public GameState gameState {get; private set;}
    public int lifes;
    public int points;

    private GameManager()
    {
        lifes = 3;
        points = 0;
        gameState = GameState.MENU;
    }

    public void ChangeState(GameState nextState)
    {
        if (nextState == GameState.GAME) Reset();

        gameState = nextState;
        changeStateDelegate();
    }

    private void Reset()
    {
        lifes = 3;
        points = 0;
    }
}
