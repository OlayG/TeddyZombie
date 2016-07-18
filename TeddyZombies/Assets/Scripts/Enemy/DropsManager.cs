using UnityEngine;
using System.Collections;
using System.Collections.Generic; // That is important

public class DropsManager : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();

    GameObject enemy;
    EnemyHealth enemyHealth;
    
    void Awake()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void LateUpdate()
    {
        if (enemyHealth.currentHealth <= 0)
        {
            dropRandomItem();
        }
    }

    void dropRandomItem()
    {
        Instantiate(items[Random.Range(0, items.Count - 1)], transform.position, Quaternion.identity);
    }
}