using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private Vector3 currentPosition;
    private Vector3 targetPosition;
    private float speed = 0f;

    void Start()
    {
        currentPosition = pointA.position;
        targetPosition = pointB.position;
        float distanceAB = Vector3.Distance(pointA.position, pointB.position);
        speed = distanceAB / 2.0f;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        if (transform.position == targetPosition)
        {
            if (targetPosition == pointA.position)
            {
                targetPosition = pointB.position;
            }
            else
            {
                targetPosition = pointA.position;
            }
        }
    }
}
