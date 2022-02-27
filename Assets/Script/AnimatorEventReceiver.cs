using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventReceiver : MonoBehaviour
{
    public event Action<AnimationEvent> MakeMeleeDamage;

    protected virtual void OnMakeMeleeDamage(AnimationEvent obj)
    {
        MakeMeleeDamage?.Invoke(obj);
    }
}