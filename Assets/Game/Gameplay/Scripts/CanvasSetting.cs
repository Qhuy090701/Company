using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasSetting : MonoBehaviour
{
    public Text MoneyPoint;
    public int ScoreMoney;
    public Button fightGameButton;
    public Button createNumberButton;
    public Button playAgainButton;
    private FightGame fightGame;
    private RunningGame runningGame;
    private PlayerFight playerFight;
    private bool isFinishReached = false;
    private bool isFightting;
    private float finishReachedTime = 0f;
    private float activateButtonDelay = 2f;
    public bool clickFightButton = false;
    private void Awake()
    {
        fightGame = FindObjectOfType<FightGame>();
        runningGame = FindObjectOfType<RunningGame>();
        playerFight = FindObjectOfType<PlayerFight>();
        // Check if the buttons were found and throw an error if not
        if (fightGameButton == null)
        {
            GameObject fightGameButtonObject = GameObject.Find("FightGameButton");
            if (fightGameButtonObject != null)
            {
                fightGameButton = fightGameButtonObject.GetComponent<Button>();
            }
            else
            {
                Debug.LogError("FightGameButton not found in the scene.");
            }
        }

        if (createNumberButton == null)
        {
            GameObject createNumberButtonObject = GameObject.Find("CreateNumberButton");
            if (createNumberButtonObject != null)
            {
                createNumberButton = createNumberButtonObject.GetComponent<Button>();
            }
            else
            {
                Debug.LogError("CreateNumberButton not found in the scene.");
            }
        }

        if (playAgainButton == null)
        {
            GameObject playAgainButtonObject = GameObject.Find("PlayAgainButton");
            if (playAgainButtonObject != null)
            {
                playAgainButton = playAgainButtonObject.GetComponent<Button>();
            }
            else
            {
                Debug.LogError("PlayAgainButton not found in the scene.");
            }
        }
    }


    private void Start()
    {
        ScoreMoney = 300;

        if(isFightting == false)
        {
            fightGameButton.gameObject.SetActive(false);
            createNumberButton.gameObject.SetActive(false);
        }

        playAgainButton.gameObject.SetActive(false);

        UpdateMoneyText();
        UpdateCreateButtonText();
    }

    private void Update()
    {
        // Check if isFinish is true and the delay has passed
        if (fightGame.isFinish && !isFinishReached)
        {
            isFinishReached = true;
            finishReachedTime = Time.time;
        }

        // Check if the delay has passed since isFinish became true
        if (isFinishReached && Time.time - finishReachedTime >= activateButtonDelay)
        {
            isFightting = true;
            if(isFightting == true)
            {
                fightGameButton.gameObject.SetActive(true);
                createNumberButton.gameObject.SetActive(true);
            }      
        }

        if(runningGame.isDie == true)
        {
            playAgainButton.gameObject.SetActive(true);
        }
    }

    public void UpdateMoneyText()
    {
        MoneyPoint.text = ScoreMoney.ToString();
    }

    public void OnCreateNumberButtonClicked()
    {
        if (fightGame != null && fightGame.isFinish)
        {
            int cost = 100 * (fightGame.GetCount() + 1);

            if (ScoreMoney >= cost)
            {
                fightGame.CreateNumber();
                ScoreMoney -= cost;
                UpdateMoneyText();
            }
            else
            {
                Debug.Log("Not enough money");
            }

            UpdateCreateButtonText();
        }
    }

    private void UpdateCreateButtonText()
    {
        int cost = 100 * (fightGame.GetCount() + 1);
        string buttonText = (ScoreMoney >= cost) ? "Create Number (" + cost + ")" : "Not enough money";
        createNumberButton.GetComponentInChildren<Text>().text = buttonText;
    }

    public void PlayAgainButtonClick()
    {
        runningGame.ResetGame();
    }

    public void FightButtonClick()
    {
        fightGame.isShooting = true;
        isFightting = false;
        clickFightButton = true;
    }

}
