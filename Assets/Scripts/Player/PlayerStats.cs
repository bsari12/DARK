using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private Player player;
    float currentHealth;
    [SerializeField] private HealthBar healthBarControl;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarControl.SetSliderValue(currentHealth, maxHealth);
    }

    public void DamagePlayer(float damage)
    {
        currentHealth -= damage;
        healthBarControl.SetSliderValue(currentHealth, maxHealth);
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
