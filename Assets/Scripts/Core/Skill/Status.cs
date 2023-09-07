using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Core.Skill
{
    public class Status
    {
        public static Status None = new Status(0);
        private float HP;
        private LinkedList<Effect> Effects;
        private event Action<Event> eventBus;
        public Status(float initialHP)
        {
            HP = initialHP;
        }

        public void ChangeHP(float delta)
        {
            HP += delta;
        }

        public float GetHP()
        {
            return HP;
        }

        public void EffectEvent(Event triggeredEvent)
        {
            eventBus?.Invoke(triggeredEvent);
        }
    }
}
