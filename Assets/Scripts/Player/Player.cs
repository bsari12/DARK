using UnityEngine;

public class Player : MonoBehaviour
{
    public GatherInput gatherInput;
    public StateMachine stateMachine;

    private BaseAbility[] playerAbilities;

    void Awake()
    {
        stateMachine = new StateMachine();
        playerAbilities =GetComponents<BaseAbility>();
        stateMachine.arrayOfAbilities = playerAbilities;
    }
    void Update()
    {
        foreach(BaseAbility ability in playerAbilities)
        {
            if(ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessAbility();
            }
        }
    }
    void FixedUpdate()
    {
        foreach(BaseAbility ability in playerAbilities)
        {
            if(ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessFixedAbility();
            }
        }    
    }
}
