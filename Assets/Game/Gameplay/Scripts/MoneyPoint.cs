using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPoint : MonoBehaviour
{
    private MoneyCanvas moneyCanvas;
    [SerializeField] private int moneyPoint = 100;
    private bool hasTriggered = false;

    private void Start()
    {
        moneyCanvas = FindObjectOfType<MoneyCanvas>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.gameObject.CompareTag(Constant.TAG_PLAYER))
        {
            hasTriggered = true;
            Destroy(gameObject);
            moneyCanvas.ScoreMoney += moneyPoint;
            moneyCanvas.MoneyPoint.text = moneyCanvas.ScoreMoney.ToString();
            Debug.Log("Tiền: " + moneyCanvas.ScoreMoney);
        }
    }
}
