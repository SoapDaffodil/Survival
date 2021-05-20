using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { GUN, DRONE, EMP, LIGHTTRAP, BATTERY }
public class PlayerItem
{
    private ItemSpawner grabitem;
    public ItemSpawner item_number1;
    public List<ItemSpawner> item_number2;
    public int batteryCount;                //남은탄창(HUMAN만 쓰임)
    public PlayerItem()
    {
        grabitem = null;
        item_number1 = null;
        item_number2 = new List<ItemSpawner>();
        batteryCount = 0;
    }
    public ItemSpawner GrabItem
    {
        get { return grabitem; }
        set { grabitem = value; }
    }
}
public class ItemSpawner : MonoBehaviour
{
    public int spawnerId;                   //아이템ID
    public bool hasItem;                    //아이템존재여부
    public MeshRenderer itemModel;          //아이템 mesh

    public float itemRotationSpeed = 50f;   //아이템 회전속도
    public float itemBobSpeed = 2f;         //아이템 이동속도
    private Vector3 basePosition;           //아이템 기본포지션

    public ItemType itemType;               //이 아이템이 무엇인가

    /// <summary>Trap 정보</summary>
    public struct TrapInfo
    {
        public int floor;
        public ItemSpawner trap;
        public TrapInfo(int _f, ItemSpawner _trap)
        {
            floor = _f;
            trap = _trap;
        }
        public bool Compare(ItemSpawner _trap)
        {
            return (_trap == trap);
        }
    }
    public static List<TrapInfo> empTrapList = new List<TrapInfo>();
    public static List<TrapInfo> lightTrapList = new List<TrapInfo>();

    public void Start()
    {
        itemModel = this.GetComponent<MeshRenderer>();
        try
        {
            itemType = (ItemType)ItemType.Parse(typeof(ItemType), this.name);
        }
        catch(System.Exception ex)
        {
            itemType = (ItemType)ItemType.Parse(typeof(ItemType), this.name.Remove(this.name.Length - "(Clone)".Length, "(Clone)".Length));
        }
    }
    /*
    private void Update()
    {
        //아이템이 존재하면 회전, 이동
        if (hasItem)
        {
            transform.Rotate(Vector3.up, itemRotationSpeed * Time.deltaTime, Space.World);
            transform.position = basePosition + new Vector3(0f, 0.25f * Mathf.Sin(Time.time * itemBobSpeed), 0f);
        }
    }*/

    /// <summary>아이템초기화</summary>
    /// <param name="_spawnerId">아이템ID</param>
    /// <param name="_hasItem">아이템 존재여부</param>
    public void Initialize(int _spawnerId, bool _hasItem, bool _modelEnabled)
    {
        spawnerId = _spawnerId;
        hasItem = _hasItem;
        itemModel.enabled = _modelEnabled;

        basePosition = transform.position;
    }

    /// <summary>아이템 생성됨 > 맵에 표시</summary>
    public void ItemSpawned()
    {
        hasItem = true;
        itemModel.enabled = true;
    }

    /// <summary>아이템 획득 > 맵에 표시X</summary>
    public void ItemPickedUp()
    {
        hasItem = false;
        itemModel.enabled = false;
    }

    /// <summary>아이템 버리기 > 맵에 표시O</summary>
    public void ItemThrow(Vector3 _position)
    {
        if (transform.parent != null)
        {
            transform.parent.GetComponent<PlayerManager>().playerItem.GrabItem.GetComponent<Collider>().enabled = true;
            transform.parent.GetComponent<PlayerManager>().playerItem.GrabItem = null;
            switch (itemType)
            {
                case ItemType.GUN: case ItemType.DRONE:
                    transform.parent.GetComponent<PlayerManager>().playerItem.item_number1 = null;
                    transform.parent.GetComponent<PlayerManager>().animator.SetBool("Gun", false);
                    UIManager.instance.itemImageUI[0].sprite = UIManager.instance.itemGrayImage
                        [(int)GameManager.players[Client.instance.myId].playerType * UIManager.instance.itemImageUI.Length];
                    UIManager.instance.itemCountText[0].text = "0";
                    UIManager.instance.itemCountText[0].color = UIManager.instance.textColor[(int)UIManager.TextColor.GRAY];
                    break;
                case ItemType.EMP: case ItemType.LIGHTTRAP:
                    transform.parent.GetComponent<PlayerManager>().playerItem.item_number2.Remove(this);
                    UIManager.instance.itemCountText[1].text = transform.parent.GetComponent<PlayerManager>().playerItem.item_number2.Count.ToString();
                    if(transform.parent.GetComponent<PlayerManager>().playerItem.item_number2.Count == 0)
                    {
                        UIManager.instance.itemImageUI[1].sprite = UIManager.instance.itemGrayImage
                            [(int)GameManager.players[Client.instance.myId].playerType * UIManager.instance.itemImageUI.Length + 1];
                        UIManager.instance.itemCountText[1].color = UIManager.instance.textColor[(int)UIManager.TextColor.GRAY];
                    }
                    
                    break;
            }
            transform.SetParent(null, true);
            transform.position = _position;
            hasItem = true;
            itemModel.enabled = true;
        }
        else
        {
            Debug.Log($"아이템을 버릴 수 없습니다\nError - 부모오브젝트 {transform.parent}");
        }
    }

    /// <summary>GrabItem 해제</summary>
    public void ItemReleaseGrab()
    {
        transform.SetParent(null, true);
        itemModel.enabled = false;
    }

    /// <summary>아이템 들기</summary>
    public void ItemGrab(PlayerManager _byPlayer, Vector3 _position)
    {
        itemModel.enabled = true;
        transform.position = _position;
        if (transform.parent != _byPlayer.transform) {
            transform.SetParent(_byPlayer.transform, true);
            gameObject.GetComponent<Collider>().enabled = false;
        }
        if (_byPlayer.playerItem.GrabItem != null && _byPlayer.playerItem.GrabItem.transform.parent == _byPlayer.transform)
        {
            _byPlayer.playerItem.GrabItem.ItemReleaseGrab();
        }
        _byPlayer.playerItem.GrabItem = this;

        if(_byPlayer.playerItem.GrabItem.itemType == ItemType.GUN)
        {
            UIManager.instance.currentBulletText.text = "0";
            UIManager.instance.bulletAmoutText.text = "0";
            _byPlayer.animator.SetBool("Gun", true);
        }
        else if (_byPlayer.playerType == PlayerType.HUMAN)
        {
            _byPlayer.animator.SetBool("Gun", false);
        }
    }

    /// <summary>EMPZONE에 EMP 설치하기 > 맵에 표시O</summary>
    public void InstallEMP(Vector3 _position)
    {
        ItemThrow(_position);
        hasItem = false;
        Debug.Log($"EMP Zone 설치완료");
        Debug.Log($"empCount : {GameManager.instance.empCount}");

        if (GameManager.instance.empCount == 2)
        {
            GameManager.instance.CreatureColorHpBar();
        }
    }

    /// <summary>Install Item 표시</summary>
    /// <param name="_installPlayer">아이템을 설치한 player</param>
    /// <param name="_position">아이템 설치 위치</param>
    /// <param name="_position">아이템 설치 층</param>
    public void InstallTrap(Vector3 _position, int _floor)
    {
        ItemThrow(_position);
        hasItem = false;
        gameObject.GetComponent<BoxCollider>().size = new Vector3(0.015f, 0.015f, 0.015f);
        gameObject.GetComponent<BoxCollider>().isTrigger = true;

        switch (this.itemType)
        {
            case ItemType.EMP:
                GameManager.instance.AddEMPTrap(_floor, this);
                this.gameObject.GetComponent<EMP>().isDetectiveMode = true;
                Debug.Log($"EMPTrap 설치완료");
                break;
            case ItemType.LIGHTTRAP:
                GameManager.instance.AddLightTrap(_floor, this);
                this.gameObject.GetComponent<LightTrap>().isDetectiveMode = true;
                this.gameObject.GetComponent<LightTrap>().trapId = this.gameObject.GetComponent<ItemSpawner>().spawnerId;
                Debug.Log($"LightTrap 설치완료");
                break;
            default:
                Debug.Log("트랩 아이템이 아닙니다");
                break;
        }
        /*
        itemModel.enabled = true;
        this.transform.position = _position;
        switch (this.itemType)
        {
            case ItemType.EMP:
                _installPlayer.playerItem.GrabItem = null;
                _installPlayer.playerItem.item_number2.Remove(this);
                itemModel.enabled = true;
                transform.position = _position;
                if (transform.parent != null)
                {
                    transform.SetParent(null, true);
                }
                break;
            case ItemType.LIGHTTRAP:
                _installPlayer.playerItem.GrabItem = null;
                _installPlayer.playerItem.item_number2.Remove(this);
                GameManager.instance.AddLightTrap(_floor, this);
                itemModel.enabled = true;
                transform.position = _position;
                if (transform.parent != null)
                {
                    transform.SetParent(null, true);
                }
                break;
            default:
                Debug.Log("설치가능한 아이템이 아닙니다");
                break;
        }*/
    }
}