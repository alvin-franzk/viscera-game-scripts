using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatus : MonoBehaviour
{
    public TextMeshProUGUI highScore, currentScore;

    public bool isInvulnerable = false;
    public bool isDead = false;
    public float maxHealth, currentHealth, healthRegenRate;
    public float maxStamina, currentStamina, staminaRegenRate;
    [HideInInspector] public float healthBarLength, staminaBarLength;
    public int score, currentEnemyCount, maxEnemyCount, enemiesKilled;
    public float storeStaminaRegen, storeHealthRegen;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enemiesKilled = 0;
        score = 0;
        maxHealth = 100f;
        currentHealth = 100f;
        maxStamina = 75f;
        currentStamina = 50f;
        healthRegenRate = 0.05f;
        staminaRegenRate = 2.5f;
        healthBarLength = Screen.width / 3;
        staminaBarLength = Screen.width / 3;
        currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        maxEnemyCount = 10;

        player = GameObject.Find("Player Viscera");
        highScore.text = $"High Score: {PlayerPrefs.GetInt("HighScore", 0)}";
    }

    // Update is called once per frame
    void Update()
    {   
        if (!isDead)
        {
            if (currentHealth != 0)
            {
                RegenHealth(healthRegenRate);
            }
            else
            {
                AdjustCurrentHealth(-maxHealth * Time.deltaTime);
                // AdjustCurrentStamina(-maxStamina * Time.deltaTime);
                isDead = true;
            }
            if (Mathf.Floor(currentStamina) != 0)
            {
                RegenStamina(staminaRegenRate);
                storeStaminaRegen = staminaRegenRate;
            }
            else
            {
                currentStamina = 0f;
                staminaRegenRate = 0f;
                player.GetComponent<PlayerMovementStateMachine>().StartCoroutine(player.GetComponent<PlayerMovementStateMachine>().ResetStaminaRegen());
            }

            // update current score dynamically
            currentScore.text = $"Score: {score}";
        }
        else
        {
            if (PlayerPrefs.GetInt("HighScore", 0) < score)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
            
            AdjustCurrentStamina(-maxStamina * Time.deltaTime);
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, healthBarLength, 40), Mathf.Round(currentHealth)+ "/" + Mathf.Round(maxHealth));
        GUI.Box(new Rect(10, 60, staminaBarLength, 40), Mathf.Round(currentStamina) + "/" + Mathf.Round(maxStamina));
    }

    public void RegenHealth(float value)
    {
        currentHealth += value * Time.deltaTime;

        if (currentHealth > maxHealth) { currentHealth = maxHealth; }

        healthBarLength = (Screen.width / 3) * (currentHealth / maxHealth);
    }
    public void RegenStamina(float value)
    {
        currentStamina += value * Time.deltaTime;

        if (currentStamina > maxStamina) { currentStamina = maxStamina; }

        staminaBarLength = (Screen.width / 3) * (currentStamina / maxStamina);
    }

    public void AdjustCurrentHealth(float value)
    {
        currentHealth += value;

        if (currentHealth < 0) { currentHealth = 0; }
        if (currentHealth > maxHealth) { currentHealth = maxHealth; }
        if (maxHealth < 1) { maxHealth = 1; }

        healthBarLength = (Screen.width / 2) * (currentHealth / maxHealth);
    }
    public void AdjustCurrentStamina(float value)
    {
        currentStamina += value;

        if (currentStamina < 0) { currentStamina = 0; }
        if (currentStamina > maxStamina) { currentStamina = maxStamina; }
        if (maxStamina < 1) { maxStamina = 1; }

        staminaBarLength = (Screen.width / 3) * (currentStamina / maxStamina);
    }
    
    public void ResetStaminaRegen()
    {
        currentStamina = 1f;
        staminaRegenRate = storeStaminaRegen;
    }
}
