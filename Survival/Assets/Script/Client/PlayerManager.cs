﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { CREATURE, HUMAN }

public enum PlayerAnimation
{
    humanDieAnimation,
    humanGunStandAnimation,
    humanGunHitReactionAnimation,
    humanGunWalkingAnimation,
    humanHealingAnimation,
    humanHitReactionAnimation,
    humanInstallAnimation,
    humanPoseGunStand,
    humanPoseSit,
    humanPoseStand,
    humanRopeAnimation,
    humanSneakWalkAnimation,
    creatureAttackAnimation,
    creatureDieAnimation,
    creatureHitReactionAnimation,
    creatureInstallAnimation,
    creaturePoseSit,
    creatureRunAnimation,
    creatureStandAnimation,
    creatureTeleportationAnimation,
    creatureWalkAnimation,
}

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;                 //player 이름
    public float hp;                        //체력
    public float maxHp = 100f;              //최대체력
    public MeshRenderer model;
    //public ItemSpawner grabitem;            //현재 들고있는 아이템
    public PlayerItem playerItem;           //플레이어의 아이템목록
    public PlayerType playerType;           //플레이어의 타입(괴물, 인간)
    public bool isCuring = false;           //플레이어 치료 중
    public bool isCreatureAttack = false;    //괴물 공격 성공
    public bool isInstalling = false;       //설치 오브젝트 설치중
    public bool isCreatureSpeedUp = false;   // 괴물 이속증가 여부

    public AudioClip footStepSound;
    public AudioClip busurukSound;
    public AudioClip creatureAttackSound;
    public AudioClip endSound;

    public PlayerController controller;
    public Animator animator;

    //public Dictionary<int, Animation> animationUse;

    public void Start()
    {
        controller = GetComponentInChildren<PlayerController>();
        playerItem = new PlayerItem();
        animator = this.GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        if (controller != null)
        {
            //UIManager.instance.fisrtFloorPlayer.GetComponent<RectTransform>().localRotation = Quaternion.Euler(this.transform.rotation.eulerAngles + new Vector3(0, 180, 0));
            UIManager.instance.fisrtFloorPlayer.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.y);
        }
    }

    public void Initialize(int _id, int _username)
    {
        id = _id;
        hp = maxHp;
        username = _username.ToString();
        switch (_username)
        {
            case (int)PlayerType.CREATURE:
                playerType = PlayerType.CREATURE;
                break;
            case (int)PlayerType.HUMAN:
                playerType = PlayerType.HUMAN;
                break;
        }
        //InitializeAnimationData();
    }
    /*
    private void InitializeAnimationData()
    {
        animationUse = new Dictionary<int, Animation>();
        for(int i = 0; i < GameManager.instance.playerAnimation.Length; i++)
        {
            animationUse.Add((int)(PlayerAnimation)PlayerAnimation.Parse(typeof(PlayerAnimation), GameManager.instance.playerAnimation[i].name), GameManager.instance.playerAnimation[i]);
        }
        Debug.Log("Initialized Animation.");
    }*/

    /// <summary>HP 세팅</summary>
    /// <param name="_health"></param>
    public void SetHealth(float _health)
    {
        hp = _health;
        if (id == Client.instance.myId) {
            if (playerType == PlayerType.CREATURE)
            {
                UIManager.instance.HPGuage[(int)PlayerType.CREATURE].value = _health;
            }
            else
            {
                UIManager.instance.HPGuage[(int)PlayerType.HUMAN].value = _health;
            }
        }
        /*//HP가 0이 되면 죽음
        if (hp <= 0f)
        {
            Die();
        }*/
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

    public void Hide(Vector3 _position)
    {
        transform.position = _position;       
    }

    public void PlayerInstallingSound(bool _isInstalling)
    {
        if(this.GetComponent<AudioSource>().clip != null && _isInstalling)
        {
            if(this.GetComponent<AudioSource>().clip != busurukSound)
            {
                this.GetComponent<AudioSource>().clip = busurukSound;
            }
            if( (this.GetComponent<AudioSource>().clip == busurukSound && this.GetComponent<AudioSource>().isPlaying) 
                || this.GetComponent<AudioSource>().clip != busurukSound)
            {
                this.GetComponent<AudioSource>().Stop();
            }
            else if( (this.GetComponent<AudioSource>().clip == busurukSound && !this.GetComponent<AudioSource>().isPlaying) 
                || this.GetComponent<AudioSource>().clip != busurukSound)
            {
                this.GetComponent<AudioSource>().clip = busurukSound;
                this.GetComponent<AudioSource>().pitch = 3f;
                this.GetComponent<AudioSource>().Play();
            }
        }
        if(this.GetComponent<AudioSource>().clip != null && !_isInstalling)
        {
            this.GetComponent<AudioSource>().Stop();
        }
    }

}
