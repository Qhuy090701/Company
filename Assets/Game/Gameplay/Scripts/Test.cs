using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject parent;

    void Start()
    {
        if(parent == null)
        {
            //find parent
            parent = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constant.TAG_NUMBER))
        {
            other.gameObject.transform.parent = parent.transform; 
            other.gameObject.tag = Constant.TAG_PLAYER;

            if(other.gameObject.transform.position.x < gameObject.transform.position.x)
            {
                other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x - 0.5f, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else
            {
                other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z);
               
            }
        }
    }
}
