using UnityEngine;

public class DeathLogic : MonoBehaviour
{
    public string attacker;
    public string target;
    public bool attackdead;
    public bool targetdead;

    public bool attackerIsWhite;
    public GameObject targetcp;

    public void FixedUpdate()
    {
        Debug.Log(targetcp);
    }
}
