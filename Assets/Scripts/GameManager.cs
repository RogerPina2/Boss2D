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

    public int lifes;
    public int points;
    public bool isPaused = false;

    private GameManager()
    {
        lifes = 3;
        points = 0;
    }

    private void Reset()
    {
        lifes = 3;
        points = 0;
    }
}
