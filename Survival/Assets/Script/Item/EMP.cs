using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class EMP : MonoBehaviour
{

    protected float minGauge = 15f;
    protected float maxGauge = 45f;
    protected float chargingTime;
    protected Slider powerSlider;


    protected float currenGauge;
    protected float chargingSpeed;
    public  bool finished;
    protected int empAmount = 0;

    public int count = 0;
    public bool isInstalling = false;
    public bool gaugeCheck = false;
    public bool isDetectiveMode = false;



    private void Start()
    {
        chargingSpeed = 2f;
        isInstalling = false;
        gaugeCheck = false;
        isDetectiveMode = false;

        //UIManager.instance.powerSlider = GameObject.Find("Power Slider").GetComponent<Slider>();       
        chargingTime = (maxGauge - minGauge)/100 * chargingSpeed;

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
            gaugeCheck = false;
            UIManager.instance.powerSlider.gameObject.SetActive(true);

            if (currenGauge < maxGauge)
            {
                currenGauge += currenGauge * chargingTime * Time.deltaTime;
                UIManager.instance.powerSlider.value = currenGauge;
            }

            if (currenGauge >= maxGauge)
            {
                isInstalling = false;
                currenGauge = maxGauge;
                
                UIManager.instance.powerSlider.gameObject.SetActive(false);

                ClientSend.InstallEMP(transform.position, ((ItemSpawner)(GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerItem.item_number2[0])).gameObject);
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

    public void OnTriggerStay(Collider other)
    {
        if (isDetectiveMode)
        {
            gameObject.GetComponent<SphereCollider>().radius = 5f;
            gameObject.GetComponent<SphereCollider>().isTrigger = true;

            if (other.GetComponent<PlayerManager>().playerType == PlayerType.MONSTER)
            {
                Debug.Log($"몬스터가 접근하고 있는 중입니다.");
            }
        }
    }



}

