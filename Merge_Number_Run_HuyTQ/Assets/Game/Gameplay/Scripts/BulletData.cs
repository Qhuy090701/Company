using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Custom/Bullet Data")]
public class BulletData : ScriptableObject
{
    public int damage;
    public float size;
    public Material material;
}
