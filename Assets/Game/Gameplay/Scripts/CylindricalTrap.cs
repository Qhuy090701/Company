using UnityEngine;
using UnityEngine.UI;

public class CylindricalTrap: MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemSpawnPoint;
    // [SerializeField] private Text healthText;

    private void Start()
    {
        currentHealth = maxHealth;
        // UpdateHealthText();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        // UpdateHealthText();

        if (currentHealth <= 0)
        {
           // Instantiate(itemPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
            Destroy(gameObject);
        }
    }

    //private void UpdateHealthText()
    //{
    //    healthText.text = "Health: " + currentHealth.ToString();
    //}
}
