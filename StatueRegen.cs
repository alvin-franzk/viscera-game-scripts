using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueRegen : MonoBehaviour
{
    public RegenType regenType;
    public enum RegenType { Health, Stamina }
    public float radius;
    public bool playerIsNear;

    private LayerMask playerLayerMask;
    private PlayerMovementStateMachine _pmsm;
    // Start is called before the first frame update
    void Start()
    {
        radius = 3f;
        playerLayerMask = LayerMask.GetMask("Player");
        _pmsm = GameObject.Find("Player Viscera").GetComponent<PlayerMovementStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        playerIsNear = Physics.CheckSphere(transform.position, radius, playerLayerMask);

        if (playerIsNear)
        {
            HandleRegen(regenType);
        }
    }

    private void HandleRegen(RegenType regen)
    {
        switch (regen)
        {
            case RegenType.Health:
                var valueH = _pmsm.playerStatus.maxHealth * 0.05f;
                _pmsm.playerStatus.currentHealth += valueH * Time.deltaTime;
                if (_pmsm.playerStatus.currentHealth > _pmsm.playerStatus.maxHealth) { _pmsm.playerStatus.currentHealth = _pmsm.playerStatus.maxHealth; }
                break;
            case RegenType.Stamina:
                var valueS = _pmsm.playerStatus.maxStamina * 0.1f;
                _pmsm.playerStatus.currentStamina += valueS * Time.deltaTime;
                if (_pmsm.playerStatus.currentStamina > _pmsm.playerStatus.maxStamina) { _pmsm.playerStatus.currentStamina = _pmsm.playerStatus.maxStamina; }
                break;
        }
    }
}
