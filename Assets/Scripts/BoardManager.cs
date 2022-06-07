using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    int width = 2; //The board starts in -1 and adds up until 2 so we can have 3 rolls of width
    int height = 2; //The board starts in -1 and adds up until 2 so we can have 3 rolls of height
    int currentCreatedPiece;//This variable is used to count the pieces created and add each one to the "Space" array

    public GameObject emptySpacePrefab;

    public Space[] spaces = new Space[9];
    public int availableSpaces;

    public AIManager machineManager;
    public GameManager gameManager;

    void Start()
    {
        machineManager = FindObjectOfType<AIManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void BoardSetup(GameManager gameManager)
    {
        for (int j = -1; j < width; j++)
        //Since this int is used as the empty space initial position, I decided to start on negative 1 so
        //that the board would be centralized to it's parent.
        //Another solution would be defining an offset for the parent of the board.
        {
            for (int i = -1; i < height; i++)
            //Read comment above about why starting on a negative Int
            {
                Vector2 tempPos = new Vector2(i, j);

                GameObject newSpace = Instantiate(emptySpacePrefab, tempPos, Quaternion.identity);

                newSpace.transform.parent = transform;
                spaces[currentCreatedPiece] = newSpace.GetComponent<Space>();
                spaces[currentCreatedPiece].gameManager = gameManager;
                currentCreatedPiece++;
            }
        }
    }

    public bool CheckForWinners()
    {
        int i = 0; //This integer is used as base for checking results.
        //Instead of each "for" creating a new instance for the base verification
        //they re-use and overwrite an existing memory space

        //Check results on Horizontal
        for (i = 0; i <= 6; i += 3)
        {
            if (!CheckValues(i, i + 1))
            {
                continue;
            }

            if (!CheckValues(i, i + 2))
            {
                continue;
            }

            return true;
        }

        //Check results on Vertical
        for (i = 0; i <= 2; i ++)
        {
            if (!CheckValues(i, i + 3))
            {
                continue;
            }
            
            if (!CheckValues(i, i + 6))
            {
                continue;
            }

            return true;
        }

        //Check results on Diagonal [left]
        if (CheckValues(0, 4) && CheckValues(0, 8))
        {
            return true;
        }

        //Check results on Diagonal [left]
        if (CheckValues(2, 4) && CheckValues(2, 6))
        {
            return true;
        }

        //Here is when the command is given back to the AI
        //If player is unable to win the game, the AI take it's turn in playing
        //If the player wins, the AI never makes it's play.
        gameManager.xTurn = !gameManager.xTurn;
        machineManager.canPlay = true;

        return false;
    }

    private bool CheckValues(int firstIndex, int secondIndex)
    {
        string firstChild = spaces[firstIndex].childEnabled;
        string secondChild = spaces[secondIndex].childEnabled;

        if (firstChild == "" || secondChild == "")
        {
            return false;
        }

        if (firstChild == secondChild &&
            firstChild != null && secondChild != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Reset()
    {
        foreach(Space space in spaces)
        {
            space.ResetSpace();
        }
    }

    public void AvailableSpaces()
    {
        availableSpaces = 0;

        foreach (Space script in spaces)
        {
            if (script.childEnabled == null ||
                script.childEnabled == "")
            {
                availableSpaces++;
            }
        }
    }
}