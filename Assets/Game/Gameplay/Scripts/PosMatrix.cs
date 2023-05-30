using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosMatrix : MonoBehaviour
{
    public bool isHavePlayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(Constant.TAG_PLAYER))
        {
            Debug.Log("va cham");
            isHavePlayer = true;
            return;
        }
    }

    public bool IsEmpty()
    {
        return !isHavePlayer;
    }

}
