using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private Player player;
    float currentHealth;
    [SerializeField] private HealthBar healthBarControl;


    [Header("Flash")]
    [SerializeField] private float flashDuration;
    [SerializeField, Range(0,1)] private float flashStrength;
    [SerializeField] private Color flashCol;
    [SerializeField] private Material flashMaterial;
    private Material defaultMaterial;
    private SpriteRenderer spriter;
    private bool canTakeDamage = true;

    [Header("StatsColliders")]
    [SerializeField] private Collider2D standingStatsCol;
    [SerializeField] private Collider2D crouchStatsCol;
    private Collider2D currentStatsCol;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarControl.SetSliderValue(currentHealth, maxHealth);
        spriter = GetComponentInParent<SpriteRenderer>();
        defaultMaterial = spriter.material;
    }

    public void DamagePlayer(float damage)
    {
        if(canTakeDamage == false)
	        return;
        currentHealth -= damage;
        healthBarControl.SetSliderValue(currentHealth, maxHealth);
        StartCoroutine(Flash());
        if(currentHealth <= 0)
        {
            if(player.stateMachine.currentState != PlayerStates.State.KnockBack)
                Debug.Log("Player is Dead");
        }
    }
    private IEnumerator Flash()
    {
        canTakeDamage = false;
        spriter.material = flashMaterial;
        flashMaterial.SetColor("_FlashColor", flashCol);
        flashMaterial.SetFloat("_FlashAmount", flashStrength);
        yield return new WaitForSeconds(flashDuration);
        spriter.material = defaultMaterial;
        if(currentHealth>0)
            canTakeDamage = true;
    }

    public void EnableStatsStandCol()
    {
        if(currentHealth <=0)
            return;
        crouchStatsCol.enabled = false;
        standingStatsCol.enabled = true;
        currentStatsCol = standingStatsCol;

    }

    public void EnableStatsCrouchCol()
    {
        if(currentHealth <=0)
            return;
        crouchStatsCol.enabled = true;
        standingStatsCol.enabled = false;
        currentStatsCol = crouchStatsCol;

    }


    public bool GetCanTakeDamage()
    {
        return canTakeDamage;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
