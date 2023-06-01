using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightGame : MonoBehaviour
{
    public List<Transform> listPosition = new List<Transform>();
    public PlayerFight playerFight;
    public GameObject numberCreate;
    public bool isFinish;

    private int currentPositionIndex = -1;
    private FightState fightState;
    private MoneyCanvas moneyCanvas;
    private int count;

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
        moneyCanvas = FindObjectOfType<MoneyCanvas>();
    }

    private void Start()
    {
        playerFight.enabled = true;
        fightState = FightState.wait;
        count = 0;
    }

    private void Update()
    {
        switch (fightState)
        {
            case FightState.wait:
                break;
            case FightState.move:
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

    public void CreateNumber()
    {
        currentPositionIndex++; // Tăng chỉ số vị trí hiện tại lên mỗi lần tạo số mới

        // Kiểm tra nếu chỉ số vượt quá số lượng vị trí trong danh sách
        if (currentPositionIndex >= listPosition.Count)
        {
            currentPositionIndex = 0; // Quay lại vị trí đầu tiên
        }

        bool isEmptyPosition = false; // Biến kiểm tra xem có vị trí trống không

        Transform position = listPosition[currentPositionIndex];
        PosMatrix posMatrix = position.GetComponent<PosMatrix>();

        // Kiểm tra nếu vị trí hiện tại đã có người chơi
        if (posMatrix != null && !posMatrix.isHavePlayer)
        {
            isEmptyPosition = true;
        }
        else
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
                    isEmptyPosition = true;
                    break;
                }
            }
        }

        if (!isEmptyPosition)
        {
            Debug.Log("Full"); // Hiển thị thông báo debug "Full" nếu tất cả các vị trí đã có người chơi
            return;
        }

        int cost = 100 * (count + 1); // Tính toán chi phí dựa trên số lần đã tạo

        if (moneyCanvas.ScoreMoney >= cost)
        {
            Instantiate(numberCreate, position.position, position.rotation);
            posMatrix.isHavePlayer = true; // Đánh dấu vị trí đã có người chơi

            count++;
        }
        else
        {
            Debug.Log("Not enough money"); // Hiển thị thông báo debug "Not enough money" nếu không đủ tiền
        }
    }

    private void Fight()
    {

    }

    public int GetCount()
    {
        return count;
    }
}