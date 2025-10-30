using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    //Some functions will need reference to the controller
    public GameObject controller;
    private DeathLogic deathLogic = null;

    //The Chesspiece that was tapped to create this MovePlate
    private GameObject reference = null;
    private GameObject cp = null;
    private GameObject targetref = null;

    //Location on the board
    int matrixX;
    int matrixY;

    //false: movement, true: attacking
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            //Set to red
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }

        if (controller == null)
            controller = GameObject.FindGameObjectWithTag("GameController");

        if (controller != null)
            deathLogic = controller.GetComponent<DeathLogic>();
    }

    public void OnMouseUp()
    {
        if (controller == null)
            controller = GameObject.FindGameObjectWithTag("GameController");
        Game game = controller.GetComponent<Game>();
        Chessman chessman = reference.GetComponent<Chessman>();
        deathLogic = controller.GetComponent<DeathLogic>();


        //Destroy the victim Chesspiece
        if (attack)
        {
            cp = game.GetPosition(matrixX, matrixY);

            if (cp != null)
            {
                //if (cp.name == "white_king") game.Winner("black");
                //if (cp.name == "black_king") game.Winner("white");


                if (cp.name.Contains("white"))
                {
                    if (deathLogic == null && controller != null) deathLogic = controller.GetComponent<DeathLogic>();
                    if (deathLogic != null) deathLogic.attackerIsWhite = false;
                }
                else if (cp.name.Contains("black"))
                {
                    if (deathLogic == null && controller != null) deathLogic = controller.GetComponent<DeathLogic>();
                    if (deathLogic != null) deathLogic.attackerIsWhite = true;
                }
                deathLogic.targetcp = cp;
                // Immediately clear the board matrix slot and destroy the victim now.
                // Destroy marks the object for destruction at end of frame; after Destroy the
                // Unity overloads will make the reference compare equal to null.
                //game.SetPositionEmpty(matrixX, matrixY);
                //Destroy(cp);
            }
            foreach (var obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.CompareTag("Player"))
                {
                    obj.SetActive(true);
                }
                if (obj.tag == "Untagged" && obj.scene.IsValid() && obj.activeInHierarchy)
                {
                    obj.SetActive(false);
                }

            }

            ////Set the Chesspiece's original location to be empty
            //game.SetPositionEmpty(chessman.GetXBoard(),
            //chessman.GetYBoard());

            ////Move reference chess piece to this position
            //chessman.SetXBoard(matrixX);
            //chessman.SetYBoard(matrixY);
            //chessman.SetCoords();


            //////Update the matrix
            ////game.SetPosition(reference);
            //chessman.SetMoveEnd();

            ////Switch Current Player
            //game.NextTurn();

            ////Destroy the move plates including self
            //chessman.DestroyMovePlates()
        }
        else
        {
            //Set the Chesspiece's original location to be empty
            game.SetPositionEmpty(chessman.GetXBoard(),
            chessman.GetYBoard());

            //Move reference chess piece to this position
            chessman.SetXBoard(matrixX);
            chessman.SetYBoard(matrixY);
            chessman.SetCoords();

            //Update the matrix
            game.SetPosition(reference);
            chessman.SetMoveEnd();

            //Switch Current Player
            game.NextTurn();

            //Destroy the move plates including self
            chessman.DestroyMovePlates();
        }
    }

    public void Update()
    {
        // Debug friendly: print names instead of object references (safe if null)
        if (controller == null)
            controller = GameObject.FindGameObjectWithTag("GameController");

        if (deathLogic == null && controller != null)
            deathLogic = controller.GetComponent<DeathLogic>();

        Game game = controller.GetComponent<Game>();
        Chessman chessman = reference.GetComponent<Chessman>();



        if (deathLogic != null && targetref != null)
        {
            if (deathLogic.attackdead)
            {
                if (deathLogic.attackerIsWhite)
                {


                    Destroy(targetref);

                }
                else
                {
                    Destroy(deathLogic.targetcp);

                }

                deathLogic.attackdead = false;
            }

            if (deathLogic.targetdead)
            {
                if (deathLogic.attackerIsWhite)
                {
                    Destroy(deathLogic.targetcp);


                }
                else
                {
                    Destroy(targetref);

                }

                deathLogic.targetdead = false;
            }
        }
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    // IMPORTANT: set targetref here so it exists immediately after SetReference is called
    public void SetReference(GameObject obj)
    {
        reference = obj;
        targetref = obj; // <-- assign immediately to avoid timing/race issues
    }

    public GameObject GetReference()
    {
        return reference;
    }
}