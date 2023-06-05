using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    //camera follow player
    [SerializeField] private Transform player;
    [SerializeField] private GameObject maxtrixMap;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed = 16f;
    [SerializeField] private float leftLimit = -3.5f;
    [SerializeField] private float rightLimit = 3.5f;
    public CameraState cameraState;

    public enum CameraState
    {
        CameraFollowRunningGame,
        CameraFollowFightGame,
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT).transform;
        }
        
        switch (cameraState)
        {
            case CameraState.CameraFollowRunningGame:
                if (player != null)
                {
                    Vector3 desiredPosition = player.position + offset;
                    desiredPosition.x = Mathf.Clamp(desiredPosition.x, leftLimit, rightLimit);
                    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
                    transform.position = smoothedPosition;
                }
                break;
            case CameraState.CameraFollowFightGame:

                //change player = matrix map
                player = maxtrixMap.transform;
                transform.position = new Vector3(-1, 34.5f, 349.9f);
                transform.rotation = Quaternion.Euler(20.3f, 0, 0);


                break;
        }
    }
}
