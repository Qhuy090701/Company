using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    //camera follow player
    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = 16f;

    private void LateUpdate()
    {
        //CHECK NULL
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(Constant.TAG_PLAYER).transform;
        }
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
