using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPoint : MonoBehaviour
{
    private CanvasSetting canvasSetting;
    [SerializeField] private int moneyPoint = 100;
    private bool hasTriggered = false;

    private void Start()
    {
        canvasSetting = FindObjectOfType<CanvasSetting>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.gameObject.CompareTag(Constant.TAG_PLAYER))
        {
            hasTriggered = true;
            Destroy(gameObject);
            canvasSetting.ScoreMoney += moneyPoint;
            canvasSetting.MoneyPoint.text = canvasSetting.ScoreMoney.ToString();
            Debug.Log("Tiền: " + canvasSetting.ScoreMoney);
        }
    }
}
