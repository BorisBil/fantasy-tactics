using UnityEngine;
using UnityEngine.UI;

/// 
/// Attack button
/// 

public class Attack : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    Button attack;
    public GameLoopController gameLoopController;
    public PlayerController playerController;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /// Generate the button on start
    private void Start()
    {
        attack = GetComponent<Button>();

        attack.gameObject.SetActive(false);

        attack.onClick.AddListener(() => AttackUI());
    }

    /// Changes the player's control state from movement to attacking
    void AttackUI()
    {
        if (!playerController.isMoving)
        {
            Unit unit = playerController.selectedUnit.GetComponent<Unit>();
            playerController.SetAttackMode();
        }
    }

    /// Show the button
    public void ShowButton()
    {
        attack.gameObject.SetActive(true);
    }

    /// Hide the button
    public void HideButton()
    {
        attack.gameObject.SetActive(false);
    }
}
