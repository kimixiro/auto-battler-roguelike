using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectUinit : MonoBehaviour
{
    public GameObject window;
    public UiItems items;
    public GameBehaviour gameBehaviour;

    public List<UIUnitSlot> slots;

    public void ShowSelectUnit()
    {
        window.SetActive(true);
        foreach (var slot in slots)
        {
            slot.Fill(GanerateReward(),this);
        }
        Debug.Log("win");
    }


    public void SelectReward(ClassUnit classUnit)
    {
        window.SetActive(false);
        if(gameBehaviour.data.maxSlotUnit <= gameBehaviour.poolUnit.Count) 
            return;
        gameBehaviour.AddUnit(classUnit);
        items.Fill(gameBehaviour.poolUnit,gameBehaviour.data.maxSlotUnit);
       
    }

    ClassUnit GanerateReward()
    {
        return (ClassUnit)Random.Range(1, 4);
    }

    public void ReRoll()
    {
        foreach (var slot in slots)
        {
            slot.Fill(GanerateReward(),this);
        }
    }
    
    
}
