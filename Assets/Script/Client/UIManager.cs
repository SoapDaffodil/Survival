using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;

    /// <summary>이미 존재하는지 체크</summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destoying object!");
            Destroy(this);
        }
    }

    /// <summary>연결을 시작하면 UI숨기고 client를 server에 연결</summary>
    public void ConnectToServer()
    {
        if (usernameField.text == "0" || usernameField.text == "monster" || usernameField.text == "Monster" ||
            usernameField.text == "1" || usernameField.text == "human" || usernameField.text == "Human") {
            startMenu.SetActive(false);
            usernameField.interactable = false;
            Client.instance.ConnectToServer();
        }
        else
        {
            usernameField.text = "";
            usernameField.placeholder.GetComponent<Text>().text = "올바른 캐릭터를 입력해주세요\n(0: human, 1: monster)";
        }
    }
}
