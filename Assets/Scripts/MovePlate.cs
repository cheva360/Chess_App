using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    //Some functions will need reference to the controller
    public GameObject controller;

    //The Chesspiece that was tapped to create this MovePlate
    GameObject reference = null;

    //Location on the board
    int matrixX;
    int matrixY;

    //false: movement, true: attacking
    public bool attack = false;

    public GameObject P1;
    public GameObject P2;

    public void Start()
    {
        if (attack)
        {
            //Set to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        Game game = controller.GetComponent<Game>();
        Chessman chessman = reference.GetComponent<Chessman>();

        //Destroy the victim Chesspiece
        if (attack)
        {
            GameObject cp = game.GetPosition(matrixX, matrixY);

            if (cp.name == "white_king") game.Winner("black");
            if (cp.name == "black_king") game.Winner("white");

            //Destroy(cp);
            Debug.Log(cp);
            Debug.Log(reference);
            //when attacking make game objects tagged as "player" active
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
            
            bool isPVP = true;


            ////Set the Chesspiece's original location to be empty
            //game.SetPositionEmpty(chessman.GetXBoard(),
            //chessman.GetYBoard());

            ////Move reference chess piece to this position
            //chessman.SetXBoard(matrixX);
            //chessman.SetYBoard(matrixY);
            //chessman.SetCoords();


            ////Update the matrix
            ////game.SetPosition(reference);
            //chessman.SetMoveEnd();

            ////Switch Current Player
            //game.NextTurn();

            ////Destroy the move plates including self
            //chessman.DestroyMovePlates();

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

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }

    // player 1 or player 2 death check
    //private void FixedUpdate()
    //{
    //    // Example: Check if P1 or P2 is dead
    //    if (P1 != null)
    //    {
    //        var p1Script = P1.GetComponent<player1>();
    //        if (p1Script != null && p1Script.IsDead)
    //        {
    //            Debug.Log("Player 1 is dead!");
    //            // Handle player 1 death logic here
    //        }
    //    }

    //    if (P2 != null)
    //    {
    //        var p2Script = P2.GetComponent<player1>();
    //        if (p2Script != null && p2Script.IsDead)
    //        {
    //            Debug.Log("Player 2 is dead!");
    //            // Handle player 2 death logic here
    //        }
    //    }
    //}

}