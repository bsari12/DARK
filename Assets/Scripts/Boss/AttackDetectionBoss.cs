using UnityEngine;

public class AttackDetectionBoss : MonoBehaviour
{
    [SerializeField] private BossPhysics bossPhysics;

    void OnTriggerEnter2D(Collider2D collision)
    {
        bossPhysics.inAttackRange = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        bossPhysics.inAttackRange = false;
    }
}
