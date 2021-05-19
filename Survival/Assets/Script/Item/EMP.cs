using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class EMP : MonoBehaviour
{

    protected float minGauge = 1f;
    protected float maxGauge = 100f;
    protected float chargingTime;
    protected Slider powerSlider;


    protected float currenGauge;
    public float chargingSpeed;
    public  bool finished;
    protected int empAmount = 0;

    public int count = 0;
    public bool isInstalling = false;
    public bool gaugeCheck = false;
    public bool isDetectiveMode = false;


    private void Start()
    {
        isInstalling = false;
        gaugeCheck = false;
        isDetectiveMode = false;

        //UIManager.instance.powerSlider = GameObject.Find("Power Slider").GetComponent<Slider>();       

        if (UIManager.instance.powerSlider != null)
        {
            Debug.Log("slider : " + UIManager.instance.powerSlider);
            finished = false;
            currenGauge = minGauge;
            UIManager.instance.powerSlider.value = minGauge;
            UIManager.instance.powerSlider.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("slider : " + UIManager.instance.powerSlider);
            Debug.Log("슬라이더가 없음");

        }
    }
    public void Update()
    {
        if (isInstalling && gaugeCheck)
        {
            chargingTime = (maxGauge - minGauge) / 100 * chargingSpeed;
            gaugeCheck = false;
            UIManager.instance.powerSlider.gameObject.SetActive(true);

            if (currenGauge < maxGauge)
            {
                currenGauge +=  chargingTime * Time.deltaTime;
                UIManager.instance.powerSlider.value = currenGauge;                
            }

            if (currenGauge >= maxGauge)
            {
                isInstalling = false;
                currenGauge = maxGauge;
                
                UIManager.instance.powerSlider.gameObject.SetActive(false);

                if(transform.parent.GetComponent<PlayerController>().isInEMPZone)
                {
                    ClientSend.InstallEMP(transform.position, GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerItem.GrabItem);
                }
                else
                {
                    int _floor = (this.transform.parent.position.y < 10f) ? 1 : 2;
                    ClientSend.Install(this.transform.position, GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerItem.GrabItem.spawnerId, _floor);
                }
                
            }
            else
            {
                gaugeCheck = true;
            }
        }
    }

   
    
    public void InstallCancle()
    {
        currenGauge = minGauge;
        UIManager.instance.powerSlider.value = minGauge;
        isInstalling = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isDetectiveMode)
        {
            for(int i = 1; i <= GameManager.players.Count; i++)
            {
                if(GameManager.players[i].playerType == PlayerType.CREATURE)
                {
                    Debug.Log("키체인지");
                    other.GetComponent<PlayerController>().KeyChange();
                }
            }
        }
    }



}

