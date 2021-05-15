using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public bool getKeyDownF = false;
    public bool getKeyDownE = false;
    public KeyCode[] input = { KeyCode.W , KeyCode.S , KeyCode.A , KeyCode.D , KeyCode.Space };

    private IEnumerator lightTrapInstall;

    private bool isInEMPZone = false;
    private bool isInHideZone = false;

    private void Start()
    {
        input[0] = KeyCode.W;
        input[1] = KeyCode.S;
        input[2] = KeyCode.A;
        input[3] = KeyCode.D;
        input[4] = KeyCode.Space;
    }

    private IEnumerator WaitForMilliSec()
    {
        yield return new WaitForSeconds(0.1f);
        if (getKeyDownF) {
            Debug.Log($"주위에 아무것도 없습니다");
            getKeyDownF = false;
        }
        if (getKeyDownE) {
            Debug.Log($"주위에 아무것도 없습니다");
            getKeyDownE = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(this.GetComponent<PlayerManager>().playerType == PlayerType.HUMAN)
            {
                ItemSpawner _grabItem = this.GetComponent<PlayerManager>().playerItem.GrabItem;

                if (_grabItem != null && _grabItem.itemType == ItemType.GUN)
                {
                    Gun gun = _grabItem.GetComponent<Gun>();
                    if (gun.currentBattery != 0)
                    {
                        ClientSend.PlayerShootBullet(camTransform.forward);
                    }                  
                }
            }
            else
            {
                Debug.Log("몬스터 공격!");
                ClientSend.MonsterAttack(camTransform.forward);
            }
            
                                   
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ItemSpawner _grabItem = this.GetComponent<PlayerManager>().playerItem.GrabItem;

            if (_grabItem != null)
            {
                if (this.GetComponent<PlayerManager>().playerType == PlayerType.HUMAN && _grabItem.itemType == ItemType.GUN)
                {
                    Gun gun = _grabItem.GetComponent<Gun>();
                    if (gun.currentBattery >= 5)
                    {
                        ClientSend.PlayerShootBomb(camTransform.forward);
                    }
                }
            }            
        }
        //상호작용(아이템획득, 문열기, 은폐 등
        if (Input.GetKeyDown(KeyCode.F))
        {
            getKeyDownF = true;
            StartCoroutine(WaitForMilliSec());
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (this.GetComponent<PlayerManager>().playerItem.GrabItem != null)
            {
                ClientSend.PlayerThrowItem(this.GetComponent<PlayerManager>().playerItem.GrabItem, transform.position);
            }
            else
            {
                Debug.Log($"들고있는 아이템이 없습니다");
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            UIManager.instance.mapActive = !UIManager.instance.mapActive;
            UIManager.instance.map.SetActive(UIManager.instance.mapActive);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            KeyChange();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            getKeyDownE = true;
            StartCoroutine(WaitForMilliSec());
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameManager.players[Client.instance.myId].isCuring = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (this.GetComponent<PlayerManager>().playerItem.item_number1 != null)
            {
                int _grabItemId = (this.GetComponent<PlayerManager>().playerItem.GrabItem != null) ? this.GetComponent<PlayerManager>().playerItem.GrabItem.spawnerId : -1;
                ClientSend.PlayerGrabItem(_grabItemId, this.GetComponent<PlayerManager>().playerItem.item_number1.spawnerId, 1);


            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (this.GetComponent<PlayerManager>().playerItem.item_number2.Count > 0)
            {
                int _grabItemId = (this.GetComponent<PlayerManager>().playerItem.GrabItem != null) ? this.GetComponent<PlayerManager>().playerItem.GrabItem.spawnerId : -1;
                ClientSend.PlayerGrabItem(_grabItemId, this.GetComponent<PlayerManager>().playerItem.item_number2[0].spawnerId, 2);
            }
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            ItemSpawner _grabItem = this.GetComponent<PlayerManager>().playerItem.GrabItem;
            switch (this.GetComponent<PlayerManager>().playerType)
            {
                case PlayerType.HUMAN:
                    if (_grabItem != null && _grabItem.itemType == ItemType.GUN)
                    {
                        Debug.Log($"장전!");
                        _grabItem.GetComponent<Gun>().Reloade();
                    }
                    else
                    {
                        Debug.Log($"총을 들어주세요");
                    }
                    break;
                case PlayerType.MONSTER:
                    if (_grabItem != null && _grabItem.itemType == ItemType.DRONE && !this.GetComponent<PlayerManager>().isMonsterAttack)
                    {                          
                        ClientSend.SkillDrone(_grabItem.spawnerId);
                    }
                    else
                    {
                        Debug.Log($"드론을 들어주세요");
                    }
                    break;
                default:
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(this.GetComponent<PlayerManager>().playerType == PlayerType.MONSTER && !this.GetComponent<PlayerManager>().isMonsterAttack)
            {
                ClientSend.SpeedUp(GameManager.players[Client.instance.myId]);
            }
        }
    }

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(input[0]),
            Input.GetKey(input[1]),
            Input.GetKey(input[2]),
            Input.GetKey(input[3]),
            Input.GetKey(input[4])
        };

        ClientSend.PlayerMovement(_inputs);
    }

    public void KeyChange()
    {
        KeyCode[] changeInput = new KeyCode[input.Length];
        int[] index = new int[input.Length];
        int[] random = new int[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            index[i] = i;
        }
        for (int i = input.Length; i > 0; i--)
        {
            int value = Random.Range(0, i - 1);
            random[i - 1] = index[value];
            index[value] = index[i - 1];
        }
        for (int i = 0; i < input.Length; i++)
        {
            changeInput[i] = input[random[i]];
        }
        input = changeInput;


        UIManager.instance.monsterKey[0].text = "전진 :" + input[0].ToString();
        UIManager.instance.monsterKey[1].text = "왼쪽 :" + input[1].ToString();
        UIManager.instance.monsterKey[2].text = "뒤로 :" + input[2].ToString();
        UIManager.instance.monsterKey[3].text = "오른쪽 :" + input[3].ToString();
        UIManager.instance.monsterKey[4].text = "점프 :" + input[4].ToString();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EMPZONE"))
        {
            isInEMPZone = true;
            Debug.Log($"isINEMPZone : {isInEMPZone}");
        }    
        if(other.CompareTag("HIDEZONE"))
        {
            isInHideZone = true;
            Debug.Log($"isInHideZone : {isInHideZone}");
        }
    }

    public void OnTriggerStay(Collider other)
    {
        Debug.Log($"현재 콜라이더 : {other.name}");
        if (getKeyDownF)
        {
            getKeyDownF = false;
            //문열기
            if (other.CompareTag("Door"))
            {
                if (other.gameObject.transform.rotation.y == 0f)
                {
                    other.gameObject.transform.RotateAround(other.gameObject.transform.GetChild(0).position, Vector3.up, -90f);
                }
                else
                {
                    other.gameObject.transform.RotateAround(other.gameObject.transform.GetChild(0).position, Vector3.up, 90f);
                }
            }
            //은폐
            if (isInHideZone)
            {
                ClientSend.Hide(other.gameObject);
            }
            //아이템획득
            if (other.CompareTag("Item"))
            {
                if (other.GetComponent<ItemSpawner>().hasItem) 
                {
                    switch (other.GetComponent<ItemSpawner>().itemType)
                    {
                        case ItemType.GUN: case ItemType.DRONE:
                            if (this.GetComponent<PlayerManager>().playerItem.item_number1 == null)
                            {
                                Debug.Log($"아이템 줍기");
                                ClientSend.PlayerGetItem(other.gameObject);
                            }
                            else
                            {
                                Debug.Log($"해당 아이템은 1개만 획득할 수 있습니다");
                            }
                            break;
                        default:
                            Debug.Log("아이템 줍기");
                            ClientSend.PlayerGetItem(other.gameObject);
                            break;
                    }
                }
                else
                {
                    Debug.Log($"주위에 아무것도 없습니다");
                }
            }
            else
            {
                Debug.Log($"주위에 아무것도 없습니다");
            }
        }

        if (getKeyDownE)
        {
            Debug.Log($"현재 콜라이더 : {other.name}");
            getKeyDownE = false;
            ItemSpawner _grabItem = this.GetComponent<PlayerManager>().playerItem.GrabItem;
            if (_grabItem != null) {
                if (_grabItem.itemType == ItemType.EMP && this.GetComponent<PlayerManager>().playerType == PlayerType.HUMAN)
                {
                    EMP emp = _grabItem.GetComponent<EMP>();

                    if (isInEMPZone)
                    {                        
                        Debug.Log($"지금은 : {other.gameObject.name}");
                        if (emp.isInstalling)
                        {
                            Debug.Log($"emp 설치 취소");
                            emp.InstallCancle();
                        }
                        else
                        {
                            emp.isInstalling = true;
                            emp.gaugeCheck = true;
                        }
                    }                   
                    else
                    {
                        int _floor = (this.transform.position.y < 10f) ? 1 : 2;
                        ClientSend.Install(this.transform.position, _grabItem.spawnerId, _floor);
                    }
                }
                else if (_grabItem.itemType == ItemType.LIGHTTRAP && this.GetComponent<PlayerManager>().playerType == PlayerType.MONSTER)
                {
                    if(!this.GetComponent<PlayerManager>().isMonsterAttack)
                    {
                        int _floor = (this.transform.position.y < 10f) ? 1 : 2;
                        ClientSend.Install(this.transform.position, _grabItem.spawnerId, _floor);
                    }                    
                }
                else
                {
                    Debug.Log($"설치할 수 없는 아이템입니다");
                }
            }
            else
            {
                Debug.Log($"아이템을 들고있지 않습니다");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("EMPZONE"))
        {
            isInEMPZone = false;
            Debug.Log($"isINEMPZone : {isInEMPZone}");
        }
        if(other.CompareTag("HIDEZONE"))
        {
            isInHideZone = false;
            Debug.Log($"isInHideZone : {isInHideZone}");
        }
    }
    /*
    /// <summary>괴물 Light Trap 설치</summary>
    /// <returns></returns>
    private IEnumerator InstallLightTrap(int _spawnerId)
    {
        //animation실행 (1초간 설치)
        yield return new WaitForSeconds(1f);

        ClientSend.InstallLightTrap(this.transform.position, _spawnerId);
    }*/
}