using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>해당 클라이언트에서 동작하는 것들 관리(server로부터 수신한 data를 토대로 gameobject update)</summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    /// <summary>emp존 설치완료여부 판단</summary>
    public static bool EMPInstallFinished = false;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();            //모든플레이어정보 저장
    public static Dictionary<int, ItemSpawner> itemSpawners = new Dictionary<int, ItemSpawner>();           //모든아이템정보 저장
    public static Dictionary<int, ProjectileManager> projectiles = new Dictionary<int, ProjectileManager>();//모든 폭탄정보 저장
    public static Dictionary<int, BulletManager> bullets = new Dictionary<int, BulletManager>();            //모든 총알 정보 저장

    /// <summary>자신 player 프리팹</summary>
    public GameObject[] localPlayerPrefab;
    /// <summary>다른 player 프리팹</summary>
    public GameObject[] otherPlayerPrefab;
    [Tooltip("[0] : monster, [1] : human 모델링 메쉬")]
    public Mesh[] playerMesh = new Mesh[2];
    /// <summary>아이템 프리팹</summary>
    public GameObject[] itemSpawnerPrefab;
    public Dictionary<ItemType, GameObject> itemSpawnerObject;
    /// <summary>폭탄 프리팹</summary>
    public GameObject projectilePrefab;
    /// <summary>총알 프리팹</summary>
    public GameObject bulletPrefab;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
        itemSpawnerObject = new Dictionary<ItemType, GameObject>() {
            { ItemType.GUN, itemSpawnerPrefab[0] },
            { ItemType.DRONE, itemSpawnerPrefab[1] },
            { ItemType.EMP, itemSpawnerPrefab[2] },
            { ItemType.LIGHTTRAP, itemSpawnerPrefab[3] },
            { ItemType.BATTERY, itemSpawnerPrefab[4] },
        };
        EMPInstallFinished = false;
    }


    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>
    /// <param name="_position">The player's starting position.</param>
    /// <param name="_rotation">The player's starting rotation.</param>
    public void SpawnPlayer(int _id, int _userType, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player = null;
        if (_id == Client.instance.myId)
        {//현재 클라이언트의 플레이어인경우
            switch (_userType)
            {
                case (int)PlayerType.MONSTER:
                    _player = Instantiate(localPlayerPrefab[(int)PlayerType.MONSTER], _position, _rotation);
                    break;
                case (int)PlayerType.HUMAN:
                    _player = Instantiate(localPlayerPrefab[(int)PlayerType.HUMAN], _position, _rotation);
                    break;
            }
            for (int i = 0; i < UIManager.instance.itemImageUI.Length; i++)
            {
                UIManager.instance.itemImageUI[i].sprite = UIManager.instance.itemImage[
                    (int)UIManager.instance.playerType * UIManager.instance.itemImageUI.Length + i];
                UIManager.instance.skillImageUI[i].sprite = UIManager.instance.skillImage[
                    (int)UIManager.instance.playerType * UIManager.instance.itemImageUI.Length + i];
            }
            if (!EMPInstallFinished && UIManager.instance.playerType == PlayerType.MONSTER)
            {
                UIManager.instance.HPBarUI[0].sprite = UIManager.instance.HPBarImage[0];
            }
            else
            {
                UIManager.instance.HPBarUI[0].sprite = UIManager.instance.HPBarImage[1];
            }
        }
        else
        {//다른 크라이언트의 플레이어인경우
            switch (_userType)
            {
                case (int)PlayerType.MONSTER:
                    _player = Instantiate(otherPlayerPrefab[(int)PlayerType.MONSTER], _position, _rotation);
                    break;
                case (int)PlayerType.HUMAN:
                    _player = Instantiate(otherPlayerPrefab[(int)PlayerType.HUMAN], _position, _rotation);
                    break;
            }
        }
        /*
        if (character_human) {
            _player.GetComponent<MeshFilter>().mesh = playerMesh[1];        //모델링 적용시 사용 수정예정
        }
        else
        {
            _player.GetComponent<MeshFilter>().mesh = playerMesh[0];        //모델링 적용시 사용 수정예정
        }
        */
        _player.GetComponent<PlayerManager>().Initialize(_id, _userType);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }
    
    /// <summary>아이템 생성(아이템 초기화 및 dictionary 추가)</summary>
    /// <param name="_spawnerId">아이템ID</param>
    /// <param name="_position">아이템 position</param>
    /// <param name="_hasItem">아이템 존재여부</param>
    public void CreateItemSpawner(int _spawnerId, Vector3 _position, ItemType _type, bool _hasItem, bool _modelEnabled)
    {
        ItemSpawner _spawner = (Instantiate(itemSpawnerObject[_type], _position, itemSpawnerObject[_type].transform.rotation)).GetComponent<ItemSpawner>();
        _spawner.Initialize(_spawnerId, _hasItem, _modelEnabled);
        itemSpawners.Add(_spawnerId, _spawner);
    }
    public void SetTrap(int _spawnerId, int _floor)
    {
        ItemSpawner _trap = itemSpawners[_spawnerId];
        switch (_trap.itemType)
        {
            case ItemType.EMP:
                ItemSpawner.empTrapList.Add(new ItemSpawner.TrapInfo(_floor, _trap));
                break;
            case ItemType.LIGHTTRAP:
                ItemSpawner.empTrapList.Add(new ItemSpawner.TrapInfo(_floor, _trap));
                break;
        }
    }

    /// <summary>플레이어가 먹은 아이템 할당</summary>
    /// <param name="_spawnerId">아이템ID</param>
    /// <param name="_playerId">플레이어ID</param>
    public void SaveItemToPlayer(ItemSpawner _spawner, PlayerManager _player)
    {
        switch (_player.playerType)
        {
            case PlayerType.HUMAN:
                if (_spawner.itemType == ItemType.GUN)
                {
                    _player.playerItem.item_number1 = _spawner;
                }
                else if (_spawner.itemType == ItemType.EMP)
                {
                    _player.playerItem.item_number2.Add(_spawner);
                }
                else if (_spawner.itemType == ItemType.BATTERY)
                {
                    _player.playerItem.batteryCount += 30;                   
                }
                else
                {
                    Debug.Log($"Error - 서버에서 이미 동작하였습니다. 아이템을 먹을 수 없습니다");
                }
                break;

            case PlayerType.MONSTER:
                if (_spawner.itemType == ItemType.DRONE)
                {
                    _player.playerItem.item_number1 = _spawner;
                }
                else if (_spawner.itemType == ItemType.LIGHTTRAP)
                {
                    _player.playerItem.item_number2.Add(_spawner);
                }
                else
                {
                    Debug.Log($"Error - 서버에서 이미 동작하였습니다. 아이템을 먹을 수 없습니다");
                }
                break;
        }
        _spawner.ItemPickedUp();
    }


    /// <summary>폭탄 생성</summary>
    /// <param name="_id">폭탄 ID</param>
    /// <param name="_position">폭탄 position</param>
    public void SpawnProjectile(int _id, Vector3 _position)
    {
        GameObject _projectile = Instantiate(projectilePrefab, _position, Quaternion.identity);
        _projectile.GetComponent<ProjectileManager>().Initialize(_id);
        projectiles.Add(_id, _projectile.GetComponent<ProjectileManager>());
    }

    /// <summary>총알 생성</summary>
    /// <param name="_id">총알 ID</param>
    /// <param name="_position">총알 position</param>
    public void SpawnBullet(int _id, Vector3 _position)
    {
        GameObject _bullet = Instantiate(bulletPrefab, _position, Quaternion.identity);
        _bullet.GetComponent<BulletManager>().Initialize(_id);
        bullets.Add(_id, _bullet.GetComponent<BulletManager>());
    }

    /// <summary>EMPTrap 리스트 추가</summary>
    /// <param name="_floor">설치한 층</param>
    /// <param name="_EMPTrap">설치한 EMPTrap</param>
    public void AddEMPTrap(int _floor, ItemSpawner _EMPTrap)
    {
        ItemSpawner.empTrapList.Add(new ItemSpawner.TrapInfo(_floor, _EMPTrap));
    }
    //RemoveLigthTrap 의 매개변수와 비교하여 더 나은걸로 할 예정
    public void RemoveEMPTrap(ItemSpawner _trap)
    {
        foreach (ItemSpawner.TrapInfo _info in ItemSpawner.empTrapList)
        {
            if (_info.Compare(_trap)) {
                /*ItemSpawner usedTrap = _info.trap;
                Destroy(usedTrap.gameObject);*/

                ItemSpawner.empTrapList.Remove(_info);
                break;
            }
        }
    }
    /// <summary>LightTrap 리스트 추가</summary>
    /// <param name="_floor">설치한 층</param>
    /// <param name="_lightTrap">설치한 lightTrap</param>
    public void AddLightTrap(int _floor, ItemSpawner _lightTrap)
    {
        ItemSpawner.lightTrapList.Add(new ItemSpawner.TrapInfo(_floor, _lightTrap));
        if (players[Client.instance.myId].GetComponent<PlayerManager>().playerType == PlayerType.MONSTER)
        {
            UIManager.instance.SetLightTrapUI();
        }
    }
    public void RemoveLightTrap(int number)
    {
        /*ItemSpawner usedTrap = lightTrapList[number].trap;
        Destroy(usedTrap.gameObject);*/

        ItemSpawner.lightTrapList.Remove(ItemSpawner.lightTrapList[number]);
        if (players[Client.instance.myId].GetComponent<PlayerManager>().playerType == PlayerType.MONSTER)
        {
            UIManager.instance.SetLightTrapUI();
        }
    }
    /* UIManager로 이식중
    public void SetLightTrapUI()
    {
        for (int i = 0; i < UIManager.instance.UI_LightTrapList.Length; i++)
        {
            if (i >= lightTrapList.Count)
            {
                UIManager.instance.UI_LightTrapList[i].SetActive(false);
                UIManager.instance.lightTrapUIButton[i].gameObject.SetActive(false);
                continue;
            }
            UIManager.instance.UI_LightTrapList[i].SetActive(true);
            UIManager.instance.lightTrapUIButton[i].gameObject.SetActive(true);
            UIManager.instance.UI_LightTrapList[i].transform.position = lightTrapList[i].lightTrap.transform.position + UIManager.instance.position_UI_LightTrap[lightTrapList[i].floor - 1];
            UIManager.instance.UI_LightTrapList[i].GetComponent<MeshRenderer>().material = UIManager.instance.material_UI_LightTrap[i];
        }
    }*/
}
