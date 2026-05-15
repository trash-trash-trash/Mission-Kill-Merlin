using UnityEngine;

public class Player : CharacterBase
{
    public Transform playerBody;
    
    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}