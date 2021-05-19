﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cure : MonoBehaviour
{
    //private int hp = 1;
    //public bool isDead = false;
    private Slider hpSlider;
    private float minGauge = 1f;
    private float maxGauge = 100f;
    private float currenGauge;
    private float chargingSpeed;
    private float chargingTime = 3f;

    private void Start()
    {
        chargingSpeed = 10f;
        chargingTime = (maxGauge - minGauge) / 100 * chargingSpeed;
        currenGauge = minGauge;

        if (UIManager.instance.hpSlider != null)
        {
            Debug.Log("slider : " + UIManager.instance.hpSlider);
            currenGauge = minGauge;
            UIManager.instance.hpSlider.value = minGauge;
            UIManager.instance.hpSlider.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("slider : " + UIManager.instance.powerSlider);
            Debug.Log("슬라이더가 없음");
        }
    }

    public void Update()
    {
        if (GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().isCuring)
        {
            UIManager.instance.hpSlider.gameObject.SetActive(true);

            if (currenGauge < maxGauge)
            {
                currenGauge += chargingTime * Time.deltaTime;
                UIManager.instance.hpSlider.value = currenGauge;

                if (this.GetComponent<AudioSource>().clip != null && this.GetComponent<PlayerManager>().playerType == PlayerType.HUMAN)
                {
                    if (this.GetComponent<AudioSource>().clip != this.GetComponent<PlayerManager>().busurukSound)
                    {
                        this.GetComponent<AudioSource>().clip = this.GetComponent<PlayerManager>().busurukSound;
                    }
                    Debug.Log($"플레이어 치료 소리 : {this.GetComponent<AudioSource>().clip.name}");

                    if ((this.GetComponent<AudioSource>().clip == this.GetComponent<PlayerManager>().busurukSound && !this.GetComponent<AudioSource>().isPlaying)
                        || this.GetComponent<AudioSource>().clip != this.GetComponent<PlayerManager>().busurukSound)
                    {
                        this.GetComponent<AudioSource>().clip = this.GetComponent<PlayerManager>().busurukSound;
                        this.GetComponent<AudioSource>().pitch = 3f;
                        this.GetComponent<AudioSource>().Play();
                        Debug.Log("소리 재생");
                    }
                }
            }
            else if (currenGauge >= maxGauge)
            {
                GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().isCuring = false;
                currenGauge = maxGauge;
                UIManager.instance.hpSlider.gameObject.SetActive(false);
                UIManager.instance.HPGuage[(int)PlayerType.HUMAN].value += 50;

                Debug.Log($"플레이어 체력 : {GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().hp}");
                ClientSend.Cure(GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().hp);
                ClientSend.SkillCure(false);
                currenGauge = minGauge;
                UIManager.instance.hpSlider.value = currenGauge;

                if (this.GetComponent<AudioSource>().clip != null && this.GetComponent<PlayerManager>().playerType == PlayerType.HUMAN)
                {
                    if (this.GetComponent<AudioSource>().clip != this.GetComponent<PlayerManager>().busurukSound)
                    {
                        this.GetComponent<AudioSource>().clip = this.GetComponent<PlayerManager>().busurukSound;
                    }
                    Debug.Log($"플레이어 치료 소리 : {this.GetComponent<AudioSource>().clip.name}");
                    if ((this.GetComponent<AudioSource>().clip == this.GetComponent<PlayerManager>().busurukSound && this.GetComponent<AudioSource>().isPlaying)
                        || this.GetComponent<AudioSource>().clip != this.GetComponent<PlayerManager>().busurukSound)
                    {
                        this.GetComponent<AudioSource>().Stop();
                        Debug.Log("소리 멈춤");
                    }
                }
            }
        }
    }


}
