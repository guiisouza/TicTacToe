using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Space : MonoBehaviour
{
    public string childEnabled;

    public GameObject[] playerIcon;
        
    private MeshRenderer spaceMeshRender;

    private BoxCollider spaceCollider;

    public GameManager gameManager;

    void Start()
    {
        spaceMeshRender = GetComponent<MeshRenderer>();
        spaceCollider = GetComponent<BoxCollider>();
    }

    public void ChangeSpace()
    {
        spaceMeshRender.enabled = false;
        spaceCollider.enabled = false;

        if (gameManager.xTurn)
        {
            childEnabled = "X";
            playerIcon[0].SetActive(true);
        }
        else
        {
            childEnabled = "0";
            playerIcon[1].SetActive(true);
        }

        gameManager.SwitchTurn();
    }

    public void ResetSpace()
    {
        childEnabled = null;
        spaceMeshRender.enabled = true;
        spaceCollider.enabled = true;
        foreach(GameObject playerIcon in playerIcon)
        {
            playerIcon.SetActive(false);
        }
    }
}