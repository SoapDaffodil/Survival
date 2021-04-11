using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { MONSTER, HUMAN }

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;                 //player 이름
    public float hp;                        //체력
    public float maxHp = 100f;              //최대체력
    public int itemCount = 0;               //아이템 소요개수
    public MeshRenderer model;
    public ItemSpawner grabItem;            //현재 들고있는 아이템
    public Item playerItem;                 //플레이어의 아이템목록
    public PlayerType playerType;           //플레이어의 타입(괴물, 인간)

    public PlayerController controller;

    public void Start()
    {
        controller = GetComponentInChildren<PlayerController>();
    }

    public void FixedUpdate()
    {
        if (controller != null)
        {
            UIManager.instance.fisrtFloorPlayer.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles + new Vector3(0, 180, 0));
        }
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        hp = maxHp;
    }

    /// <summary>HP 세팅</summary>
    /// <param name="_health"></param>
    public void SetHealth(float _health)
    {
        hp = _health;

        //HP가 0이 되면 죽음
        if (hp <= 0f)
        {
            Die();
        }
    }

    //3Dobject renderer off
    public void Die()
    {
        model.enabled = false;
    }

    //3Dobject renderer on, 체력 리셋
    public void Respawn()
    {
        model.enabled = true;
        SetHealth(maxHp);
    }
}
