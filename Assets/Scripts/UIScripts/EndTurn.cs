using UnityEngine;
using UnityEngine.UI;

/// 
/// End turn button
/// 

public class EndTurn : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    Button endTurn;
    public GameLoopController gameLoopController;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /// Generate the button on start
    private void Start()
    {
        endTurn = GetComponent<Button>();

        endTurn.onClick.AddListener(() => EndPlayerTurn());
    }

    /// Go through the gameloop pipeline after clicking the button, then hide the button
    public void EndPlayerTurn()
    {
        gameLoopController.EndPlayerTurn();
        HideButton();
    }

    /// Hide the button
    public void HideButton()
    {
        endTurn.gameObject.SetActive(false);
    }

    /// Show the button
    public void ShowButton()
    {
        endTurn.gameObject.SetActive(true);
    }
}
