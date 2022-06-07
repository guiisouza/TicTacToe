using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public BoardManager boardManager;

    public GameObject winner; //The End Game screen

    [Header("Turn Manager")]
    public bool xTurn = true;
    private int turnCounter = 0;

    [Header("Raycast Manager")]
    RaycastHit spaceHit;
    float camRayLenght = 10000f;
    int spaceMask;

    [Header("AI Manager")]
    public AIManager machineManager;
    public bool playAlone;
    public int gameMode;

    [Header("UI Manager")]
    public string victoryPlayer;
    public string victoryMachine;

    void Awake()
    {
        gameMode = PlayerPrefs.GetInt("mode");

        machineManager = FindObjectOfType<AIManager>();
        boardManager = FindObjectOfType<BoardManager>();
        boardManager.BoardSetup(this);
    }

    void Start()
    {
        spaceMask = LayerMask.GetMask("Space");

        //Simples two statements to see if it's a single player or two players mode.
        if (gameMode == 0)
        {
            playAlone = false;
        }
        if (gameMode == 1)
        {
            playAlone = true;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Player is only allowed to choose a space if it's X's turn
            if (xTurn && playAlone)
            {
              ShootRaycast();
            }

            //When selected PVP, the same raycast changes the spaces
            //But alternating between player and machine
            if (!playAlone)
            {
                ShootRaycast();
            }
        }

        //this allows the machine to work when the player selected to play against the machine
        if (playAlone)
        {
            //Although it's on Update, this is played once
            if (!xTurn)
            {
                machineManager.AvailableSpaces();
            }
        }
    }

    public void SwitchTurn()
    {
        turnCounter++;
        
        bool hasWinner = boardManager.CheckForWinners();

        if (hasWinner || turnCounter == 9)
        {
            StartCoroutine(EndGame(hasWinner));

            return;
        }
    }

    void ShootRaycast()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(camRay, out spaceHit, camRayLenght, spaceMask))
        {
            if (spaceHit.transform.tag == "EmptySpace")
            {
                spaceHit.transform.GetComponent<Space>().ChangeSpace();
            }
        }
    }

    private IEnumerator EndGame(bool checkWinner)
    //I'm using a Coroutine here so I can time the end of the game and allow to self-reset after 5 seconds.
    //This could be a function called from a button too.
    {   
        TextMeshProUGUI winnerlabel = winner.GetComponentInChildren<TextMeshProUGUI>();

        if (checkWinner)
        {
            if (xTurn)
            {
                winnerlabel.text = victoryPlayer;
            }
            if (!xTurn)
            {
                winnerlabel.text = victoryMachine;
            }
        }
        else
        {
            winnerlabel.text = "DRAW!";
        }

        winner.SetActive(true);

        WaitForSeconds wait = new WaitForSeconds(5.0f);
        yield return wait;


        boardManager.Reset();
        turnCounter = 0;
        xTurn = true;
        winner.SetActive(false);
    }
}