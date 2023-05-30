using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGame : MonoBehaviour
{
    public List<Transform> listPosition = new List<Transform>();
    public PlayerFight playerFight;
    public GameObject numberCreate;
    public bool isFinish;

    private PosMatrix posmMatrix;
    [SerializeField] private FightState fightState;
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
        foreach (Transform position in listPosition)
        {
            PosMatrix posMatrix = position.GetComponent<PosMatrix>();
            if (posMatrix != null && posMatrix.IsEmpty())
            {
                Instantiate(numberCreate, position.position, position.rotation);
                break; // Nếu bạn chỉ muốn tạo số tại một vị trí duy nhất, bạn có thể thoát khỏi vòng lặp sau khi đã tạo số.
            }
        }
    }

    private void Fight()
    {

    }
}
