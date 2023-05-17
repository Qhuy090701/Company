using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    private int damage;
    void Update()
    {
        StartCoroutine(DisableAfterTime());
    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        ObjectPool.Instance.ReturnToPool(Constant.TAG_BULLET, gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_TRAP))
        {
            ObjectPool.Instance.ReturnToPool(Constant.TAG_BULLET, gameObject);
        }
        if (other.CompareTag(Constant.TAG_COLUMN))
        {
            Debug.Log("Va cham");
            ObjectPool.Instance.ReturnToPool(Constant.TAG_BULLET, gameObject);
            TrapHealth hurtTrap = other.gameObject.GetComponent<TrapHealth>();
            hurtTrap.TakeDamage(damage);
        }
    }

    public void SetBulletProperties(BulletData bulletData)
    {
        damage = bulletData.damage;
        float size = bulletData.size;
        Material material = bulletData.material;
        Debug.Log("Bullet Properties: " + damage + "  " + size + " " + material);
    }
}
