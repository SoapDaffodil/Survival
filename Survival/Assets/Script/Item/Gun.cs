using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready, Empty, Reloading
    }

    public State state { get;  set; }
    private int damage = 50;
    public float fireDistance = 100f;
    private int bulletAmount; // 가지고 있는 총 배터리 수
    private int currentBettery = 0; // 현재 탄창에 들어 있는 배터리 수
    private int bulletCapacity = 30; // 탄창 용량
    public int currentBullet = 0;  // 현재 탄창에 있는 총알 수 

    private float fireTime = 0.3f; // 총알 발사 사이 시간 간격
    private float lastFireTime; // 총을 마지막으로 쏜 시간

    private void Start()
    {
        /* 삭제
        if (currentBettery == 0)
        {
            Debug.Log("배터리가 없습니다");
            state = State.Empty;
        }
        else
        {
            state = State.Ready;
        }
        */

        UIManager.instance.currentBulletText = GameObject.Find("Current Bullet").GetComponent<Text>();
        UIManager.instance.bulletAmoutText = GameObject.Find("Bullet Amount").GetComponent<Text>();
    }

    public bool Reloade()
    {
        state = State.Reloading;
        bulletAmount = GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerItem.batteryCount;
        currentBettery = currentBullet / 30;

        if (currentBullet >= bulletCapacity)
        {
            Debug.Log("충전이 이미 된 상태입니다");
            state = State.Ready;
            return false;
        }
        else if(bulletAmount == 0)
        {
            Debug.Log("충전 가능한 배터리가 없습니다!");
        }

        Debug.Log("현재 탄창에 있는 총알 : " + currentBettery);
        Debug.Log("가지고 있는 총 총알 : " + bulletAmount);

        int chargeCapacity = bulletCapacity - currentBullet;
        int chargeBullet = Mathf.Clamp(bulletAmount, 0, chargeCapacity);
        currentBullet = Mathf.Clamp(bulletAmount + currentBettery, 0, bulletCapacity);
        bulletAmount = bulletAmount - chargeBullet;

        UIManager.instance.currentBulletText.text = string.Format(" {0:} ", currentBullet);
        UIManager.instance.bulletAmoutText.text = string.Format(" {0:} ", bulletAmount * 30);

        Debug.Log("충전 된 총알 : " + chargeBullet);
        Debug.Log("현재 탄창에 있는 총알 : " + bulletAmount);
        state = State.Ready;
        Debug.Log("장전 완료");

        return true;

    }

    /* 삭제
    public void Fire(Vector3 startPosition, Vector3 direction)
    {

        if (state == State.Ready && Time.time >= lastFireTime + fireTime)
        {
            RaycastHit hit;
            Vector3 hitPosition;

            if (Physics.Raycast(startPosition, direction, out hit, fireDistance))
            {
                /*
                var target = hit.collider.gameObject.GetComponent<IDamagable>();  //ToDo : 공격 받았을 때, 데미지를 받을 수 있는 오브젝트면...
                target.ApplyDamage(damage);
                hitPosition = hit.point;
                
            }

            lastFireTime = Time.time;

            currentBullet -= 1;
            UIManager.instance.currentBulletText.text = string.Format(" {0:} ", currentBullet);
            UIManager.instance.bulletAmoutText.text = string.Format(" {0:} ", batteryAmount * 30);
            Debug.Log("뚜쉬뚜쉬!!");

            if (currentBullet % 30 == 0)
            {
                currentBettery -= 1;
            }

            if (currentBettery <= 0 && currentBullet <= 0)
            {
                state = State.Empty;
                Debug.Log("배터리가 없습니다");
            }
        
        }
    }
    */
    public void CheckBettery()
    {
        for (int i = 0; i < Item.arrayIndex; i++)
        {
            if (Item.myItem[i].name == "Battery")
            {
                //batteryAmount += 1;
               // Debug.Log("bettery Amount : " + batteryAmount);
            }
        }
    }


}


