using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private float dame = 1f;
    [SerializeField] private float lifetime = 2.5f;

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
        //if (other.CompareTag(Constant.TAG_TRAP))
        //{
        //    ObjectPool.Instance.ReturnToPool(Constant.TAG_BULLET, gameObject);
        //}
        //ObjectPool.Instance.ReturnToPool(Constant.TAG_BULLET, gameObject);
        if (other.CompareTag(Constant.TAG_COLUMN))
        {
            Debug.Log("Va cham");
            ObjectPool.Instance.ReturnToPool(Constant.TAG_BULLET, gameObject);
            TrapHealth hurtTrap = other.gameObject.GetComponent<TrapHealth>();
            hurtTrap.TakeDamage(dame);
        }
    }
}