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

    /// <summary>몬스터의 현재 키배치</summary>
    [Tooltip("몬스터의 현재 키배치")]
    public Text[] monsterKey;

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
            SetActiveFalseMonsterKey();
            usernameField.interactable = false;
            Client.instance.ConnectToServer();
        }
        else
        {
            usernameField.text = "";
            usernameField.placeholder.GetComponent<Text>().text = "올바른 캐릭터를 입력해주세요\n(0: human, 1: monster)";
        }
    }

    public void SetActiveTrueMonsterKey()
    {
            for (int i = 0; i < monsterKey.Length; i++)
            {
                monsterKey[i].gameObject.SetActive(true);
            }       
    }

    public void SetActiveFalseMonsterKey()
    {
        for (int i = 0; i < monsterKey.Length; i++)
        {
            monsterKey[i].gameObject.SetActive(false);
        }
    }
        
}
