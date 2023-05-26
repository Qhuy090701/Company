using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class CylindricalTrap : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemSpawnPoint;
    [SerializeField] private TextMesh healthText;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthText();

        if (currentHealth <= 0)
        {
            if (itemPrefab == null)
            {
                return;
            }
            else
            {
                Instantiate(itemPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
            }

            Destroy(gameObject);
        }
    }

    private void UpdateHealthText()
    {
        healthText.text =  currentHealth.ToString();
    }
}
