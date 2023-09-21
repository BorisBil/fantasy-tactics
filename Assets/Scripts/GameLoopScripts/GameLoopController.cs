using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 
/// GAME LOOP FILE
/// 

public class GameLoopController : MonoBehaviour
{
    public UnitManager unitManager;
    public EndTurn endTurn;

    public void EndPlayerTurn()
    {
        Debug.Log("Turn being ended");
        /// Clean up, lead into AI Behavior
        /// 
        StartCoroutine(timer());
    }

    public void StartPlayerTurn()
    {
        Debug.Log("Starting next turn");
        
        foreach (Unit unit in unitManager.playerUnits)
        {
            unit.actionPoints = 2;
        }


        endTurn.ShowButton();
    }

    IEnumerator timer()
    {
        yield return new WaitForSecondsRealtime(4);
        StartPlayerTurn();
    }

}
