using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGame : MonoBehaviour
{
    public List<Transform> listPosition = new List<Transform>();
    public PlayerFight playerFight;


    [SerializeField] private FightState fightState;
    private enum FightState
    {
        move,
        shoot,
        end
    }

    private void Awake()
    {
        //findobject nam playerfight
        playerFight = FindObjectOfType<PlayerFight>();
    }
    private void Start()
    {
        playerFight.enabled = true;
        //if (isFinish)
        //{
        //    playerFight.enabled = false;
        //}
    }

    private void Update()
    {
        switch (fightState)
        {
            case FightState.move:
                //Move();
                break;
            case FightState.shoot:
                //Shoot();
                break;
            case FightState.end:
                //End();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constant.TAG_FINISH))
        {
            Debug.Log("va chammmmmmmmmmm");
            //isFinish = true;
            //if(isFinish == true)
            //{
            //playerFight.enabled = true;
            //}
        }
    }
}
