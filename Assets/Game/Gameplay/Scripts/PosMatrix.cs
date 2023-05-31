using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosMatrix : MonoBehaviour
{
    public bool isHavePlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constant.TAG_PLAYER))
        {
            Debug.Log("Va chạm");
            isHavePlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Constant.TAG_PLAYER))
        {
            Debug.Log("Rời khỏi");
            isHavePlayer = false;
        }
    }

    public bool IsEmpty()
    {
        return !isHavePlayer;
    }
}
