using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGame : MonoBehaviour
{
    public List<Transform> listPosition = new List<Transform>();
    public PlayerFight playerFight;
    public bool isFinish;

    [SerializeField] private FightState fightState;
    private enum FightState
    {
        wait,
        move,
        shoot,
        end
    }

    private void Awake()
    {
        playerFight = FindObjectOfType<PlayerFight>();
    }
    private void Start()
    {
        playerFight.enabled = true;
        fightState = FightState.wait;
    }

    private void Update()
    {
        switch (fightState)
        {
            case FightState.wait:
                break;
            case FightState.move:
                break;
            case FightState.shoot:
                break;
            case FightState.end:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constant.TAG_FINISH))
        {
            isFinish = true;
            fightState = FightState.move;
        }
    }
}
