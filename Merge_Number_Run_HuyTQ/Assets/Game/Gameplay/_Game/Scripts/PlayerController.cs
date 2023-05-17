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

    [SerializeField] private BulletData bulletData;
    private PlayerState currentState;

    private float lastShotTime;
    private bool hasJumped = false;
    private bool isShooting = false;

    private void Awake()
    {
        if (bulletData == null)
        {
            string bulletName = "Bullet" + currentLevel;
            bulletData = Resources.Load<BulletData>(bulletName);

            if (bulletData == null)
            {
                Debug.LogError("Unable to find databullet: " + bulletName);
            }
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
            if (attackPoint == null) return;
            GameObject bullet = ObjectPool.Instance.SpawnFromPool(Constant.TAG_BULLET, attackPoint.position, Quaternion.identity);

            Bullets bulletController = bullet.GetComponent<Bullets>();
            bulletController.SetBulletProperties(bulletData);

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
            SortChildObjectsByX();
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

        if (other.CompareTag(Constant.TAG_COLUMN) || other.CompareTag(Constant.TAG_TRAP))
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
        if (other.CompareTag(Constant.TAG_DEADZONE))
        {
            Destroy(gameObject);
        }
    }
    private void SortChildObjectsByX()
    {
        if (parent == null)
        {
            parent = GameObject.FindGameObjectWithTag(Constant.TAG_PARENT);
        }

        // Lấy danh sách các object con trong parent
        GameObject[] childObjects = new GameObject[parent.transform.childCount];
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            childObjects[i] = parent.transform.GetChild(i).gameObject;
        }

        // Sắp xếp danh sách các object con dựa trên vị trí x
        System.Array.Sort(childObjects, (obj1, obj2) => obj1.transform.position.x.CompareTo(obj2.transform.position.x));

        // Đặt lại vị trí của các object con trong parent dựa trên thứ tự đã sắp xếp
        for (int i = 0; i < childObjects.Length; i++)
        {
            childObjects[i].transform.SetSiblingIndex(i);
        }
    }
}
