using UnityEngine;

public class AttackBoss : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;

    void OnTriggerEnter2D(Collider2D collision)
    {
        KnockBackAbility knockBackAbility = collision.GetComponentInParent<KnockBackAbility>();
        knockBackAbility.StartKnockBack(knockBackDuration, knockBackForce, transform.parent);

        collision.GetComponent<PlayerStats>().DamagePlayer(damage);



    }



}
