using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    private float HP;
    private LinkedList<Effect> Effects;
    public Status(float initialHP)
    {
        HP = initialHP;
    }

    public void ChangeHP(float delta)
    {
        HP += delta;
    }

    public void AddEffect(Effect effect)
    {
        Effects.AddLast(effect);
    }

    public float GetHP()
    {
        return HP;
    }

    public void EffectEvent(Event triggeredEvent)
    {
        
    }
}
