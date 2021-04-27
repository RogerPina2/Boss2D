using UnityEngine;
using UnityEngine.UI;

public class UI_Lives : MonoBehaviour
{
    private Text textComp;
    private GameManager gm;

    private void Start()
    {
        textComp = GetComponent<Text>();
        gm = GameManager.GetInstance();
    }

    private void Update()
    {
        textComp.text = $"Lives: {gm.lifes}";
    }
}