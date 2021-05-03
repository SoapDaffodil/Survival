﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public bool getKeyDownF = false;
    public bool getKeyDownE = false;
    public KeyCode[] input = { KeyCode.W , KeyCode.S , KeyCode.A , KeyCode.D , KeyCode.Space };

    private IEnumerator lightTrapInstall;

    private void Start()
    {
        input[0] = KeyCode.W;
        input[1] = KeyCode.S;
        input[2] = KeyCode.A;
        input[3] = KeyCode.D;
        input[4] = KeyCode.Space;
    }

    private IEnumerator WaitForGetItem()
    {
        yield return new WaitForSeconds(0.1f);
        if (getKeyDownF) {
            Debug.Log($"주위에 아무것도 없습니다");
            getKeyDownF = false;
        }
    }

    private void Update()
    {
        //총 발사
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ClientSend.PlayerShootBullet(camTransform.forward);
        }
        //탄 발사
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ClientSend.PlayerShootBomb(camTransform.forward);
        }
        //상호작용(아이템획득, 문열기, 은폐 등
        if (Input.GetKeyDown(KeyCode.F))
        {
            getKeyDownF = true;
            StartCoroutine(WaitForGetItem());
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (GetComponent<PlayerManager>().grabItem != null)
            {
                ClientSend.PlayerThrowItem(GetComponent<PlayerManager>().grabItem, transform.position);
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
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameManager.players[Client.instance.myId].isCuring = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (this.GetComponent<PlayerManager>().playerItem.item_number1 != null)
            {
                ClientSend.PlayerGrabItem(GetComponent<PlayerManager>().playerItem.item_number1.spawnerId, 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (this.GetComponent<PlayerManager>().playerItem.item_number2.Count > 0)
            {
                ClientSend.PlayerGrabItem(GetComponent<PlayerManager>().playerItem.item_number2[0].spawnerId, 2);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerType == PlayerType.HUMAN)
            {
                Gun gun = GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerItem.item_number1.GetComponent<Gun>();
                

                if (GameManager.players[Client.instance.myId].isOnHand && transform.GetChild(1).gameObject.GetComponent<Gun>())
                {
                    Debug.Log("장전!");
                    gun.state = Gun.State.Empty;
                    gun.Reloade();
                }
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

    public void OnTriggerStay(Collider other)
    {
        if (getKeyDownF)
        {
            getKeyDownF = false;
            //아이템획득
            if (other.CompareTag("Item"))
            {
                /*if (Item.arrayIndex == Item.inventoryBox.Length)
                {
                    Debug.Log("아이템 창이 가득 찼습니다");
                }
                else
                {
                    ClientSend.PlayerGetItem(other.gameObject);
                }*/
                ClientSend.PlayerGetItem(other.gameObject);
            }
            //문열기
            else if (other.CompareTag("Door"))
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
            else if (other.CompareTag("Hide"))
            {
                ClientSend.Hide(other.gameObject);
            }
            else
            {
                Debug.Log($"주위에 아무것도 없습니다");
            }
        }

        if (getKeyDownE)
        {
            getKeyDownE = false;
            ItemSpawner grabItem = GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().grabItem;
            if (grabItem != null && (
                grabItem.itemType == ItemType.EMP || grabItem.itemType == ItemType.LIGHTTRAP)
                )
            {
                int _floor = (this.transform.position.y < 10f) ? 1 : 2;
                ClientSend.Install(this.transform.position, grabItem.spawnerId, _floor);
            }
            /*
            if (GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerType == PlayerType.HUMAN)
            {
                if (grabItem != null && ((ItemSpawner)grabItem).itemType == ItemType.EMP)
                {
                    EMP emp = grabItem.GetComponent<EMP>();

                    if (other.CompareTag("EMPZONE"))
                    {
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
                        //EMP TRAP 설치
                        if (emp.isInstalling)
                        {
                            emp.InstallCancle();
                        }
                        else
                        {
                            //emp.Install();
                            emp.isDetectiveMode = true;
                            emp.isInstalling = true;
                            emp.gaugeCheck = true;
                        }
                    }
                }
                else
                {
                    Debug.Log("가지고 있는 emp가 없습니다");
                }
            }
            else
            {
                if (grabItem != null && ((ItemSpawner)grabItem).itemType == ItemType.LIGHTTRAP)
                {
                    //괴물 LIGHT TRAP 설치
                    lightTrapInstall = InstallLightTrap(((ItemSpawner)grabItem).spawnerId);
                    StartCoroutine(lightTrapInstall);
                }
            }*/
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