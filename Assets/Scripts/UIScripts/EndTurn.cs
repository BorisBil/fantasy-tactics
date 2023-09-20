using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour
{
    Button endTurn;
    public GameLoopController gameLoopController;

    private void Start()
    {
        endTurn = GetComponent<Button>();

        endTurn.onClick.AddListener(() => EndPlayerTurn());
    }

    public void EndPlayerTurn()
    {
        gameLoopController.EndPlayerTurn();
        Debug.Log("Ending the turn");
        HideButton();
        ShowButton();
    }

    void HideButton()
    {
        endTurn.gameObject.SetActive(false);
    }

    public void ShowButton()
    {
        Debug.Log("Showing button");
        endTurn.gameObject.SetActive(true);
    }
}
