using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerRun : MonoBehaviour
{
    [SerializeField] private GameObject mergedObject;
    [SerializeField] private GameObject backObject;


    [SerializeField] private int currentLevel;

    [SerializeField] private float speedBullets = 10f;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float shottime = 0.1f;
    [SerializeField] private float objectSpacing = 2f;
    [SerializeField] private Transform attackPoint;

    [SerializeField] private BulletData bulletData;
    [SerializeField] private PlayerState currentState;

    private RunningGame runningGame;
    private PlayerRun playerRun;
    private float lastShotTime;
    private bool hasJumped;
    public bool isShooting;
    //public bool shootType;

    private void Awake()
    {
        runningGame = FindObjectOfType<RunningGame>();
        playerRun = GetComponent<PlayerRun>();
        if (bulletData == null)
        {
            string bulletName = "Bullet" + currentLevel;
            bulletData = Resources.Load<BulletData>(bulletName);
        }
    }

    private void Start()
    {
        isShooting = false;
        if (runningGame.parent == null)
        {
            runningGame.parent = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT);
        }
    }

    private enum PlayerState
    {
        Idle,
        Wait,
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
                UpdateIdleState();
                break;
            case PlayerState.Wait:
                UpdateWaitState();
                break;
            case PlayerState.Moving:
                UpdateMoveState();
                break;
            case PlayerState.Jumping:
                UpdateJumpState();
                break;
            case PlayerState.Win:
                WinRun();
                break;
        }
    }

    private void UpdateIdleState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentState = PlayerState.Wait;
        }
    }

    private void UpdateWaitState()
    {
        if (transform.parent == runningGame.parent.transform)
        {
            currentState = PlayerState.Moving;
        }
    }

    private void UpdateMoveState()
    {
        if (currentState == PlayerState.Moving)
        {
            isShooting = true;
        }

        if (isShooting == true)
        {
            ShootBulletRun();
        }

        if (runningGame.isFinish == true)
        {
            currentState = PlayerState.Win;
        }

        SortChildrenByPosX();
        UpdatePos();
    }


    public void ShootBulletRun()
    {
        if (isShooting && Time.time - lastShotTime >= shottime && attackPoint != null)
        {
            GameObject bullet = ObjectPool.Instance.SpawnFromPool(Constant.TAG_BULLET, attackPoint.position, Quaternion.identity);
            Bullets bulletController = bullet.GetComponent<Bullets>();
            bulletController?.SetBulletProperties(bulletData);
            bullet.GetComponent<Rigidbody>().velocity = (transform.forward + Vector3.up * 0.5f) * (speedBullets * 10);
            lastShotTime = Time.time;
            if (runningGame.shootType == true)
            {
                CreateBullets();
            }
        }
    }


    private void UpdateJumpState()
    {
        if (!hasJumped)
        {
            Jump();
            isShooting = false;
            hasJumped = true;
            currentState = PlayerState.Moving;
        }
    }

    private void Jump()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce , ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (runningGame.isFinish == false)
        {
            if (collision.gameObject.CompareTag(Constant.TAG_PLAYER))
            {
                var merge = collision.gameObject.GetComponent<PlayerRun>();
                if (merge != null && merge.currentLevel == currentLevel && GetInstanceID() >= merge.GetInstanceID())
                {
                    if (runningGame.parent == null)
                    {
                        runningGame.parent = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT);
                    }

                    GameObject mergedObj = Instantiate(mergedObject, merge.transform.position, Quaternion.identity);
                    mergedObj.transform.SetParent(runningGame.parent.gameObject.transform);
                    mergedObj.GetComponent<PlayerRun>().runningGame.shootType = merge.runningGame.shootType;
                    Destroy(gameObject);
                    Destroy(collision.gameObject);

                    mergedObj.gameObject.tag = Constant.TAG_PLAYER;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (runningGame.isFinish == false)
        {
            if (other.gameObject.CompareTag(Constant.TAG_NUMBER))
            {
                other.gameObject.transform.parent = runningGame.parent.transform;
                other.gameObject.tag = Constant.TAG_PLAYER;

                if (runningGame.shootType == true)
                {
                    other.gameObject.GetComponent<PlayerRun>().runningGame.shootType = true;
                }
            }

            if (other.CompareTag(Constant.TAG_COLUMN) || other.CompareTag(Constant.TAG_TRAP))
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
                other.gameObject.tag = Constant.TAG_PLAYER;
                LevelDownNumber();
            }

            //if (other.CompareTag(Constant.TAG_JUMPPOINT))
            //{
            //    currentState = PlayerState.Jumping;
            //    hasJumped = false;
            //    isShooting = false;
            //}

            if (other.CompareTag(Constant.TAG_DEADZONE))
            {
                Destroy(gameObject);
            }

            if (other.CompareTag(Constant.TAG_LINE))
            {
                gameObject.SetActive(false);
            }
        }

    }

    public void LevelDownNumber()
    {
        if (backObject == null)
        {
            return;
        }

        GameObject backObj = Instantiate(backObject, gameObject.transform.position, Quaternion.identity);
        backObj.transform.SetParent(runningGame.parent.transform);
        backObj.gameObject.tag = Constant.TAG_PLAYER;
        currentState = PlayerState.Moving;
        backObj.GetComponent<PlayerRun>().runningGame.shootType = runningGame.shootType; // Set shootType to true
    }

    public void LevelUpNumber()
    {
        if (mergedObject == null)
        {
            return;
        }

        GameObject upObject = Instantiate(mergedObject, gameObject.transform.position, Quaternion.identity);
        //debuglog up level
        Debug.Log("up level");
        upObject.transform.SetParent(runningGame.parent.transform);
        upObject.gameObject.tag = Constant.TAG_PLAYER;
        currentState = PlayerState.Moving;
        upObject.GetComponent<PlayerRun>().runningGame.shootType = runningGame.shootType; // Set shootType to true
    }


    public void SpeedBulletDown()
    {
        speedBullets -= 1;
    }

    public void SpeedBulletUp()
    {
        speedBullets += 1;
    }

    public void CreateNumber()
    {

    }

    public void CreateBullets()
    {
        if (attackPoint != null)
        {
            // Tạo tia bắn sang bên trái 
            GameObject bulletLeft = ObjectPool.Instance.SpawnFromPool(Constant.TAG_BULLET, attackPoint.position, Quaternion.identity);
            Bullets bulletControllerLeft = bulletLeft.GetComponent<Bullets>();
            bulletControllerLeft?.SetBulletProperties(bulletData);
            bulletLeft.GetComponent<Rigidbody>().velocity = (transform.forward + Vector3.left * 0.1f) * (speedBullets * 10);

            // Tạo tia bắn sang bên phải 
            GameObject bulletRight = ObjectPool.Instance.SpawnFromPool(Constant.TAG_BULLET, attackPoint.position, Quaternion.identity);
            Bullets bulletControllerRight = bulletRight.GetComponent<Bullets>();
            bulletControllerRight?.SetBulletProperties(bulletData);
            bulletRight.GetComponent<Rigidbody>().velocity = (transform.forward + Vector3.right * 0.1f) * (speedBullets * 10);
        }
    }

    private void WinRun()
    {
        playerRun.enabled = false;
        isShooting = false;
        return;
    }

    private void SortChildrenByPosX()
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in runningGame.parent.transform)
        {
            children.Add(child);
        }

        children.Sort((a, b) => a.position.x.CompareTo(b.position.x));
        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }

    private void UpdatePos()
    {
        for (int i = 0; i < runningGame.parent.transform.childCount; i++)
        {
            Transform child = runningGame.parent.transform.GetChild(i);
            Vector3 parentPos = runningGame.parent.transform.position;
            float newX = (float)(parentPos.x + i * objectSpacing * 1.5);
            Vector3 newPos = new Vector3(newX, parentPos.y, parentPos.z);
            child.position = newPos;
        }
    }
}

