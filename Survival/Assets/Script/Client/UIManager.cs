using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public PlayerType playerType;
    public Sprite[] startButtonImage;

    public Image[] HPBarUI;
    public Sprite[] HPBarImage;
    public Image[] itemImageUI;
    public Image[] skillImageUI;
    public Sprite[] itemGrayImage;
    public Sprite[] skillGrayImage;
    public Sprite[] itemImage;
    public Sprite[] skillImage;

    public Image[] coolTimeBackGroundImage;
    public Text[] coolTimeText;
    public float seconds = 10f;

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

    /// <summary>몬스터의 현재 키배치</summary>
    [Tooltip("몬스터의 현재 키배치")]
    public Text[] monsterKey;

    /// <summary>플레이어 체력 회복 게이지</summary>
    [Tooltip("플레이어 체력 회복 게이지")]
    public Slider hpSlider;



    public GameObject[] UI_LightTrapList;
    public Material[] material_UI_LightTrap;
    public Vector3[] position_UI_LightTrap = { new Vector3(-100, 0, 0), new Vector3(-200, 0, 0) };

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

    private void Update()
    {
        if(GameObject.FindWithTag("Player") != null)
        {
            if (GameManager.players[Client.instance.myId].playerType == PlayerType.MONSTER && GameManager.players[Client.instance.myId].isMonsterAttack)
            {
                if (seconds > 0)
                {
                    seconds -= Time.deltaTime;
                    Debug.Log(seconds);
                }
                MonsterSkillUIControll(seconds);
            }
        }
    }

    /// <summary>버튼에 커서가 들어오면 실행</summary>
    public void PointerEnter()
    {
        Button button = UnityEngine.EventSystems.EventSystem.current.GetComponent<Button>();

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
    public void PointerExit()
    {
        Button button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

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
            startMenu.SetActive(false);
            SetActiveMonsterKey(false);
            Client.instance.ConnectToServer();
        }
        else if (clickedButton.name == "1" || clickedButton.name == "human"
            || clickedButton.name == "Human" || clickedButton.name == "HUMAN")
        {
            playerType = PlayerType.HUMAN;
            startMenu.SetActive(false);
            SetActiveMonsterKey(false);
            Client.instance.ConnectToServer();
        }
    }

    public void SetLightTrapUI()
    {
        for (int i=0;i< UI_LightTrapList.Length;i++)
        {
            if (i >= ItemSpawner.lightTrapList.Count)
            {
                UI_LightTrapList[i].SetActive(false);
                lightTrapUIButton[i].gameObject.SetActive(false);
                continue;
            }
            UI_LightTrapList[i].SetActive(true);
            lightTrapUIButton[i].gameObject.SetActive(true);
            UI_LightTrapList[i].transform.position = ItemSpawner.lightTrapList[i].trap.transform.position + position_UI_LightTrap[ItemSpawner.lightTrapList[i].floor - 1];
            UI_LightTrapList[i].GetComponent<MeshRenderer>().material = material_UI_LightTrap[i];
        }
    }

    public void SetActiveMonsterKey(bool _active)
    {
        for (int i = 0; i < monsterKey.Length; i++)
        {
            monsterKey[i].gameObject.SetActive(_active);
        }
    }
    
    /// <summary>괴물 맵 이동스킬</summary>
    public void SkillTeleportationMap()
    {
        Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int buttonNumber = System.Int32.Parse(clickedButton.GetComponentInChildren<Text>().text);
        if (ItemSpawner.lightTrapList.Count < buttonNumber)
        {
            Debug.Log($"{buttonNumber}번째 LightTrap이 없습니다");
            return;
        }
        else
        {
            Vector3 target = Vector3.zero;
            try
            {
                target = ItemSpawner.lightTrapList[buttonNumber - 1].trap.transform.position;
            }
            catch
            {
                //temp 수정해야함
                //target = GameManager.instance.drone.transform.position;
            }
            ClientSend.SkillTeleportation(target);
        }
    }

    /// <summary>괴물 공격 성공시 스킬 비활성화</summary>
    public void MonsterSkillUIControll(float seconds)
    {
        for(int i = 0; i < coolTimeBackGroundImage.Length; i++)
        {
            coolTimeBackGroundImage[i].gameObject.SetActive(true);
            coolTimeText[i].gameObject.SetActive(true);

            coolTimeText[i].text = string.Format("{0:F0}", seconds);

            if(seconds <= 0)
            {
                coolTimeBackGroundImage[i].gameObject.SetActive(false);
                coolTimeText[i].gameObject.SetActive(false);
                GameManager.players[Client.instance.myId].isMonsterAttack = false;
            }
        }

    }
}
