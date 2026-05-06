using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Walking,
    Dash,
    
    TakeDamage,
    Death
}

public class PlayerBrain : MonoBehaviour
{
    public PlayerState currentState;

    public Health health;
    public PlayerMove playerMove;

    private GameObject prevObj;
    public GameObject idleStateObj;
    public GameObject walkingStateObj;
    public GameObject dashStateObj;
    public GameObject takeDamageStateObj;
    public GameObject deathStateObj;
    
    public Dictionary<PlayerState, GameObject> statesDict = new Dictionary<PlayerState, GameObject>();
    
    public TMP_Text stateText;

    void Start()
    {
        statesDict.Add(PlayerState.Idle, idleStateObj);
        statesDict.Add(PlayerState.Walking, walkingStateObj);
        statesDict.Add(PlayerState.Dash, dashStateObj);
        statesDict.Add(PlayerState.TakeDamage, takeDamageStateObj);
        statesDict.Add(PlayerState.Death, deathStateObj);
        
        ChangeState(PlayerState.Idle);
    }

    void Awake()
    {
        playerMove.AnnounceMoveVector += SetMovement;
        playerMove.AnnounceDash += SetDash;
    }

    private void SetMovement(Vector3 aObj)
    {
        if(aObj == Vector3.zero)
            ChangeState(PlayerState.Idle);
        else
            ChangeState(PlayerState.Walking);
    }

    private void SetDash(bool dashing)
    {
        if(dashing)
            ChangeState(PlayerState.Dash);
    }

    public void ChangeState(PlayerState newState)
    {
        if (statesDict.TryGetValue(newState, out GameObject stateObj))
        {
            if (prevObj != null)
            {
                PlayerStateBase state = prevObj.GetComponent<PlayerStateBase>();
                state.ExitState();
                prevObj.SetActive(false);
            }
            
            stateObj.SetActive(true);
            prevObj = stateObj;
            // Get the state script and run StartState
            PlayerStateBase stateScript = stateObj.GetComponent<PlayerStateBase>();
            stateScript.EnterState();

            currentState = newState;
        }
    }
}
