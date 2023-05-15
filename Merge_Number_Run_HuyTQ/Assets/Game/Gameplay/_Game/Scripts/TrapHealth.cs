using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using UnityEngine.UIElements;

public class TrapHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemSpawnPoint; 
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Instantiate(itemPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
            Destroy(gameObject);
        }
    }
}
