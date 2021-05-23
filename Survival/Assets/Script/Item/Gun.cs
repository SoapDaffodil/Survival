using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public int batteryAmount; // 가지고 있는 총 배터리 수
    public int currentBattery = 0; // 현재 탄창에 들어 있는 배터리 수
    private int batteryCapacity = 30; // 탄창 용량

    //private float fireTime = 0.3f; // 총알 발사 사이 시간 간격
    // private float lastFireTime; // 총을 마지막으로 쏜 시간

    public AudioClip normalGunSound;
    public AudioClip empGunSound;
    public AudioClip reloadSound;

    private void Start()
    {
        //UIManager.instance.currentBulletText = GameObject.Find("Current Bullet").GetComponent<Text>();
        //UIManager.instance.bulletAmoutText = GameObject.Find("Bullet Amount").GetComponent<Text>();
    }

    public bool Reloade()
    {
        batteryAmount = GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerItem.batteryCount;
        

        if (currentBattery >= batteryCapacity)
        {
            Debug.Log("충전이 이미 된 상태입니다");
            return false;
        }
        else if(batteryAmount == 0)
        {
            Debug.Log("충전 가능한 배터리가 없습니다!");

            return false;
        }

        int chargeCapacity = batteryCapacity - currentBattery;
        int chargeBattery = Mathf.Clamp(batteryAmount, 0, chargeCapacity);
        currentBattery = Mathf.Clamp(batteryAmount + currentBattery, 0, batteryCapacity);

        int before = batteryAmount / 30;
        batteryAmount = batteryAmount - chargeBattery;
        if(before > batteryAmount)
        {
            try
            {
                ClientSend.PlayerUseItem(transform.parent.GetComponent<PlayerManager>().playerItem.item_number2[0].spawnerId);
            }
            catch (System.Exception e)
            {
                Debug.Log($"error - 건을 소유하고 있지 않음");
            }
        }
        GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerItem.batteryCount = batteryAmount;

        UIManager.instance.currentBulletText.text = string.Format(" {0:} ", currentBattery);
        UIManager.instance.bulletAmoutText.text = string.Format(" {0:} ", batteryAmount);
        UIManager.instance.itemCountText[2].text = (batteryAmount / 30).ToString();
        this.GetComponent<AudioSource>().PlayOneShot(reloadSound);

        if(batteryAmount /30 == 0)
        {
            UIManager.instance.itemImageUI[2].sprite = UIManager.instance.itemGrayImage[5];
            UIManager.instance.itemCountText[2].color = UIManager.instance.textColor[(int)UIManager.TextColor.GRAY];
        }

        return true;
    }
}


