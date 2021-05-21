using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempGameManager : MonoBehaviour
{
    public float seconds;
    public float min;
    public Text timer;
    public bool isGameFinish;
    public GameObject playUI;
    public GameObject endUI;

    private void Start()
    {
        seconds = 10f;
        min = 0f;
        isGameFinish = false;
    }

    private void Update()
    {
        if(!isGameFinish)
        {
            if (seconds <= 0)
            {
                if (min > 0)
                {
                    min -= 1f;
                    seconds = 59f;
                }

                if (min <= 0f && seconds <= 0f)
                {
                    isGameFinish = true;
                }
            }

            seconds = seconds - Time.deltaTime;
            timer.text = string.Format("{0:F0} : {1:F0}", min, seconds);

        }
        else
        {
            EndUI();
        }
    }

    public void EndUI()
    {
        playUI.SetActive(false);
        endUI.SetActive(true);
        /*
        for (int j = 0; j < UIManager.instance.HPBarUI.Length; j++)
        {
            UIManager.instance.HPBarUI[j].gameObject.SetActive(false);
        }
        for (int j = 0; j < UIManager.instance.skillImageUI.Length; j++)
        {
            UIManager.instance.skillImageUI[j].gameObject.SetActive(false);
            UIManager.instance.itemImageUI[j].gameObject.SetActive(false);
            UIManager.instance.itemCountText[j].gameObject.SetActive(false);
        }
        for (int j = 0; j < UIManager.instance.creatureKey.Length; j++)
        {
            UIManager.instance.creatureKey[j].gameObject.SetActive(false);
            UIManager.instance.creaturekeyBackground[j].gameObject.SetActive(false);
        }
        //GameObject.Find("Aim").gameObject.SetActive(false);
        */

        for (int i = 1; i <= GameManager.players.Count; i++)
        {            
            GameManager.players[i].GetComponent<AudioSource>().clip = GameManager.players[i].endSound;
            GameManager.players[i].GetComponent<AudioSource>().Play();

            if (GameManager.players[i].playerType == PlayerType.HUMAN && GameManager.players[i].hp > 0)
            {
                if (GameManager.players[i].id == Client.instance.myId)
                {
                    endUI.GetComponent<Image>().sprite = UIManager.instance.endImage[(int)UIManager.EndType.VICTORY];
                }
                else
                {
                    endUI.GetComponent<Image>().sprite = UIManager.instance.endImage[(int)UIManager.EndType.DEFEAT];
                }
            }
        }
    }
}
