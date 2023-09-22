using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    Button attack;
    public GameLoopController gameLoopController;
    public PlayerController playerController;

    private void Start()
    {
        attack = GetComponent<Button>();

        attack.gameObject.SetActive(false);

        attack.onClick.AddListener(() => AttackUI());
    }

    void AttackUI()
    {
        if (!playerController.isMoving)
        {
            Unit unit = playerController.selectedUnit.GetComponent<Unit>();
            playerController.SetAttackMode();
        }
    }

    public void ShowButton()
    {
        attack.gameObject.SetActive(true);
    }

    public void HideButton()
    {
        attack.gameObject.SetActive(false);
    }
}
