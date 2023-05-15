using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerStatus : MonoBehaviour
{
    [SerializeField] private bool status;
    [SerializeField] private Behaviour scripts;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_PLAYER))
        {
            status = true;
            scripts.enabled = status;
            Debug.Log("Script is enabled");
        }
    }
}
