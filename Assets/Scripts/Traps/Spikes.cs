using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float spikeDamage;

    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;

    void OnTriggerEnter2D(Collider2D collision)
    {
        KnockBackAbility knockBackAbility = collision.GetComponentInParent<KnockBackAbility>();
        StartCoroutine(knockBackAbility.KnockBack(knockBackDuration, knockBackForce, transform));


        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        playerStats.DamagePlayer(spikeDamage);
    }

}
