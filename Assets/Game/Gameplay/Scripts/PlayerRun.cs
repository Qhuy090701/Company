using System;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    [SerializeField] private GameObject mergedObject;
    [SerializeField] private GameObject backObject;

    [SerializeField] private int currentLevel;

    [SerializeField] private float speedBullets = 10f;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float shottime = 0.1f;
    [SerializeField] private float objectSpacing = 2.0f;
    [SerializeField] private Transform attackPoint;

    [SerializeField] private BulletData bulletData;
    [SerializeField] private PlayerState currentState;

    private GamePlay gamePlay;
    private RunningGame runningGame;
    private PlayerRun playerRun;
    private float lastShotTime;
    private bool hasJumped;
    public bool isShooting;
    public bool shootType;

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
        GameObject parentObject = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT);
        if (parentObject != null)
        {
            gamePlay = parentObject.GetComponent<GamePlay>();
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
        if (gamePlay != null && transform.parent == gamePlay.parent.transform)
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
            ShootBullet();
        }

        if (runningGame != null && runningGame.isFinish == true)
        {
            currentState = PlayerState.Win;
        }
    }

    public void ShootBullet()
    {
        if (isShooting && Time.time - lastShotTime >= shottime && attackPoint != null)
        {
            GameObject bullet = ObjectPool.Instance.SpawnFromPool(Constant.TAG_BULLET, attackPoint.position, Quaternion.identity);
            Bullets bulletController = bullet.GetComponent<Bullets>();
            bulletController?.SetBulletProperties(bulletData);
            bullet.GetComponent<Rigidbody>().velocity = (transform.forward + Vector3.up * 0.5f) * (speedBullets * 10);
            lastShotTime = Time.time;
            if (shootType == true)
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
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (runningGame != null && runningGame.isFinish == false)
        {
            if (collision.gameObject.CompareTag(Constant.TAG_PLAYER))
            {
                var merge = collision.gameObject.GetComponent<PlayerRun>();
                if (merge != null && merge.currentLevel == currentLevel && GetInstanceID() >= merge.GetInstanceID())
                {
                    if (gamePlay == null)
                    {
                        GameObject parentObject = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT);
                        if (parentObject != null)
                        {
                            gamePlay = parentObject.GetComponent<GamePlay>();
                        }
                    }

                    GameObject mergedObj = Instantiate(mergedObject, merge.transform.position, Quaternion.identity);
                    mergedObj.transform.SetParent(gamePlay.parent.gameObject.transform);
                    mergedObj.GetComponent<PlayerRun>().shootType = merge.shootType;
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                    mergedObj.gameObject.tag = Constant.TAG_PLAYER;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (runningGame != null && runningGame.isFinish == false)
        {
            if (other.gameObject.CompareTag(Constant.TAG_NUMBER))
            {
                if (gamePlay != null)
                {
                    other.gameObject.transform.parent = gamePlay.parent.transform;
                    other.gameObject.tag = Constant.TAG_PLAYER;

                    if (other.gameObject.transform.position.x < gameObject.transform.position.x)
                    {
                        other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x - 0.5f, gameObject.transform.position.y, gameObject.transform.position.z);
                    }
                    else
                    {
                        other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z);
                    }

                    if (shootType == true)
                    {
                        other.gameObject.GetComponent<PlayerRun>().shootType = true;
                    }

                    SortChildObjectsByX();
                }
            }

            if (other.CompareTag(Constant.TAG_COLUMN) || other.CompareTag(Constant.TAG_TRAP))
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
                other.gameObject.tag = Constant.TAG_PLAYER;
                LevelDownNumber();
            }

            if (other.CompareTag(Constant.TAG_JUMPPOINT))
            {
                currentState = PlayerState.Jumping;
                hasJumped = false;
                isShooting = false;
            }

            if (other.CompareTag(Constant.TAG_DEADZONE))
            {
                Destroy(gameObject);
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
        if (gamePlay != null)
        {
            backObj.transform.SetParent(gamePlay.parent.transform);
        }
        backObj.gameObject.tag = Constant.TAG_PLAYER;
        currentState = PlayerState.Moving;
        backObj.GetComponent<PlayerRun>().shootType = shootType; // Set shootType to true
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
        if (gamePlay != null)
        {
            upObject.transform.SetParent(gamePlay.parent.transform);
        }
        upObject.gameObject.tag = Constant.TAG_PLAYER;
        currentState = PlayerState.Moving;
        upObject.GetComponent<PlayerRun>().shootType = shootType; // Set shootType to true
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

    public void SortChildObjectsByX()
    {
        if (gamePlay == null)
        {
            GameObject parentObject = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT);
            if (parentObject != null)
            {
                gamePlay = parentObject.GetComponent<GamePlay>();
            }
        }

        if (gamePlay != null)
        {
            Transform parent = gamePlay.parent.transform;
            int childCount = parent.childCount;

            for (int i = 0; i < childCount; i++)
            {
                GameObject child = parent.GetChild(i).gameObject;
                child.transform.localPosition = new Vector3(i * objectSpacing, child.transform.localPosition.y, child.transform.localPosition.z);
            }
        }
    }

    private void WinRun()
    {
        // Do something when the player wins
    }
}
