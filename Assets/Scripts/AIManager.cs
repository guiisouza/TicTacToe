using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public BoardManager boardManager;
    public GameManager gameManager;

    Space[] tempSpaces;
    int currentSelectedSlot;
    public int randomSpace; //I left public for debug purposes

    public bool canPlay; //This variable needs to be public to allow BoardManager to change if the machine can play or not

    void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void AvailableSpaces()
    {
        if (canPlay)
        {
            //When it's the machine turn, this scripts asks for the boardManager
            //to count all the spaces still available for play.
            boardManager.AvailableSpaces();

            //Reseting all parameters before filling up the array
            currentSelectedSlot = 0;

            //Here we are rebuilding the array with a new custom size basead on boardManager reply
            tempSpaces = new Space[boardManager.availableSpaces];

            //After rebuilding the array we start filling the array with all the spaces available.
            foreach (Space script in boardManager.spaces)
            {
                //I choose to check the spaces for selected children
                //But I could check if the space is "selectable" trhough it's boxCollider if it's enabled or not
                if (script.childEnabled == null ||
                    script.childEnabled == "")
                {
                    //If the space is available, it's added to the list
                    //currentSelectedSlot is the position to be filled up
                    tempSpaces[currentSelectedSlot] = script;

                    if (currentSelectedSlot < boardManager.availableSpaces)
                    {
                        currentSelectedSlot++;
                    }
                    else
                    {
                        currentSelectedSlot = 0;
                    }
                }
            }
            
            //This condition allows the machine to play only when spaces are available
            if (tempSpaces.Length > 0)
            {
                randomSpace = Random.Range(0, tempSpaces.Length);
                tempSpaces[randomSpace].ChangeSpace();
            }

            canPlay = false;
        }
    }
}