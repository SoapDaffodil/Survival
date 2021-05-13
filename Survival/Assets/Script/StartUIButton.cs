using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUIButton : MonoBehaviour
{
    public PlayerType playerType;
    public Sprite[] startButtonImage;

    private void Start()
    {
        Cursor.visible = true;
    }

    /// <summary>버튼에 커서가 들어오면 실행</summary>
    public void PointerEnter(Button button)
    {
        if (button.name == "0" || button.name == "monster"
            || button.name == "Monster" || button.name == "MONSTER")
        {
            button.image.sprite = startButtonImage[((int)PlayerType.MONSTER * 2) + 1];
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
        if (button.name == "0" || button.name == "monster"
            || button.name == "Monster" || button.name == "MONSTER")
        {
            button.image.sprite = startButtonImage[((int)PlayerType.MONSTER * 2)];
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
        /*
        if (usernameField.text == "0" || usernameField.text == "monster" || usernameField.text == "Monster" ||
            usernameField.text == "1" || usernameField.text == "human" || usernameField.text == "Human") {
            startMenu.SetActive(false);
            SetActiveMonsterKey(false);
            usernameField.interactable = false;
            Client.instance.ConnectToServer();
        }
        else
        {
            usernameField.text = "";
            usernameField.placeholder.GetComponent<Text>().text = "올바른 캐릭터를 입력해주세요\n(0: human, 1: monster)";
        }
        */
        Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        if (clickedButton.name == "0" || clickedButton.name == "monster"
            || clickedButton.name == "Monster" || clickedButton.name == "MONSTER")
        {
            playerType = PlayerType.MONSTER;
            Client.instance.ConnectToServer();
        }
        else if (clickedButton.name == "1" || clickedButton.name == "human"
            || clickedButton.name == "Human" || clickedButton.name == "HUMAN")
        {
            playerType = PlayerType.HUMAN;
            Client.instance.ConnectToServer();
        }
    }
}