using System.Threading;
using UnityEngine;

public class SwingBlade : MonoBehaviour
{
    [Header("Swing Settings")]
    [SerializeField] private float maxAngle;
    [SerializeField] private float speed;
    private float timer;

    private int pushDirection = 1;
    private float previousAngle = 0f;


    [Header("KnockBack Settings")]
    [SerializeField] private float bladeDamage;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;

    void Update()
    {
        timer += speed* Time.deltaTime;
        float angle = maxAngle * Mathf.Sin(timer);
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        if(angle > previousAngle)
            pushDirection =1;
        else if(angle<previousAngle)
            pushDirection = -1;
        
        previousAngle = angle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockBackAbility knockBackAbility = collision.GetComponentInParent<KnockBackAbility>();
        knockBackAbility.StartSwingKnockBack(knockBackDuration, knockBackForce, pushDirection);

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        playerStats.DamagePlayer(bladeDamage);
    }
}
