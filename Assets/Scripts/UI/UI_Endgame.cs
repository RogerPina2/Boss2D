using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Endgame : MonoBehaviour
{
    public Text message;
    GameManager gm;

    private void OnEnable()
    {
        gm = GameManager.GetInstance();

        if (gm.lifes > 0)
            message.text = "Você Ganhou!!!";
        else
            message.text = "Você Perdeu!!!";
    }

    public void Resume()
    {
        gm.ChangeState(GameManager.GameState.GAME);
    }

    public void Menu()
    {
        gm.ChangeState(GameManager.GameState.MENU);
    }
}
