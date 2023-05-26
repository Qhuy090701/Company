using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    [SerializeField] private PlayerFightState playerFightState;
    [SerializeField] private float moveSpeed = 20f; // Tốc độ di chuyển của player

    private bool isMoving = false; // Trạng thái di chuyển của player
    private bool reachedTarget = false; // Kiểm tra xem đã đạt đến vị trí chỉ định hay chưa

    private FightGame fightGame;
    private PlayerFight playerFight;
    private PlayerRun playerRun;
    [SerializeField] private Transform currentTarget;

    private enum PlayerFightState
    {
        PlayerWait,
        PlayerMove,
        StopMove,
        EndGame
    }

    private void Awake()
    {
        playerRun = GetComponent<PlayerRun>();
        playerFight = GetComponent<PlayerFight>();
        //playerFight.enabled = false;
    }

    private void Start()
    {
        fightGame = FindObjectOfType<FightGame>();

        if (fightGame != null && fightGame.listPosition.Count > 0)
        {
            playerFightState = PlayerFightState.PlayerWait;
            currentTarget = fightGame.listPosition[Random.Range(0, fightGame.listPosition.Count)];
        }
        else
        {
            Debug.LogError("Missing or empty listPosition in FightGame!");
        }
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
                // Logic khi dừng di chuyển
                break;
            case PlayerFightState.EndGame:
                // Logic khi kết thúc trò chơi
                EndGame();
                break;
        }

        // Kiểm tra xem đã đạt đến vị trí chỉ định hay chưa
        if (playerFightState == PlayerFightState.StopMove && !isMoving)
        {
            reachedTarget = true;
        }
        else
        {
            reachedTarget = false;
        }

        // Kiểm tra nếu tất cả các object trong parent đã đạt đến vị trí chỉ định
        if (reachedTarget && transform.parent != null)
        {
            bool allObjectsReachedTarget = true;
            foreach (Transform child in transform.parent)
            {
                PlayerFight childPlayerFight = child.GetComponent<PlayerFight>();
                if (childPlayerFight != null && !childPlayerFight.reachedTarget)
                {
                    allObjectsReachedTarget = false;
                    break;
                }
            }

            if (allObjectsReachedTarget)
            {
                // Tất cả các object trong parent đã đạt đến vị trí chỉ định
                Debug.Log("Tất cả các object đã đạt đến vị trí chỉ định!");
            }
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

    private IEnumerator MoveToTarget()
    {
        while (transform.position != currentTarget.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
        playerFightState = PlayerFightState.StopMove;

        // Kiểm tra nếu vị trí hiện tại là vị trí đã có nhân vật, thì chọn vị trí khác làm mục tiêu mới
        PosMatrix posMatrix = currentTarget.GetComponent<PosMatrix>();
        if (posMatrix.isHavePlayer)
        {
            currentTarget = GetNewTarget();
        }
    }

    private Transform GetNewTarget()
    {
        HashSet<Transform> availablePositions = new HashSet<Transform>();

        foreach (Transform position in fightGame.listPosition)
        {
            PosMatrix posMatrix = position.GetComponent<PosMatrix>();
            if (!posMatrix.isHavePlayer)
            {
                availablePositions.Add(position);
            }
        }

        if (availablePositions.Count > 0)
        {
            Transform[] availablePositionsArray = new Transform[availablePositions.Count];
            availablePositions.CopyTo(availablePositionsArray);
            return availablePositionsArray[Random.Range(0, availablePositionsArray.Length)];
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
            Debug.Log("block wall");
            playerFightState = PlayerFightState.PlayerMove;
        }
    }
}
