using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Pause : MonoBehaviour
{
    GameManager gm;

    private void OnEnable()
    {
        gm = GameManager.GetInstance();
    }

    public void Resume()
    {
        gm.ChangeState(GameManager.GameState.GAME);
    }

    public void Restart()
    {
        Debug.Log("PRECISA IMPLEMENTAR O RESTART");
        gm.ChangeState(GameManager.GameState.GAME);
    }

    public void Menu()
    {
        gm.ChangeState(GameManager.GameState.MENU);
    }
}

