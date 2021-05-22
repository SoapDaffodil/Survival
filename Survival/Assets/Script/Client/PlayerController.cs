using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public bool getKeyDownF = false;
    public bool getKeyDownE = false;
    public KeyCode[] input = { KeyCode.W , KeyCode.S , KeyCode.A , KeyCode.D , KeyCode.Space, KeyCode.LeftShift, KeyCode.LeftControl };

    private IEnumerator lightTrapInstall;

    public bool isInEMPZone = false;
    private bool isInHideZone = false;

    public float fireRate = 3f;
    public float nextTimeToFire;

    private void Start()
    {
        input = new KeyCode[8];
        input[0] = KeyCode.W;
        input[1] = KeyCode.S;
        input[2] = KeyCode.A;
        input[3] = KeyCode.D;
        input[4] = KeyCode.Space;
        input[5] = KeyCode.LeftShift;
        input[6] = KeyCode.LeftControl;
        input[7] = KeyCode.Mouse0;
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
        if(Input.GetKey(KeyCode.Mouse0))
        {
            if (this.GetComponent<PlayerManager>().playerType == PlayerType.HUMAN)
            {
                ItemSpawner _grabItem = this.GetComponent<PlayerManager>().playerItem.GrabItem;

                if (_grabItem != null && _grabItem.itemType == ItemType.GUN && GameManager.EMPInstallFinished)
                {
                    Gun gun = _grabItem.GetComponent<Gun>();
                    if (gun.currentBattery != 0 && Time.time >= nextTimeToFire)
                    {
                        nextTimeToFire = Time.time + 1 / fireRate;
                        gun.GetComponent<AudioSource>().PlayOneShot(gun.normalGunSound);
                        ClientSend.PlayerShootBullet(camTransform.forward);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            /*
            if(this.GetComponent<PlayerManager>().playerType == PlayerType.HUMAN)
            {
                ItemSpawner _grabItem = this.GetComponent<PlayerManager>().playerItem.GrabItem;

                if (_grabItem != null && _grabItem.itemType == ItemType.GUN)
                {
                    Gun gun = _grabItem.GetComponent<Gun>();
                    if (gun.currentBattery != 0)
                    {
                        gun.normalGunSound.PlayOneShot(gun.normalGunSound.clip);
                        ClientSend.PlayerShootBullet(camTransform.forward);
                    }                  
                }
            }
            */
            if(this.GetComponent<PlayerManager>().playerType == PlayerType.CREATURE)
            {
                this.GetComponent<PlayerManager>().GetComponent<AudioSource>().PlayOneShot(this.GetComponent<PlayerManager>().creatureAttackSound);
                ClientSend.CreatureAttack(camTransform.forward);
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
                        gun.GetComponent<AudioSource>().PlayOneShot(gun.empGunSound);
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
            Cursor.visible = UIManager.instance.mapActive;
        }
        /*
        if (Input.GetKeyDown(KeyCode.U))
        {
            KeyChange();
        }*/
        if (Input.GetKeyDown(KeyCode.E))
        {
            //getKeyDownE = true;
            if(this.GetComponent<PlayerManager>().playerItem.GrabItem != null)
            {
                if(this.GetComponent<PlayerManager>().playerItem.GrabItem.itemType == ItemType.EMP || this.GetComponent<PlayerManager>().playerItem.GrabItem.itemType == ItemType.LIGHTTRAP)
                {
                    this.GetComponent<PlayerManager>().playerItem.GrabItem.GetComponent<Collider>().enabled = true;
                }
            }
            ItemSpawner _grabItem = this.GetComponent<PlayerManager>().playerItem.GrabItem;
            if (_grabItem != null)
            {
                if (_grabItem.itemType == ItemType.EMP && this.GetComponent<PlayerManager>().playerType == PlayerType.HUMAN)
                {
                    EMP emp = _grabItem.GetComponent<EMP>();

                    if (isInEMPZone)
                    {
                        emp.chargingSpeed = 15f;
                        if (emp.isInstalling)
                        {
                            emp.InstallCancle();
                            this.GetComponent<PlayerManager>().isInstalling = false;
                            this.GetComponent<PlayerManager>().PlayerInstallingSound(this.GetComponent<PlayerManager>().isInstalling);
                        }
                        else
                        {
                            emp.isInstalling = true;
                            emp.gaugeCheck = true;
                            this.GetComponent<PlayerManager>().isInstalling = true;
                            this.GetComponent<PlayerManager>().PlayerInstallingSound(this.GetComponent<PlayerManager>().isInstalling);
                        }
                    }
                    else
                    {
                        if (GameManager.instance.trapCount < GameManager.instance.maxTrapCount && this.GetComponent<PlayerManager>().id == Client.instance.myId)
                        {
                            emp.chargingSpeed = 40f;
                            emp.isInstalling = true;
                            emp.gaugeCheck = true;
                            this.GetComponent<PlayerManager>().isInstalling = true;
                            this.GetComponent<PlayerManager>().PlayerInstallingSound(this.GetComponent<PlayerManager>().isInstalling);
                        }
                        else
                        {
                            Debug.Log("설치 가능한 트랩을 모두 설치했습니다");
                        }

                    }
                }
                else if (_grabItem.itemType == ItemType.LIGHTTRAP && this.GetComponent<PlayerManager>().playerType == PlayerType.CREATURE)
                {

                    if (!this.GetComponent<PlayerManager>().isCreatureAttack)
                    {
                        if (this.GetComponent<PlayerManager>().id == Client.instance.myId)
                        {
                            if (GameManager.instance.trapCount < GameManager.instance.maxTrapCount)
                            {
                                int _floor = (this.transform.position.y < 8f) ? 1 : 2;
                                this.GetComponent<PlayerManager>().isInstalling = true;
                                this.GetComponent<PlayerManager>().PlayerInstallingSound(this.GetComponent<PlayerManager>().isInstalling);
                                ClientSend.Install(this.transform.position, _grabItem.spawnerId, _floor);
                            }
                            else
                            {
                                Debug.Log($"설치 가능한 트랩을 모두 설치했습니다");
                            }

                        }
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
            //StartCoroutine(WaitForMilliSec());
        }
        if (Input.GetKeyDown(KeyCode.C) && this.GetComponent<PlayerManager>().playerType == PlayerType.HUMAN)
        {
            if(this.GetComponent<PlayerManager>().hp < this.GetComponent<PlayerManager>().maxHp)
            {
                GameManager.players[Client.instance.myId].isCuring = true;
                ClientSend.SkillCure(true);
            }
            else
            {
                Debug.Log("체력이 가득 차 있습니다!");
            }
            
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
                        _grabItem.GetComponent<Gun>().Reloade();
                    }
                    else
                    {
                        Debug.Log($"총을 들어주세요");
                    }
                    break;
                case PlayerType.CREATURE:
                    if (_grabItem != null && _grabItem.itemType == ItemType.DRONE && !this.GetComponent<PlayerManager>().isCreatureAttack)
                    {
                        if(_grabItem.transform.parent == null)
                        {
                            _grabItem.transform.SetParent(gameObject.transform);
                        }
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
        if (Input.GetKeyDown(KeyCode.Q) && !this.GetComponent<PlayerManager>().isCreatureSpeedUp)
        {
            if(this.GetComponent<PlayerManager>().playerType == PlayerType.CREATURE && !this.GetComponent<PlayerManager>().isCreatureAttack)
            {
                ClientSend.SpeedUp(GameManager.players[Client.instance.myId]);
                this.GetComponent<PlayerManager>().isCreatureSpeedUp = true;
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
        bool[] _inputs = new bool[input.Length];
        for(int i=0;i< _inputs.Length;i++)
        {
            _inputs[i] = Input.GetKey(input[i]);
        };

        ClientSend.PlayerMovement(_inputs);
    }

    public void KeyChange()
    {
        int changeSize = 4;
        KeyCode[] changeInput = new KeyCode[input.Length];
        int[] index = new int[input.Length];
        int[] random = new int[input.Length];
        for (int i = 0; i < changeSize; i++)
        {
            index[i] = i;
        }
        for (int i = changeSize; i > 0; i--)
        {
            int value = Random.Range(0, i - 1);
            random[i - 1] = index[value];
            index[value] = index[i - 1];
        }
        for (int i = 0; i < changeSize; i++)
        {
            changeInput[i] = input[random[i]];
            UIManager.instance.creatureKey[i].text = changeInput[i].ToString();
            if (UIManager.instance.creatureKey[i].text.Length > 3)
            { 
                UIManager.instance.creatureKey[i].text = "SP"; 
            }
        }
        for (int i = changeSize; i < input.Length; i++)
        {
            changeInput[i] = input[i];
        }
        input = changeInput;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EMPZONE"))
        {
            isInEMPZone = true;
        }    
        if(other.CompareTag("HIDEZONE"))
        {
            isInHideZone = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
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
                                ClientSend.PlayerGetItem(other.gameObject);
                            }
                            else
                            {
                                Debug.Log($"해당 아이템은 1개만 획득할 수 있습니다");
                            }
                            break;
                        default:
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

        /*if (getKeyDownE)
        {
            getKeyDownE = false;
            ItemSpawner _grabItem = this.GetComponent<PlayerManager>().playerItem.GrabItem;
            if (_grabItem != null) {
                if (_grabItem.itemType == ItemType.EMP && this.GetComponent<PlayerManager>().playerType == PlayerType.HUMAN)
                {
                    EMP emp = _grabItem.GetComponent<EMP>();

                    if (isInEMPZone)
                    {
                        emp.chargingSpeed = 15f;
                        if (emp.isInstalling)
                        {
                            emp.InstallCancle();
                            this.GetComponent<PlayerManager>().isInstalling = false;
                            this.GetComponent<PlayerManager>().PlayerInstallingSound(this.GetComponent<PlayerManager>().isInstalling);
                        }
                        else
                        {
                            emp.isInstalling = true;
                            emp.gaugeCheck = true;
                            this.GetComponent<PlayerManager>().isInstalling = true;
                            this.GetComponent<PlayerManager>().PlayerInstallingSound(this.GetComponent<PlayerManager>().isInstalling);
                        }
                    }                   
                    else
                    {
                        if(GameManager.instance.trapCount < GameManager.instance.maxTrapCount && this.GetComponent<PlayerManager>().id == Client.instance.myId)
                        {
                            emp.chargingSpeed = 40f;
                            emp.isInstalling = true;
                            emp.gaugeCheck = true;
                            this.GetComponent<PlayerManager>().isInstalling = true;
                            this.GetComponent<PlayerManager>().PlayerInstallingSound(this.GetComponent<PlayerManager>().isInstalling);
                        }
                        else
                        {
                            Debug.Log("설치 가능한 트랩을 모두 설치했습니다");
                        }
                        
                    }
                }
                else if (_grabItem.itemType == ItemType.LIGHTTRAP && this.GetComponent<PlayerManager>().playerType == PlayerType.CREATURE)
                {
                    
                    if(!this.GetComponent<PlayerManager>().isCreatureAttack)
                    {
                        if (this.GetComponent<PlayerManager>().id == Client.instance.myId)
                        {
                            if (GameManager.instance.trapCount < GameManager.instance.maxTrapCount)
                            {
                                int _floor = (this.transform.position.y < 10f) ? 1 : 2;
                                this.GetComponent<PlayerManager>().isInstalling = true;
                                this.GetComponent<PlayerManager>().PlayerInstallingSound(this.GetComponent<PlayerManager>().isInstalling);
                                ClientSend.Install(this.transform.position, _grabItem.spawnerId, _floor);
                            }
                            else
                            {
                                Debug.Log($"설치 가능한 트랩을 모두 설치했습니다");
                            }

                        }
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
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("EMPZONE"))
        {
            isInEMPZone = false;
        }
        if(other.CompareTag("HIDEZONE"))
        {
            isInHideZone = false;
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