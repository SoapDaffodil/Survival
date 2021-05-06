using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    /// <summary>아이템을 저장할 Dictionary</summary>
    public static Dictionary<int, ItemSpawner> spawners = new Dictionary<int, ItemSpawner>();
    public struct TrapInfo
    {
        public int floor;
        public ItemSpawner trap;
        public TrapInfo(int _f, ItemSpawner _trap)
        {
            floor = _f;
            trap = _trap;
        }
    }
    public static List<TrapInfo> empTrapList = new List<TrapInfo>();
    public static List<TrapInfo> lightTrapList = new List<TrapInfo>();

    private static int nextSpawnerId = 1;   //다음으로 스폰될 아이템의 ID

    public int spawnerId;                   //스폰되는 아이템의 ID
    public bool hasItem = false;            //아이템이 존재하는지 체크
    public MeshRenderer itemModel;          //아이템 mesh



    public void Initialize()
    {
        itemModel = GetComponent<MeshRenderer>();
        hasItem = false;
        itemModel.enabled = false;
        spawnerId = nextSpawnerId;
        nextSpawnerId++;
        spawners.Add(spawnerId, this);

        StartCoroutine(SpawnItem());
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        //아이템이 존재하고 아이템의 트리거범위에 player가 들어오면 아이템을 획득함
        if (hasItem && other.CompareTag("Player"))
        {
            Player _player = other.GetComponent<Player>();
            if (_player.AttemptPickupItem())
            {
                ItemPickedUp(_player.id);
            }
        }
    }
    */

    /// <summary>3초뒤에 아이템생성하고 클라이언트들에게 아이템스폰정보 전송</summary>
    /// <returns></returns>
    private IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(3f);

        hasItem = true;
        itemModel.enabled = true;
        ServerSend.ItemSpawned(spawnerId, transform.position, this.tag);
    }

    /// <summary>아이템을 먹었다는 정보를 클라이언트에게 전송</summary>
    /// <param name="_byPlayer">아이템을 획득한 플레이어</param>
    public void ItemPickedUp(Player _byPlayer)
    {
        if (_byPlayer.AttemptPickupItem())
        {
            switch (_byPlayer.playerType)
            {
                case PlayerType.HUMAN:
                    if (this.tag == "GUN" || this.tag == "EMP" || this.tag == "BATTERY")
                    {
                        hasItem = false;
                        itemModel.enabled = false;
                        ServerSend.ItemPickedUp(spawnerId, _byPlayer.id);
                    }
                    else
                    {
                        ServerSend.Error(_byPlayer.id, "It is not your Item");
                    }
                    break;

                case PlayerType.MONSTER:
                    if (this.tag == "DRONE" || this.tag == "LIGHTTRAP")
                    {
                        hasItem = false;
                        itemModel.enabled = false;
                        ServerSend.ItemPickedUp(spawnerId, _byPlayer.id);
                    }
                    else
                    {
                        ServerSend.Error(_byPlayer.id, "It is not your Item");
                    }
                    break;
            }
            //StartCoroutine(SpawnItem());
        }
    }

    /// <summary>아이템을 버렸다는 정보를 클라이언트에게 전송</summary>
    /// <param name="_byPlayer">아이템을 획득한 플레이어</param>
    /// <param name="_position">아이템을 버릴 위치</param>
    public void ItemThrow(Player _byPlayer, Vector3 _position)
    {
        hasItem = true;
        itemModel.enabled = true;
        this.transform.position = _position;
        if (this.transform.parent != null)
        {
            this.transform.SetParent(null, true);
        }
        ServerSend.ItemThrow(spawnerId, _byPlayer.id, _position);
    }

    /// <summary>GrabItem 해제</summary>
    public void ItemReleaseGrab()
    {
        transform.SetParent(null, true);
        itemModel.enabled = false;
    }

    /// <summary>아이템을 들었다는 정보를 클라이언트에게 전송</summary>
    /// <param name="_byPlayer">아이템을 들 플레이어</param>
    /// <param name="_key">플레이어가 누른 키</param>
    public void ItemGrab(int _grabSpawnerId, Player _byPlayer, int _key)
    {
        if (!itemModel.enabled)
        {
            if (_grabSpawnerId != -1) {
                spawners[_grabSpawnerId].ItemReleaseGrab();
            }

            Vector3 _itemPosition = _byPlayer.transform.position;
            _itemPosition.y = 1f;
            itemModel.enabled = true;
            transform.position = _itemPosition;
            if (transform.parent != _byPlayer.transform)
            {
                transform.SetParent(_byPlayer.transform, true);
            }
            ServerSend.ItemGrab(spawnerId, _byPlayer.id, _itemPosition);
        }
        else
        {
            ServerSend.Error(_byPlayer.id, $"It is already Grab - {this.tag}");
        }
    }
    
    /// <summary>설치정보를 모든 클라이언트에게 전송</summary>
    /// <param name="_byPlayer">설치한 플레이어</param>
    /// <param name="_position">설치한 위치</param>
    public void Install(int _byPlayer, Vector3 _position, int _floor)
    {
        switch (this.tag)
        {
            case "EMP":
                empTrapList.Add(new TrapInfo(_floor, this));
                this.transform.position = _position;
                if (this.transform.parent != null)
                {
                    this.transform.SetParent(null, true);
                }
                ServerSend.InstallTrap(spawnerId, _position, _floor);
                break;
            case "LIGHTTRAP":
                lightTrapList.Add(new TrapInfo(_floor, this));
                itemModel.enabled = true;
                this.transform.position = _position;
                if (this.transform.parent != null)
                {
                    this.transform.SetParent(null, true);
                }
                ServerSend.InstallTrap(spawnerId, _position, _floor);
                break;
            default:
                ServerSend.Error(_byPlayer, $"This item is not for installation - {this.tag}");
                break;
        }
    }

    /// <summary>EMPZONE에 EMP를 설치했다는 정보를 모든 클라이언트에게 전송</summary>
    /// <param name="_byPlayer">EMP를 설치한 플레이어</param>
    /// <param name="_position">EMP를 설치한 위치</param>
    public void InstallEMP(Vector3 _position)
    {
        itemModel.enabled = true;
        this.transform.position = _position;
        ServerSend.InstallEMP(spawnerId, _position);
    }

}
