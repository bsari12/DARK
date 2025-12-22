using UnityEngine;

public class Ladders : MonoBehaviour
{
    private LaddersAbility laddersAbility;

    void OnTriggerEnter2D(Collider2D collision)
    {
        laddersAbility = collision.GetComponent<LaddersAbility>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(laddersAbility != null)
        {
            if(laddersAbility.isPermitted)
                laddersAbility.canGoOnLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(laddersAbility != null)
        {
            if(laddersAbility.isPermitted)
                laddersAbility.canGoOnLadder = false;
        }
    }



}
