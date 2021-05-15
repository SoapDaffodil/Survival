using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUIButton : MonoBehaviour
{
    public Sprite[] startButtonImage;

    private void Start()
    {
        Cursor.visible = true;
    }

    /// <summary>버튼에 커서가 들어오면 실행</summary>
    public void PointerEnter(Button button)
    {
        if (button.name == "0" || button.name == "creature"
            || button.name == "Creature" || button.name == "CREATURE")
        {
            button.image.sprite = startButtonImage[((int)PlayerType.CREATURE * 2) + 1];
        }
        else if (button.name == "1" || button.name == "human"
            || button.name == "Human" || button.name == "HUMAN")
        {
            button.image.sprite = startButtonImage[((int)PlayerType.HUMAN * 2) + 1];
        }
    }

    /// <summary>버튼에 커서가 빠져나가면 실행</summary>
    public void PointerExit(Button button)
    {
        if (button.name == "0" || button.name == "creature"
            || button.name == "Creature" || button.name == "CREATURE")
        {
            button.image.sprite = startButtonImage[((int)PlayerType.CREATURE * 2)];
        }
        else if (button.name == "1" || button.name == "human"
            || button.name == "Human" || button.name == "HUMAN")
        {
            button.image.sprite = startButtonImage[((int)PlayerType.HUMAN * 2)];
        }
    }

    /// <summary>연결을 시작하면 UI숨기고 client를 server에 연결</summary>
    public void ConnectToServer()
    {
        Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        if (clickedButton.name == "0" || clickedButton.name == "creature"
            || clickedButton.name == "Creature" || clickedButton.name == "CREATURE")
        {
            Client.playerType = PlayerType.CREATURE;
        }
        else if (clickedButton.name == "1" || clickedButton.name == "human"
            || clickedButton.name == "Human" || clickedButton.name == "HUMAN")
        {
            Client.playerType = PlayerType.HUMAN;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Play");
    }
}