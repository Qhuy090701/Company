using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject createNumber0;
    [SerializeField] private PortalState portalState;
    public PlayerRun playerRun;

    private void Start()
    {
        if(playerRun = null)
        {
            playerRun = GetComponent<PlayerRun>();
        }    
    }

    private enum PortalState
    {
        LevelUp,
        LevelDown,
        SpeedBulletDown,
        SpeedBulletUp,
        CreateNumber,
        CreateBullet
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_PLAYER))
        {
            PlayerRun player = other.GetComponent<PlayerRun>();
            switch (portalState)
            {
                case PortalState.LevelUp:
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                    other.gameObject.tag = Constant.TAG_PLAYER;
                    player.LevelUpNumber();
                    break;
                case PortalState.LevelDown:
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                    other.gameObject.tag = Constant.TAG_PLAYER;
                    player.LevelDownNumber();
                    break;
                case PortalState.SpeedBulletUp:
                    Destroy(gameObject);
                    other.gameObject.tag = Constant.TAG_PLAYER;
                    player.SpeedBulletUp();
                    break;
                case PortalState.SpeedBulletDown:
                    Destroy(gameObject);
                    other.gameObject.tag = Constant.TAG_PLAYER;
                    player.SpeedBulletDown();
                    break;
                case PortalState.CreateNumber:
                    Destroy(gameObject);
                    GameObject number = Instantiate(createNumber0, gameObject.transform.position, Quaternion.identity);
                    number.transform.SetParent(playerRun.parent.transform);
                    break;
                case PortalState.CreateBullet:
                    break;
            }
        }
    }
}
