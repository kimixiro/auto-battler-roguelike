using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitSlot : MonoBehaviour
{
   public Image icon;
   [SerializeField]
   private ClassUnit _slotUnit;
   [SerializeField]
   private UISelectUinit _selecUI;

   public Sprite bowSprite;
   public Sprite wizardSprite;
   public Sprite swordShieldSprite;
   public Sprite doubleSwordSprite;

   public void Fill(ClassUnit classUnit, UISelectUinit selectUinit)
   {
      _selecUI = selectUinit;
      _slotUnit = classUnit;

      switch (classUnit)
      {
         case ClassUnit.None:
            break;
         case ClassUnit.Bow:
            icon.sprite = bowSprite;
            break;
         case ClassUnit.Wizard:
            icon.sprite = wizardSprite;
            break;
         case ClassUnit.SwordShield:
            icon.sprite = swordShieldSprite;
            break;
         case ClassUnit.DoubleSword:
            icon.sprite = doubleSwordSprite;
            break;
         default:
            throw new ArgumentOutOfRangeException(nameof(classUnit), classUnit, null);
      }
   }

   public void SendChoice()
   {
      _selecUI.SelectReward(_slotUnit);
   }
   
}
