using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    public GameState currentState;
    private RunningGame runningGame;
    private FightGame fightGame;

    public GameObject parent;
    public enum GameState
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
        switch (currentState)
        {
            case GameState.RunGame:
                if (runningGame != null)
                {
                    runningGame.enabled = true;
                }
                if (fightGame != null)
                {
                    fightGame.enabled = false;
                }
                break;
            case GameState.FightGame:
                if (runningGame != null)
                {
                    runningGame.enabled = false;
                }
                if (fightGame != null)
                {
                    fightGame.enabled = true;
                }
                break;
        }
    }

}
