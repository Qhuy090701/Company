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
    private FightGame fightGame;
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
        fightGame = FindAnyObjectByType<FightGame>();
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
        if (currenttarget == null)
        {
            // Tìm vị trí có player
            foreach (Transform position in fightGame.listPosition)
            {
                PosMatrix posMatrix = position.GetComponent<PosMatrix>();
                if (posMatrix != null && posMatrix.isHavePlayer)
                {
                    currenttarget = position.gameObject;
                    break;
                }
            }
        }
        else
        {
            // Di chuyển đến vị trí có player
            transform.position = Vector3.MoveTowards(transform.position, currenttarget.transform.position, speed * Time.deltaTime);

            // Nếu đã đến vị trí có player, chuyển state về wait
            if (transform.position == currenttarget.transform.position)
            {
                currenttarget = null;
                botstate = BotState.wait;
            }
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
