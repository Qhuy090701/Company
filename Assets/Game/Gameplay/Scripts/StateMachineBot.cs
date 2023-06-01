using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class StateMachineBot : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private BotState botstate;
    [SerializeField] private GameObject currenttarget;
    private RunningGame runningGame;
    private CanvasSetting canvasSetting;
    private PlayerFight playerFight;
    private enum BotState
    {
        wait,
        move,
        die,
    }
    private void Awake()
    {
        playerFight = FindAnyObjectByType<PlayerFight>();
        runningGame = FindAnyObjectByType<RunningGame>();
        canvasSetting  = FindAnyObjectByType<CanvasSetting>();
    }
    private void Update()
    {
        switch(botstate)
        {
            case BotState.wait:
                //nếu click vào fight button chuyển state sang move
                if (canvasSetting.clickFightButton == true)
                {
                    botstate = BotState.move;
                }
                break;
            case BotState.move:
                Move();
                break;
            case BotState.die:
                break;
        }
    }

    private void Move()
    {
        if(currenttarget == null)
        {
          
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(Constant.TAG_PLAYER))
        {
            Destroy(other.gameObject);
            runningGame.isDie = true;
            if (runningGame.isDie == true)
            {
                canvasSetting.playAgainButton.gameObject.SetActive(true);
            }
        }
    }
}
