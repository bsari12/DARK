using Unity.VisualScripting;
using UnityEngine;

public class RotatingBlade : MonoBehaviour
{
    [SerializeField] private float bladeDamage;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;

    void Update()
    {
        transform.Rotate(0,0,rotationSpeed* Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockBackAbility knockBackAbility = collision.GetComponentInParent<KnockBackAbility>();
        knockBackAbility.StartKnockBack(knockBackDuration, knockBackForce, transform);

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        playerStats.DamagePlayer(bladeDamage);
    }
}
