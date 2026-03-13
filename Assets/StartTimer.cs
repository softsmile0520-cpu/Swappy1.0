using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTimer : MonoBehaviour
{
    public Image TimerValue;
    public Image viewImage;

    public float TurnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        Gamemanager.instance.timerView = this.gameObject;
        viewImage.sprite = Gamemanager.instance.CurrentPlayer.SwapieDisplay.ViewImage;

        switch(GameConfigration.instance.GameMode)
        {
            case mode.Classic:

                TurnTime = 60;
                TimerHandle();

                break;

            case mode.Fast:

                TurnTime = 15;
                TimerHandle();

                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TimerHandle()
    {
            StartCoroutine(starttime());
    }
    IEnumerator starttime()
    {
        float timer = TurnTime;

        while (timer >= 0)
        {
            timer -= Time.deltaTime; // Reduce the timer by the time passed since the last frame
            TimerValue.fillAmount = timer / TurnTime; // Update the slider value

            if (timer <= 6)
            {
                PopUpTimer.ShowUI();
            }

            if (timer <= 0)
            {
                timer = 0; // Ensure the timer doesn't go below 0
                TimerValue.fillAmount = 0; // Set the slider to 0 when the timer is finished
                Gamemanager.instance.NextTurn(); // Call your turn function)
            }

            yield return null;
        }
    }
    public void AITurnF()
    {
        SmartAIManager.instance.SmartAI();
    }
}
