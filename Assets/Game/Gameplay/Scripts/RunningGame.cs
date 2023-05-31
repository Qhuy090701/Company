using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunningGame : MonoBehaviour
{
    [SerializeField] private float speedTouch = 5f;
    [SerializeField] private float swipeThreshold = 20f;
    [SerializeField] private GameObject numberStart;

    private PlayerControllerState currentState;
    private PlayerRun playerRun;
    private GamePlay gamePlay;
    private CameraFollow cameraFollow;
    public GameObject parent;
    private Vector3 startPosition;
    private Vector3 endPosition;
    public int count;
    private bool isSwipingAndHolding = false; // Biến để kiểm tra xem người dùng đã vuốt và giữ chuột hay không
    public bool isFinish = false;

    public float canvasWidth = 1080;
    public float canvasHeight;

    private enum PlayerControllerState
    {
        StartGame,
        RunGame,
        EndGame
    }

    private void Start()
    {
        currentState = PlayerControllerState.StartGame;
        playerRun = FindObjectOfType<PlayerRun>();
        gamePlay = FindObjectOfType<GamePlay>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        numberStart = Instantiate(numberStart);   
        numberStart.transform.SetParent(parent.transform);
        numberStart.transform.localPosition = new Vector3(parent.transform.position.x, parent.transform.position.y, parent.transform.position.z);
        gameObject.tag = Constant.TAG_PLAYER;
        // Lấy kích thước của Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasWidth = canvasRect.rect.width;
        canvasHeight = canvasRect.rect.height;
    }

    private void Update()
    {
        CheckCount();
        switch (currentState)
        {
            case PlayerControllerState.StartGame:
                if (Input.GetMouseButtonDown(0))
                {
                    currentState = PlayerControllerState.RunGame;
                }
                break;
            case PlayerControllerState.RunGame:
                TouchMove();
                Move();
                if (isFinish)
                {
                    currentState = PlayerControllerState.EndGame;
                    return;
                }
              
                break;

            case PlayerControllerState.EndGame:
                Debug.Log("End Game Run");
                break;
        }

        if(count == 0)
        {
            //load lại scene có tên Gameplay
            SceneManager.LoadScene("Gameplay");

        }
    }

    private void Move()
    {
        transform.position += Vector3.forward * speedTouch * Time.deltaTime;
    }

    private void TouchMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            isSwipingAndHolding = true; // Bắt đầu vuốt và giữ chuột
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSwipingAndHolding = false; // Ngừng vuốt và giữ chuột
        }

        if (isSwipingAndHolding)
        {
            endPosition = Input.mousePosition;
            float distance = Vector3.Distance(startPosition, endPosition);

            if (distance > swipeThreshold)
            {
                Vector3 direction = endPosition - startPosition;

                if (direction.x > 0)
                {
                    transform.position += Vector3.right * speedTouch * Time.deltaTime;
                }
                else
                {
                    transform.position += Vector3.left * speedTouch * Time.deltaTime;
                }
            }
        }

        // Get the current position of the object
        Vector3 currentPosition = transform.position;

        // Calculate the boundaries of the canvas
        float minX = -canvasWidth / 2f;
        float maxX = canvasWidth / 2f;

        // Clamp the player position within the canvas boundaries
        currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);

        // Update the player position
        transform.position = currentPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_FINISH))
        {
            isFinish = true;
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            // CHUYỂN TRẠNG THÁI GAMEPLAY SANG FIGHTGAME


            if (gamePlay != null)
            {
                gamePlay.currentState = GamePlay.GameState.FightGame;
                cameraFollow.cameraState = CameraFollow.CameraState.CameraFollowFightGame;
            }

            return;
        }
    }

    private void CheckCount()
    {
        count = 0;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            if (child.activeSelf)
            {
                count++;
            }
        }
    }

}
