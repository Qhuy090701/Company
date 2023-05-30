using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    [SerializeField] private GameState currentState;
    private RunningGame runningGame;
    private FightGame fightGame;

    public GameObject parent;
    private enum GameState
    {
        RunGame,
        FightGame,
    }
    private void Awake()
    {
        runningGame = GetComponent<RunningGame>();
        fightGame = GetComponent<FightGame>();
    }
    private void Start()
    {
        currentState = GameState.RunGame;
    }

    private void Update()
    {
        switch(currentState)
        {
            case GameState.RunGame:
                //runningGame.enabled = true;
                //fightGame.enabled = true;
                break;
            case GameState.FightGame:
                //runningGame.enabled = false;
                //fightGame.enabled = true;
                break;
        }
    }
}
