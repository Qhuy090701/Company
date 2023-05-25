using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoll : MonoBehaviour
{
    [SerializeField] private TrapStateRoll trapStateRoll;
    [SerializeField] private float speedroll;

    private SawTrap sawTrap;

    private void Start()
    {
        sawTrap = GetComponent<SawTrap>();
    }
    private enum TrapStateRoll
    {
        RotateAroundTheYAxis,
        RotateAroundTheZAxis,
    }

    void Update()
    {
        switch (trapStateRoll)
        {
            case TrapStateRoll.RotateAroundTheYAxis:
                UpdateRotateAroundTheYAxis();
                break;
            case TrapStateRoll.RotateAroundTheZAxis:
                UpdateRotateAroundTheZAxis();
                break;
        }
    }

    void UpdateRotateAroundTheYAxis()
    {
        transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * speedroll);
    }

    void UpdateRotateAroundTheZAxis()
    {
        //nếu di chuyển qua điểm a thì quay 0,0,1 còn di chuyển qua điểm b thì quay 0,0,-1
        if (sawTrap.targetPosition == sawTrap.pointB.position)
        {
            transform.Rotate(new Vector3(0, 0, -1) * Time.deltaTime * speedroll);
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * speedroll);
        }
    }
}
