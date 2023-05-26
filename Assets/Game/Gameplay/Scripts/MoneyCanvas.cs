using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCanvas : MonoBehaviour
{
    public Text MoneyPoint;
    public int ScoreMoney;

    private void Start()
    {
        ScoreMoney = 0;
        MoneyPoint.text = ScoreMoney.ToString();
    }
}
