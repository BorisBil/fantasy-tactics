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
        HideButton();
    }

    void HideButton()
    {
        endTurn.gameObject.SetActive(false);
    }

    public void ShowButton()
    {
        endTurn.gameObject.SetActive(true);
    }
}
