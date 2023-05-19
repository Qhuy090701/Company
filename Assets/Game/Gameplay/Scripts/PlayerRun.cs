using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    [SerializeField] private GameObject mergedObject;
    [SerializeField] private GameObject backObject;

    public GameObject parent;

    [SerializeField] private int currentLevel;

    [SerializeField] private float speedBullets = 10f;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float shottime = 0.1f;

    [SerializeField] private Transform attackPoint;

    [SerializeField] private BulletData bulletData;
    [SerializeField] private PlayerState currentState;


    private float lastShotTime;
    private bool hasJumped;
    private bool isShooting;

    public bool isShootingtype;

    private void Awake()
    {
        if (bulletData == null)
        {
            string bulletName = "Bullet" + currentLevel;
            bulletData = Resources.Load<BulletData>(bulletName);
        }
    }

    private void Start()
    {
        isShooting = false;
        if (parent == null)
        {
            parent = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT);
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
                Debug.Log("Jump");
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
        if (transform.parent == parent.transform)
        {
            currentState = PlayerState.Moving;
        }
    }

    private void UpdateMoveState()
    {
        if (currentState == PlayerState.Moving)
        {
            isShooting = true;
            ShootBullet();
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
        if (collision.gameObject.CompareTag(Constant.TAG_PLAYER))
        {
            var merge = collision.gameObject.GetComponent<PlayerRun>();
            if (merge != null && merge.currentLevel == currentLevel && GetInstanceID() >= merge.GetInstanceID())
            {
                if (parent == null)
                {
                    parent = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT);
                }

                GameObject mergedObj = Instantiate(mergedObject, gameObject.transform.position, Quaternion.identity);
                mergedObj.transform.SetParent(parent.gameObject.transform);
                Destroy(gameObject);
                Destroy(collision.gameObject);

                float distance = Mathf.Abs(merge.transform.position.x - mergedObj.transform.position.x);
                mergedObj.transform.position = new Vector3(
                    merge.transform.position.x + (mergedObj.transform.position.x > merge.transform.position.x ? distance : -distance),
                    merge.transform.position.y,
                    merge.transform.position.z);

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
            other.transform.position = new Vector3(
            gameObject.transform.position.x + (other.transform.position.x > gameObject.transform.position.x ? 3.5f : -3.5f),
            gameObject.transform.position.y,
                gameObject.transform.position.z);
            other.gameObject.tag = Constant.TAG_PLAYER;
            currentState = PlayerState.Moving;
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
            Debug.Log("Jump");
        }

        if (other.CompareTag(Constant.TAG_DEADZONE))
        {
            Destroy(gameObject);
        }
    }

    public void LevelDownNumber()
    {
        if (backObject == null)
        {
            return;
        }

        GameObject backObj = Instantiate(backObject, gameObject.transform.position, Quaternion.identity);
        backObj.transform.SetParent(parent.transform);
        backObj.gameObject.tag = Constant.TAG_PLAYER;
        currentState = PlayerState.Moving;
    }

    public void LevelUpNumber()
    {
        if (backObject == null)
        {
            return;
        }

        GameObject upObject = Instantiate(mergedObject, gameObject.transform.position, Quaternion.identity);
        upObject.transform.SetParent(parent.transform);
        upObject.gameObject.tag = Constant.TAG_PLAYER;
        currentState = PlayerState.Moving;
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

}