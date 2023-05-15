using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTMATCH : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(Constant.TAG_NUMBER))
        {
            other.gameObject.transform.parent = parent.transform;
            if (other.gameObject.transform.position.x > transform.position.x)
            {
                other.gameObject.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
            }
            else
            {
                other.gameObject.transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
            }
        }
    }
}
