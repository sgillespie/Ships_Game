using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private float rotateSpeed = 60f;
    private int moveSpeed = 1;
    public int moveNum;

    public int playerTurn = 1;

    public boat2[] boats;
    public button[] buttons;
    public shooting_2[] shots;

    public List<string> movesShip1;
    public List<string> movesShip2;

    public List<int> shotsShip1;
    public List<int> shotsShip2;

    public float degree;
    private float gridSize = 1f;

    private void Start()
    {
        buttons = FindObjectsOfType<button>();
        boats = FindObjectsOfType<boat2>();
        shots = FindObjectsOfType<shooting_2>();
        shotsShip1 = shots[0].shots;
        shotsShip2 = shots[1].shots;
        movesShip1 = boats[0].moves;
        movesShip2 = boats[1].moves;
        moveNum = 0;

        StartCoroutine(LoopTurns());
    }

    IEnumerator LoopTurns() {
        while (true) {
            if (playerTurn == 3)
            {

                if (moveNum <= 3)
                {
                    if(Vector2.Distance(boats[0].transform.position, boats[0].pos) == 0f &&
                       Vector2.Distance(boats[1].transform.position, boats[1].pos) == 0.0f &&
                       boats[0].transform.rotation == Quaternion.Euler(0, 0, boats[0].degree) &&
                       boats[1].transform.rotation == Quaternion.Euler(0, 0, boats[1].degree))
                    {
                        ResolveMoves(moveNum);
                        ResolveShots(moveNum);
                        moveNum += 1;
                    }
                
                }
                else
                {
                    moveNum = 0; // 3 Moves complete, we end the turn and reset
                    EndTurn();
                    yield return new WaitForSeconds(1f);
                }            
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    EndTurn();
                    yield return new WaitForSeconds(1f);
                }
            }

            yield return null;
        }
    }

    void EndTurn()
    {
        if (playerTurn == 1)
        {
            boats[0].ResolveButtonMovements();
            shots[0].ResolveToggleShots();
            for(int i = 0; i < 4; i++)
            {
                buttons[i].Reset();
            }
            playerTurn = 2;    
        }
        else if (playerTurn == 2)
        {
            boats[1].ResolveButtonMovements();
            shots[1].ResolveToggleShots();
            for (int i = 0; i < 4; i++)
            {
                buttons[i].Reset();
            }
            playerTurn = 3;
        }
        else if (playerTurn == 3)
        {
            boats[0].moves = new List<string>();
            boats[1].moves = new List<string>();
            movesShip1 = boats[0].moves;
            movesShip2 = boats[1].moves;
            boats[0].availableMoves = 4;
            boats[1].availableMoves = 4;
            boats[0].turnOver = false;
            boats[1].turnOver = false;
            playerTurn = 1;  
        }
        
    }
    void MoveShip1(string move)
    {
        {
            if (move == "forward")
            {
                boats[0].pos += -boats[0].transform.up * 1f;
            }
            if (move == "right")
            {
                boats[0].pos += -boats[0].transform.up * 1f;
                boats[0].pos += -boats[0].transform.right * gridSize;
                boats[0].degree = Mathf.Repeat(boats[0].degree - 90f, 360f);
            }
            if (move == "left")
            {
                boats[0].pos += -boats[0].transform.up * 1f;
                boats[0].pos += boats[0].transform.right * gridSize;
                boats[0].degree = Mathf.Repeat(boats[0].degree + 90f, 360f);
            }
            boats[0].transform.position = Vector3.MoveTowards(boats[0].transform.position, boats[0].pos, Time.deltaTime * moveSpeed);
            boats[0].transform.rotation = Quaternion.RotateTowards(boats[0].transform.rotation, Quaternion.Euler(0, 0, boats[0].degree), Time.deltaTime * rotateSpeed);
        }
        
    }

    void MoveShip2(string move)
    {   
        {
            if (move == "forward")
            {
                boats[1].pos += -boats[1].transform.up * 1f;
            }
            if (move == "right")
            {
                boats[1].pos += -boats[1].transform.up * 1f;
                boats[1].pos += -boats[1].transform.right * gridSize;
                boats[1].degree = Mathf.Repeat(boats[1].degree - 90f, 360f);
            }
            if (move == "left")
            {
                boats[1].pos += -boats[1].transform.up * 1f;
                boats[1].pos += boats[1].transform.right * gridSize;
                boats[1].degree = Mathf.Repeat(boats[1].degree + 90f, 360f);
            }
            boats[1].transform.position = Vector3.MoveTowards(boats[1].transform.position, boats[1].pos, Time.deltaTime * moveSpeed);
            boats[1].transform.rotation = Quaternion.RotateTowards(boats[1].transform.rotation, Quaternion.Euler(0, 0, boats[1].degree), Time.deltaTime * rotateSpeed);
        }
    }

    void ShootShip1(int side)
    {
        if (side == 1)
        {
            shots[0].Shoot_Portside();
        }
        if (side == 2)
        {
            shots[0].Shoot_Starboard();
        }
        else { 
        }
    }

    void ShootShip2(int side)
    {
        if (side == 1)
        {
            shots[1].Shoot_Portside();
        }
        if (side == 2)
        {
            shots[1].Shoot_Starboard();
        }
        else
        {
            
        }
    }
    void ResolveMoves(int i )
    {
        MoveShip1(movesShip1[i]);
        MoveShip2(movesShip2[i]);
        
    }
    void ResolveShots(int i)
    {
        ShootShip1(shotsShip1[i]);
        ShootShip2(shotsShip2[i]);
    }
}
