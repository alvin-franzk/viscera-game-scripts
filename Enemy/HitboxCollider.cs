using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitboxCollider : MonoBehaviour
{
    private PlayerMovementStateMachine _pmsm;
    private EnemyMovementStateMachine _emsm;
    private GameObject player, enemy;
    private bool isCollidingWithWeapon;

    public bool IsCollidingWithWeapon
    {
        get { return isCollidingWithWeapon; }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = transform.parent.gameObject;
        _pmsm = player.GetComponent<PlayerMovementStateMachine>();
        _emsm = enemy.GetComponent<EnemyMovementStateMachine>();

    }

    private void Update()
    {
        if (_emsm.enemyStatus.currentHealth <= 0)
        {
            Destroy(this);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Weapon Collider")
        {
            isCollidingWithWeapon = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.name == "Weapon Collider")
        {
            isCollidingWithWeapon = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.name == "Weapon Collider" && _pmsm.isAttacking)
        {
            isCollidingWithWeapon = false;
            _emsm.ChangeState(_emsm.hurtState);
        }
    }
}
