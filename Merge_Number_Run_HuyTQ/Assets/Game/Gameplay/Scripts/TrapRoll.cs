using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoll : MonoBehaviour
{
    [SerializeField] private TrapStateRoll trapStateRoll;
    [SerializeField] private float speedroll;
    private enum TrapStateRoll
    {
        RotateAroundTheYAxis,
        RotateAroundTheZAxis,
    }    

    void Update()
    {
        switch(trapStateRoll)
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
        transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * speedroll);
    }
}
