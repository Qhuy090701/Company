using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject mergedObject;
    [SerializeField] private GameObject backObject;
    [SerializeField] private GameObject parent;
    [SerializeField] private int currentLevel;

    [SerializeField] private float speedMove = 0.1f;
    [SerializeField] private float speedBullets = 10f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float shottime = 0.1f;

    [SerializeField] private Transform attackPoint;

    public PlayerState currentState;

    private float lastShotTime;
    private bool hasJumped = false;
    private bool isShooting = false;

    private void Start()
    {
        //currentState = PlayerState.Idle;
        isShooting = false;
        if (parent == null)
        {
            parent = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT);
        }
    }

    public enum PlayerState
    {
        Idle,
        Moving,
        Jumping,
        Lose,
        Win,
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                IdleState();
                break;
            case PlayerState.Moving:
                MoveState();
                break;
            case PlayerState.Jumping:
                JumpState();
                break;
        }
    }

    private void IdleState()
    {
        if (Input.GetMouseButton(0))
        {
            currentState = PlayerState.Moving;
        }
    }

    private void MoveState()
    {
        Move();
        if (currentState == PlayerState.Moving)
        {
            ShootBullet();
        }
    }

    //move
    private void Move()
    {
        transform.position += Vector3.forward * speedMove * Time.deltaTime;
    }

    //shoot
    private void ShootBullet()
    {
        if (isShooting = true)
        {
            if (Time.time - lastShotTime < shottime) return;
            GameObject bullet = ObjectPool.Instance.SpawnFromPool(Constant.TAG_BULLET, attackPoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = (transform.forward + Vector3.up * 0.5f) * (speedBullets * 10);
            lastShotTime = Time.time;
            Debug.Log("Shoot");
        }
    }

    //state jump
    private void JumpState()
    {
        Move();
        if (!hasJumped)
        {
            Jump();
            hasJumped = true;
        }

        currentState = PlayerState.Moving;

    }

    private void Jump()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constant.TAG_PLAYER))
        {
            Debug.Log("merge");
            var merge = collision.gameObject.GetComponent<PlayerController>();
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

                //change tag player
                mergedObj.gameObject.tag = Constant.TAG_PLAYER;
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

            other.transform.SetParent(parent.transform);
            if (other.transform.position.x > gameObject.transform.position.x)
            {
                other.transform.position = new Vector3(gameObject.transform.position.x + 3.5f, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else if (other.transform.position.x < gameObject.transform.position.x)
            {
                other.transform.position = new Vector3(gameObject.transform.position.x - 3.5f, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            other.gameObject.tag = Constant.TAG_PLAYER;
            currentState = PlayerState.Moving;

        }

        if (other.CompareTag(Constant.TAG_COLUMN))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            gameObject.SetActive(false);
            GameObject backObj = Instantiate(backObject, gameObject.transform.position, Quaternion.identity);
            currentState = PlayerState.Moving;
            backObj.transform.SetParent(parent.transform);
        }
        if (other.CompareTag(Constant.TAG_JUMPPOINT))
        {
            currentState = PlayerState.Jumping;
            hasJumped = false;
            isShooting = false;
            Debug.Log("Jump");
        }
        if(other.CompareTag(Constant.TAG_DEADZONE))
        {
            Destroy(gameObject);
        }    
    }
}
