using UnityEngine;

public class DeathLogic : MonoBehaviour
{
    public string attacker;
    public string target;
    public bool attackdead;
    public bool targetdead;

    public bool attackerIsWhite;
    public GameObject targetcp;
    public int targetx;
    public int targety;

    public void FixedUpdate()
    {
        //Debug.Log(targetcp);
        //Debug.Log(attackerIsWhite);
        //Debug.Log(attackdead);
        //Debug.Log(targetdead);
    }
}
