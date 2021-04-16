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
                                                                                                      //public static Dictionary<int, EnemyManager> enemies = new Dictionary<int, EnemyManager>();
    /// <summary>자신이 사람인지 몬스터인지 판단</summary>
    public static bool character_human;
    /// <summary>자신 player 프리팹</summary>
    public GameObject localPlayerPrefab;
    /// <summary>다른 player 프리팹</summary>
    public GameObject otherPlayerPrefab;
    [Tooltip("[0] : monster, [1] : human 모델링 메쉬")]
    public Mesh[] playerMesh = new Mesh[2];
    /// <summary>아이템 프리팹</summary>
    public GameObject[] itemSpawnerPrefab;
    public Dictionary<ItemType, GameObject> itemSpawnerObject;
    /// <summary>폭탄 프리팹</summary>
    public GameObject projectilePrefab;


    /// <summary>LightTrap 리스트</summary>
    public struct LightTrapInfo
    {
        public int floor;
        public MonoBehaviour lightTrap;
        public LightTrapInfo(int f, MonoBehaviour trap)
        {
            floor = f;
            lightTrap = trap;
        }
    }
    public List<LightTrapInfo> lightTrapList = new List<LightTrapInfo>();

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
    }


    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>
    /// <param name="_position">The player's starting position.</param>
    /// <param name="_rotation">The player's starting rotation.</param>
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {//현재 클라이언트의 플레이어인경우
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {//다른 크라이언트의 플레이어인경우
            _player = Instantiate(otherPlayerPrefab, _position, _rotation);
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
        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());
        if (_id.ToString() == UIManager.instance.usernameField.text)
        {
            switch (UIManager.instance.usernameField.text)
            {
                case "0": case "monster": case "Monster":
                    players[_id].playerType = PlayerType.MONSTER;
                    break;
                case "1": case "human": case "Human":
                    players[_id].playerType = PlayerType.HUMAN;
                    break;
            }
        }
    }
    
    /// <summary>아이템 생성(아이템 초기화 및 dictionary 추가)</summary>
    /// <param name="_spawnerId">아이템ID</param>
    /// <param name="_position">아이템 position</param>
    /// <param name="_hasItem">아이템 존재여부</param>
    public void CreateItemSpawner(int _spawnerId, Vector3 _position, bool _hasItem, ItemType _type)
    {
        GameObject _spawner = Instantiate(itemSpawnerObject[_type], _position, itemSpawnerObject[_type].transform.rotation);
        _spawner.GetComponent<ItemSpawner>().Initialize(_spawnerId, _hasItem);
        itemSpawners.Add(_spawnerId, _spawner.GetComponent<ItemSpawner>());
    }

    /// <summary>플레이어가 먹은 아이템 할당</summary>
    /// <param name="_spawnerId">아이템ID</param>
    /// <param name="_playerId">플레이어ID</param>
    public void SaveItemToPlayer(int _spawnerId, int _playerId)
    {
        switch (players[_playerId].GetComponent<PlayerManager>().playerType)
        {
            case PlayerType.HUMAN:
                if (itemSpawners[_spawnerId].itemType == ItemType.GUN)
                {
                    players[_playerId].GetComponent<PlayerManager>().playerItem.item_number1 = itemSpawners[_spawnerId].gameObject;
                }
                else if (itemSpawners[_spawnerId].itemType == ItemType.EMP)
                {
                    players[_playerId].GetComponent<PlayerManager>().playerItem.item_number2.Add(itemSpawners[_spawnerId].gameObject);
                }
                break;

            case PlayerType.MONSTER:
                if (itemSpawners[_spawnerId].itemType == ItemType.DRONE)
                {
                    players[_playerId].GetComponent<PlayerManager>().playerItem.item_number1 = itemSpawners[_spawnerId].gameObject;
                }
                else if (itemSpawners[_spawnerId].itemType == ItemType.LIGHTTRAP)
                {
                    players[_playerId].GetComponent<PlayerManager>().playerItem.item_number2.Add(itemSpawners[_spawnerId].gameObject);
                }
                break;
        }
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
    
    public void AddLightTrap(int _floor, MonoBehaviour _lightTrap)
    {
        lightTrapList.Add(new LightTrapInfo(_floor, _lightTrap));
        SetLightTrapUI();
    }
    public void RemoveLightTrap(int number)
    {
        lightTrapList.Remove(lightTrapList[number]);
        SetLightTrapUI();
    }
    public void SetLightTrapUI()
    {
        for (int i=0;i< UIManager.instance.UI_LightTrapList.Length;i++)
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
    }

}
