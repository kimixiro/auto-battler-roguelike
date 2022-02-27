using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiItems : MonoBehaviour
{
    public List<UIItemInfo> items;
    public Text text;
    

    public void Fill(List<GameObject> units,int maxCount)
    {
   /*     var Bow = 0;
        var Wizard = 0;
        var SwordShield = 0;
        var DoubleSword = 0;

        text.text = units.Count.ToString() + " / " + maxCount;

        foreach (var unit in units)
        {

            var u = unit.GetComponent<UnitSystem>();
            
            if (u.UnitConfig.ClassUnit == ClassUnit.Bow)
                Bow++;
            if (u.UnitConfig.ClassUnit == ClassUnit.Wizard)
                Wizard++;
            if (u.UnitConfig.ClassUnit == ClassUnit.SwordShield)
                SwordShield++;
            if (u.UnitConfig.ClassUnit == ClassUnit.DoubleSword)
                DoubleSword++;

        }

        foreach (var item in items)
        {
            if (item.ClassUnit == ClassUnit.Bow)
                item.Fill(Bow);
            if (item.ClassUnit == ClassUnit.Wizard)
                item.Fill(Wizard);
            if (item.ClassUnit == ClassUnit.SwordShield)
                item.Fill(SwordShield);
            if (item.ClassUnit == ClassUnit.DoubleSword)
                item.Fill(DoubleSword);
        }*/
        
    }
}
