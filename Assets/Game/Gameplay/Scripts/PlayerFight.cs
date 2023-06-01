using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    [SerializeField] private GameObject mergedObject;
    [SerializeField] private PlayerFightState playerFightState;
    [SerializeField] private float moveSpeed = 20f; // Tốc độ di chuyển của player
    [SerializeField] private float shottime = 0.2f;
    [SerializeField] private float speedBullets = 30f;
    [SerializeField] private int currentLevel;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private Transform currentTargetMonster;
    [SerializeField] private BulletData bulletData;
    private bool isMoving = false; // Trạng thái di chuyển của player
    private bool reachedTarget = false; // Kiểm tra xem đã đạt đến vị trí chỉ định hay chưa

    private FightGame fightGame;
    private PlayerFight playerFight;
    private PlayerRun playerRun;

    private float lastShotTime;
    private enum PlayerFightState
    {
        PlayerWait,
        PlayerMove,
        StopMove,
        EndGame
    }

    private List<Transform> availablePositions;
    internal Vector3 initialPosition;

    private void Awake()
    {
        playerRun = GetComponent<PlayerRun>();
        playerFight = GetComponent<PlayerFight>();
        //playerFight.enabled = false;
        if (bulletData == null)
        {
            string bulletName = "Bullet" + currentLevel;
            bulletData = Resources.Load<BulletData>(bulletName);
        }
        if (attackPoint == null)
        {
            attackPoint = transform.Find("AttackPoint");
        }
    }

    private void Start()
    {

        
        fightGame = FindObjectOfType<FightGame>();
        availablePositions = new List<Transform>(fightGame.listPosition);
        currentTarget = GetRandomPosition();
    }

    private void Update()
    {
        switch (playerFightState)
        {
            case PlayerFightState.PlayerWait:
                // Kiểm tra nếu tag của nó là player và fightGame.isFinish = true thì chuyển state sang player move;
                if (gameObject.CompareTag(Constant.TAG_PLAYER) && fightGame.isFinish == true)
                {
                    playerFightState = PlayerFightState.PlayerMove;
                    BoxCollider boxCollider = GetComponent<BoxCollider>();
                    boxCollider.enabled = false;
                }
                else
                {
                    playerFightState = PlayerFightState.PlayerWait;
                }
                break;
            case PlayerFightState.PlayerMove:
                PlayerMove();
                break;
            case PlayerFightState.StopMove:
                ShootBulletFight();
                break;
            case PlayerFightState.EndGame:
                // Logic khi kết thúc trò chơi
                EndGame();
                break;
        }

        // Kiểm tra xem đã đạt đến vị trí chỉ định hay chưa
        if (playerFightState == PlayerFightState.StopMove && !isMoving && Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            reachedTarget = true;
        }
        else
        {
            reachedTarget = false;
        }
    }

    private void PlayerMove()
    {
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(MoveToTarget());
        }
    }

    private void ShootBulletFight()
    {
        if (currentTargetMonster == null)
        {
            currentTargetMonster = GameObject.FindGameObjectWithTag(Constant.TAG_MONSTER)?.transform;
            if (currentTargetMonster == null)
            {
                fightGame.isShooting = false;
                return;
            }
        }

        if (fightGame.isShooting && Time.time - lastShotTime >= shottime && attackPoint != null)
        {
            GameObject bullet = ObjectPool.Instance.SpawnFromPool(Constant.TAG_BULLET, attackPoint.position, Quaternion.identity);
            Bullets bulletController = bullet.GetComponent<Bullets>();
            bulletController?.SetBulletProperties(bulletData);

            Vector3 direction = (currentTargetMonster.position - attackPoint.position).normalized;
            bullet.GetComponent<Rigidbody>().velocity = direction * speedBullets;

            lastShotTime = Time.time;
        }
    }


    private IEnumerator MoveToTarget()
    {
        while (transform.position != currentTarget.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
        playerFightState = PlayerFightState.StopMove;
        PosMatrix posMatrix = currentTarget.GetComponent<PosMatrix>();
        if (posMatrix.IsEmpty())
        {
            posMatrix.isHavePlayer = true;
        }
        else
        {
            currentTarget = GetRandomPosition();
        }
    }

    private Transform GetRandomPosition()
    {
        if (availablePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Transform randomPosition = availablePositions[randomIndex];
            availablePositions.RemoveAt(randomIndex);
            return randomPosition;
        }

        return currentTarget;
    }

    private void StartShooting()
    {
        Debug.Log("Player is shooting!");
        playerFightState = PlayerFightState.EndGame;
    }

    private void EndGame()
    {
        Debug.Log("Game over!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constant.TAG_FINISH))
        {
            playerFightState = PlayerFightState.PlayerMove;
        }
    }
}