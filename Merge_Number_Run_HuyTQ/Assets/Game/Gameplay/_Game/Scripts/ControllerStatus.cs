using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerStatus : MonoBehaviour
{
    [SerializeField] private bool status;
    [SerializeField] private Behaviour scripts;
    private PlayerControler playerController;
    // Start is called before the first frame update
    private void Start()
    {
        scripts.enabled = status;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_NUMBER))
        {
            status = true;
            //turn on script 

        }
    }
}
