using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    public GameObject brickPrefab;
    public float brickSpacing = 0.2f;
    public int rowCount = 5;
    public int columnCount = 5;

    void Start()
    {
        SpawnBricks();
    }

    void SpawnBricks()
    {
        Vector3 spawnPosition = transform.position;

        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                // Tạo đối tượng gạch từ prefab
                GameObject brick = Instantiate(brickPrefab, spawnPosition, Quaternion.identity);

                // Gán giá trị -1 cho dữ liệu của gạch (nếu cần)
                // brick.GetComponent<Brick>().value = -1;

                // Cập nhật vị trí spawn cho viên gạch tiếp theo
                spawnPosition.x += brickSpacing;
            }

            // Đặt lại vị trí spawn cho hàng tiếp theo
            spawnPosition.x = transform.position.x;
            spawnPosition.y += brickSpacing;
        }
    }
}
