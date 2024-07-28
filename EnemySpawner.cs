using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy, player;
    public PlayerMovementStateMachine _pmsm;
    public LayerMask playerLayerMask;
    public float spawnRate;
    public float playerSpawnRange;
    public bool playerIsNear;
    public float timer;
    public bool canUpgrade = true;
    public int tier;
    private float upgradeCooldown;

    void Awake()
    {
        tier = 0;
        spawnRate = Random.Range(6, 11);
        playerLayerMask = LayerMask.GetMask("Player");
        playerSpawnRange = Random.Range(8, 11);
        timer = 0f;
        player = GameObject.FindGameObjectWithTag("Player");
        _pmsm = player.GetComponent<PlayerMovementStateMachine>();
        upgradeCooldown = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        playerIsNear = Physics.CheckSphere(transform.position, playerSpawnRange, playerLayerMask);

        if (!playerIsNear && _pmsm.playerStatus.currentEnemyCount < _pmsm.playerStatus.maxEnemyCount)
        {
            SpawnEnemies();
        }

        if (_pmsm.playerStatus.score >= 1000 && canUpgrade)
        {
            UpgradeSpawner();
        }

        // Upgrade limit reached
        /*if (_pmsm.playerStatus.score >= 100)
        {
            canUpgrade = false;
        }*/
    }

    void LateUpdate()
    {
        // spawnRate is set to 1 if it reaches 0 (from upgrading)
        if (spawnRate <= 0)
        {
            spawnRate = 1f;
        }

        if (playerSpawnRange <= 1)
        {
            playerSpawnRange = 2f;
        }
    }
    private void SpawnEnemies()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            HandleEnemySpawn();
            _pmsm.playerStatus.currentEnemyCount++;
            timer = 0f;
        }
    }

    private void UpgradeSpawner()
    {
        var playerScore = _pmsm.playerStatus.score;

        if (playerScore >= 1000 && playerScore < 2100 && tier == 0)
        {
            tier++;
            _pmsm.playerStatus.maxEnemyCount += MaxEnemyCountRoll(tier);
            spawnRate--;
            playerSpawnRange--;
        }
        else if (playerScore >= 2100 && playerScore < 4200 && tier == 1)
        {
            tier++;
            _pmsm.playerStatus.maxEnemyCount += MaxEnemyCountRoll(tier);
            spawnRate--;
            playerSpawnRange--;
        }
        else if (playerScore >= 4200 && playerScore < 6300 && tier == 2)
        {
            tier++;
            _pmsm.playerStatus.maxEnemyCount += MaxEnemyCountRoll(tier);
            spawnRate--;
            playerSpawnRange--;
        }
        else if (playerScore >= 6300 && playerScore < 8400 && tier == 3)
        {
            tier++;
            _pmsm.playerStatus.maxEnemyCount += MaxEnemyCountRoll(tier);
            spawnRate--;
            playerSpawnRange--;
        }
        else // Tier 4 Upgrade
        {
            _pmsm.playerStatus.maxEnemyCount += MaxEnemyCountRoll(tier);
            spawnRate--;
            playerSpawnRange--;
            upgradeCooldown += 10f;
        }

        StartCoroutine(UpgradeCooldown());
    }
    
    private IEnumerator UpgradeCooldown()
    {
        canUpgrade = false;
        yield return new WaitForSeconds(upgradeCooldown);
    }

    private int MaxEnemyCountRoll(int tier)
    {
        int count = 0;
        switch (tier)
        {
            case 0:
                count = Random.Range(3, 7);
                break;
            case 1:
                count = Random.Range(7, 14);
                break;
            case 2:
                count = Random.Range(10, 20);
                break;
            case 3:
                count = Random.Range(13, 26);
                break;
            case 4:
                count = Random.Range(16, 32);
                break;
        }

        return count;
    }

    private void HandleEnemySpawn()
    {
        GameObject enemySpawned = Instantiate(enemy, transform.position, Quaternion.identity);

        enemySpawned.GetComponent<EnemyMovementStateMachine>();
        var enemyRB = enemySpawned.GetComponent<Rigidbody>();

        // Rotate instantiated enemy towards player upon spawn
        Vector3 playerDirection = (player.transform.position - enemy.transform.position).normalized;
        var rotation = Quaternion.LookRotation(playerDirection);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, 0.15f);

        // Spat out enemy from spawner
        Vector3 spawnerDirection = (transform.position - enemy.transform.position).normalized;
        enemyRB.AddForce(spawnerDirection * 20f, ForceMode.Impulse);
    }
}
