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

    public State state { get; private set; }
    private int damage = 50;
    public float fireDistance = 100f;
    private int betteryAmount; // 가지고 있는 총 배터리 수
    private int currentBettery = 0; // 현재 탄창에 들어 있는 배터리 수
    private int betteryCapacity = 5; // 탄창 용량
    int currentBullet = 0;  // 현재 탄창에 있는 총알 수 

    private float fireTime = 0.3f; // 총알 발사 사이 시간 간격
    private float lastFireTime; // 총을 마지막으로 쏜 시간
    public Text currentBulletText;
    public Text bulletAmoutText;


    public void Setup()
    {
        CheckBettery();

        if (currentBettery == 0)
        {
            Debug.Log("배터리가 없습니다");
            state = State.Empty;
        }
        else
        {
            state = State.Ready;
        }
    }

    public bool Reloade()
    {
        state = State.Reloading;

        if (currentBettery >= betteryCapacity || betteryAmount == 0)
        {
            Debug.Log("충전이 이미 된 상태입니다");
            state = State.Ready;
            return false;
        }

        Debug.Log("현재 탄창에 있는 배터리 : " + currentBettery);
        Debug.Log("가지고 있는 총 총알 : " + betteryAmount * 30);

        int chargeCapacity = betteryCapacity - currentBettery;
        int chargeBettery = Mathf.Clamp(betteryAmount, 0, chargeCapacity);
        currentBettery = Mathf.Clamp(betteryAmount + currentBettery, 0, betteryCapacity);
        betteryAmount = betteryAmount - chargeBettery;

        currentBullet = currentBettery * 30;
        currentBulletText.text = string.Format(" {0:} ", currentBullet);
        bulletAmoutText.text = string.Format(" {0:} ", betteryAmount * 30);

        Debug.Log("충전 된 총알 : " + chargeBettery * 30);
        Debug.Log("현재 탄창에 있는 총알 : " + currentBettery * 30);
        state = State.Ready;
        Debug.Log("장전 완료");

        //Todo : 배터리 아이템 창에서 사라지게 하기
        return true;
    }

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
                */
            }

            lastFireTime = Time.time;

            currentBullet -= 1;
            currentBulletText.text = string.Format(" {0:} ", currentBullet);
            bulletAmoutText.text = string.Format(" {0:} ", betteryAmount * 30);
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

    public void CheckBettery()
    {
        for (int i = 0; i < Item.arrayIndex; i++)
        {
            if (Item.myItem[i].name == "Battery")
            {
                betteryAmount += 1;
            }
        }
    }


}


