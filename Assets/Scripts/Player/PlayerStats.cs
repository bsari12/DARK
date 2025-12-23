using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private Player player;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void DamagePlayer(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            if(player.stateMachine.currentState != PlayerStates.State.KnockBack)
                Debug.Log("Player is Dead");
        }
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
