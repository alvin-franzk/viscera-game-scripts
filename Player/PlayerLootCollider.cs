using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLootCollider : MonoBehaviour
{
    private PlayerMovementStateMachine _pmsm;
    public AudioClip pickupSound;
    // Start is called before the first frame update
    void Start()
    {
        _pmsm = GetComponent<PlayerMovementStateMachine>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        // Health Boost
        if (collision.name == "HealthBoost")
        {
            if (_pmsm.playerStatus.currentHealth != _pmsm.playerStatus.maxHealth)
            {
                _pmsm.playerStatus.currentHealth += _pmsm.playerStatus.maxHealth * 0.35f;
                SoundManager.instance.PlaySound(pickupSound, transform, 0.3f);
                Destroy(collision.transform.parent.gameObject);
            }
        }

        // Stamina Boost
        if (collision.name == "StaminaBoost")
        {
            if (_pmsm.playerStatus.currentStamina != _pmsm.playerStatus.maxStamina)
            {
                _pmsm.playerStatus.currentStamina += _pmsm.playerStatus.maxStamina * 0.20f;
                SoundManager.instance.PlaySound(pickupSound, transform, 0.3f);
                Destroy(collision.transform.parent.gameObject);
            }
        }

        // Health Regen
        if (collision.name == "HealthRegen")
        {
            _pmsm.playerStatus.healthRegenRate += Random.Range(0.05f, 0.2f);
            SoundManager.instance.PlaySound(pickupSound, transform, 0.3f);
            Destroy(collision.transform.parent.gameObject);
        }

        // Stamina Regen
        if (collision.name == "StaminaRegen")
        {
            _pmsm.playerStatus.staminaRegenRate += Random.Range(0.1f, 0.5f);
            SoundManager.instance.PlaySound(pickupSound, transform, 0.3f);
            Destroy(collision.transform.parent.gameObject);
        }

        // Max Health
        if (collision.name == "MaxHealth")
        {
            _pmsm.playerStatus.maxHealth += 25f;
            SoundManager.instance.PlaySound(pickupSound, transform, 0.3f);
            Destroy(collision.transform.parent.gameObject);
        }

        // Max Stamina
        if (collision.name == "MaxStamina")
        {
            _pmsm.playerStatus.maxStamina += 15f;
            SoundManager.instance.PlaySound(pickupSound, transform, 0.3f);
            Destroy(collision.transform.parent.gameObject);
        }

        // Damage Boost
        if (collision.name == "DamageBoost")
        {
            _pmsm.minAttackDamage -= Random.Range(2, 7);
            _pmsm.maxAttackDamage -= Random.Range(2, 7);
            SoundManager.instance.PlaySound(pickupSound, transform, 0.3f);
            Destroy(collision.transform.parent.gameObject);
        }

        // Crit Chance
        if (collision.name == "CritChance")
        {
            _pmsm.critChance += Random.Range(0.05f, 0.1f);
            SoundManager.instance.PlaySound(pickupSound, transform, 0.3f);
            Destroy(collision.transform.parent.gameObject);
        }

        // Crit Damage
        if (collision.name == "CritDamage")
        {
            _pmsm.critModifier += Random.Range(0.5f, 1.0f);
            SoundManager.instance.PlaySound(pickupSound, transform, 0.3f);
            Destroy(collision.transform.parent.gameObject);
        }
    }
}
