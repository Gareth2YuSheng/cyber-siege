using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public static HealthManager main;

    [Header("References")]
    [SerializeField] private ServerScript myServer;

    [Header("Attributes")]
    [SerializeField] private int serverHealth;
    private int baseHealth = 0;
    private bool isServerAlive = true;

    [Header("Events")]
    public UnityEvent onHealthChange = new UnityEvent();
    public UnityEvent onServerDeath = new UnityEvent();

    private void Awake()
    {
        if (main != null && main != this)
        {
            Destroy(this);
        }
        else
        {
            main = this;
        }
    }

    public int GetServerHealth()
    {
        return serverHealth;
    }

    public void InitServerHealth(int _health)
    {
        if (_health < 0) throw new ArgumentOutOfRangeException("Cannot Initiate Manager with Negative Health");
        baseHealth = _health;
        HealServer(_health);
    }

    public void HealServer(int amt)
    {
        if (amt < 0) throw new ArgumentOutOfRangeException("Cannot Heal Negative Health");
        // If baseHealth has not been set, means we are initialising the health
        if (baseHealth == 0)
        {
            serverHealth += amt;
            onHealthChange.Invoke();
        }
        // else if health is not full, heal
        else if (serverHealth < baseHealth)
        {
            serverHealth += amt;
            onHealthChange.Invoke();
            // Dont let server HP go above max
            if (serverHealth > baseHealth) serverHealth = baseHealth;
            // If server hp goes above 50%, switch to the health sprite
            if (serverHealth > baseHealth / 2)
            {
                myServer.UpdateHealthySprite();
            }
        }
    }

    public void DamageServer(int amt)
    {
        if (amt < 0) throw new ArgumentOutOfRangeException("Cannot Take Negative Damage");
        if (isServerAlive)
        {
            serverHealth -= amt;
            onHealthChange.Invoke();
            // If server hp drops below 50%, switch to damaged sprite
            if (serverHealth <= baseHealth / 2)
            {
                myServer.UpdateDamagedSprite();
            }
            if (serverHealth <= 0)
            {
                isServerAlive = false;
                onServerDeath.Invoke();
            }
        }
    }
}
