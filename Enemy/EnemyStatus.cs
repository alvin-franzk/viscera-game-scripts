using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public float maxHealth, currentHealth;
    public bool isDead = false;

    void Awake()
    {
        maxHealth = Random.Range(15, 30);
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (currentHealth <= 0)
            {
                AdjustCurrentHealth(-maxHealth * Time.deltaTime);
                isDead = true;
            }
        }
    }

    public void AdjustCurrentHealth(float value)
    {
        currentHealth += value;

        if (currentHealth < 0) { currentHealth = 0; }
        if (currentHealth > maxHealth) { currentHealth = maxHealth; }
        if (maxHealth < 1) { maxHealth = 1; }
    }
}
