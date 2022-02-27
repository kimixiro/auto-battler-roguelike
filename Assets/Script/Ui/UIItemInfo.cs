using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemInfo : MonoBehaviour
{
   public ClassUnit ClassUnit;
   public Image icon;
   public Text counter;
   public int countInt;
   public List<Sprite> _sprite;

   public void Fill(int count)
   {
      if (count >= _sprite.Count)
         count = _sprite.Count;
      counter.text = count.ToString();
     // icon.sprite = _sprite[count];
   }
}
