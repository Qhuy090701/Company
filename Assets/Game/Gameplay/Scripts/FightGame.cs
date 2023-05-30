using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGame : MonoBehaviour
{
    public List<Transform> listPosition = new List<Transform>();
    public PlayerFight playerFight;
    public GameObject numberCreate;
    public bool isFinish;

    private int currentPositionIndex = -1;
    private FightState fightState;

    private enum FightState
    {
        wait,
        move,
        shoot,
        end
    }

    private void Awake()
    {
        playerFight = FindObjectOfType<PlayerFight>();
    }
    private void Start()
    {
        playerFight.enabled = true;
        fightState = FightState.wait;
    }

    private void Update()
    {
        switch (fightState)
        {
            case FightState.wait:
                break;
            case FightState.move:
                if (Input.GetKeyDown(KeyCode.X))
                {
                    CreateNumber();
                }
                Fight();
                break;
            case FightState.shoot:
                break;
            case FightState.end:
                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constant.TAG_FINISH))
        {
            isFinish = true;
            fightState = FightState.move;
        }
    }

    private void CreateNumber()
    {
        currentPositionIndex++; // Tăng chỉ số vị trí hiện tại lên mỗi lần tạo số mới

        // Kiểm tra nếu chỉ số vượt quá số lượng vị trí trong danh sách
        if (currentPositionIndex >= listPosition.Count)
        {
            currentPositionIndex = 0; // Quay lại vị trí đầu tiên
        }

        Transform position = listPosition[currentPositionIndex];
        PosMatrix posMatrix = position.GetComponent<PosMatrix>();

        // Kiểm tra nếu vị trí hiện tại đã có người chơi
        if (posMatrix != null && !posMatrix.isHavePlayer)
        {
            // Tìm vị trí tiếp theo không có người chơi
            for (int i = currentPositionIndex + 1; i < listPosition.Count; i++)
            {
                PosMatrix nextPosMatrix = listPosition[i].GetComponent<PosMatrix>();
                if (nextPosMatrix != null && !nextPosMatrix.isHavePlayer)
                {
                    position = listPosition[i];
                    posMatrix = nextPosMatrix;
                    currentPositionIndex = i; // Cập nhật chỉ số vị trí hiện tại
                    break;
                }
            }
        }

        // Tạo số mới tại vị trí đã chọn
        Instantiate(numberCreate, position.position, position.rotation);
        posMatrix.isHavePlayer = true; // Đánh dấu vị trí đã có người chơi
    }


    private void Fight()
    {

    }
}
