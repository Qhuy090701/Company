using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] private GameObject mergedObject;
    [SerializeField] private GameObject parent;
    [SerializeField] private int currentLevel;

    private void Start()
    {
        if (parent == null)
        {
            parent = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT);
        }
    }

    private void SortChildObjectsByXPosition()
    {
        // Sort child objects by x position
        for (int i = 0; i < parent.transform.childCount - 1; i++)
        {
            for (int j = i + 1; j < parent.transform.childCount; j++)
            {
                if (parent.transform.GetChild(j).position.x < parent.transform.GetChild(i).position.x)
                {
                    parent.transform.GetChild(j).SetSiblingIndex(i);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constant.TAG_NUMBER))
        {
            var merge = collision.gameObject.GetComponent<PlayerControler>();
            if (merge != null && merge.currentLevel == currentLevel)
            {
                if (GetInstanceID() < merge.GetInstanceID())
                {
                    return;
                }

                if (parent == null)
                {
                    parent = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT);
                }

                GameObject mergedObj = Instantiate(mergedObject, gameObject.transform.position, Quaternion.identity);
                mergedObj.transform.SetParent(parent.gameObject.transform);
                Destroy(gameObject);
                Destroy(collision.gameObject);

                // Move the merged object to the correct position next to the merge object
                float distance = Mathf.Abs(merge.transform.position.x - mergedObj.transform.position.x);
                if (mergedObj.transform.position.x > merge.transform.position.x)
                {
                    mergedObj.transform.position = new Vector3(merge.transform.position.x + distance, merge.transform.position.y, merge.transform.position.z);
                }
                else if (mergedObj.transform.position.x < merge.transform.position.x)
                {
                    mergedObj.transform.position = new Vector3(merge.transform.position.x - distance, merge.transform.position.y, merge.transform.position.z);
                }

                // Sort child objects by x position
                SortChildObjectsByXPosition();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_NUMBER))
        {
            if (parent == null)
            {
                parent = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT);
            }

            // Sort child objects by x position
            for (int i = 0; i < parent.transform.childCount - 1; i++)
            {
                for (int j = i + 1; j < parent.transform.childCount; j++)
                {
                    if (parent.transform.GetChild(j).position.x < parent.transform.GetChild(i).position.x)
                    {
                        parent.transform.GetChild(j).SetSiblingIndex(i);
                    }
                }
            }

            // Set the other object as a child of the parent object
            other.transform.SetParent(parent.transform);
            // Move the other object to the correct position next to the current object
            if (other.transform.position.x > gameObject.transform.position.x)
            {
                other.transform.position = new Vector3(gameObject.transform.position.x + 1f, gameObject.transform.position.y, gameObject.transform.position.z);
                Debug.Log("phai");
            }
            else if (other.transform.position.x < gameObject.transform.position.x)
            {
                other.transform.position = new Vector3(gameObject.transform.position.x - 1f, gameObject.transform.position.y, gameObject.transform.position.z);
                Debug.Log("trai");
            }

        }
    }
}
