using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject creaturePrefab;
    public GameObject humanPrefab;
    public GameObject projectilePrefab;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    /// <summary>이미 존재하는지 체크</summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destorying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(50, 80);
    }

    /// <summary>앱이 꺼질때 서버도 종료</summary>
    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    /// <summary>player 초기화</summary>
    /// <returns></returns>
    public Player InstantiatePlayer(PlayerType _playerType)
    {
        switch (_playerType)
        {
            case PlayerType.CREATURE:
                return Instantiate(creaturePrefab, new Vector3(0f, 0.5f, 0f), Quaternion.identity).GetComponent<Player>();
            case PlayerType.HUMAN:
                return Instantiate(humanPrefab, new Vector3(0f, 0.5f, 0f), Quaternion.identity).GetComponent<Player>();
            default:
                Debug.Log($"잘못된 플레이어 타입입니다 - playerType : {_playerType}");
                return null;
        }
    }

    /// <summary>폭탄 초기화</summary>
    /// <param name="_shootOrigin"></param>
    /// <returns></returns>
    public Projectile InstantiateProjectile(Transform _shootOrigin)
    {
        return Instantiate(projectilePrefab, _shootOrigin.position + _shootOrigin.forward * 0.7f, Quaternion.identity).GetComponent<Projectile>();
    }

    /// <summary>총알 초기화</summary>
    /// <param name="_shootOrigin"></param>
    /// <returns></returns>
    public Bullet InstantiateBullet(Transform _shootOrigin)
    {
        return Instantiate(bulletPrefab, _shootOrigin.position + _shootOrigin.forward * 0.7f, Quaternion.identity).GetComponent<Bullet>();
    }
}
