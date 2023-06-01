using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCanvas : MonoBehaviour
{
    public Text MoneyPoint;
    public int ScoreMoney;
    public Button fightGameButton;
    public Button createNumberButton;
    private FightGame fightGame;

    private bool isFinishReached = false;
    private float finishReachedTime = 0f;
    private float activateButtonDelay = 2f;

    private void Awake()
    {
        fightGame = FindObjectOfType<FightGame>();
    }

    private void Start()
    {
        ScoreMoney = 500;

        // Deactivate the buttons initially
        fightGameButton.gameObject.SetActive(false);
        createNumberButton.gameObject.SetActive(false);

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
            // Activate the buttons
            fightGameButton.gameObject.SetActive(true);
            createNumberButton.gameObject.SetActive(true);
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
}
