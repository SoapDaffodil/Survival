using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;

    /// <summary>EMP설치게이지</summary>
    [Tooltip("EMP설치게이지")]
    public Slider powerSlider;

    /// <summary>현재 쏠 수 있는 탄수</summary>
    [Tooltip("현재 쏠 수 있는 탄수")]
    public Text currentBulletText;

    /// <summary>장전가능한 탄 수</summary>
    [Tooltip("장전가능한 탄 수")]
    public Text bulletAmoutText;

    /// <summary>1층 맵 플레이어위치</summary>
    [Tooltip("1층 맵 플레이어위치")]
    public GameObject fisrtFloorPlayer;

    /// <summary>맵 UI</summary>
    [Tooltip("맵 UI")]
    public GameObject map;
    public bool mapActive = false;

    /// <summary>LightTrap 버튼 리스트</summary>
    public UnityEngine.UI.Button[] lightTrapUIButton;

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
        map.SetActive(mapActive);
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
