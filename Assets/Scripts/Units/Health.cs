using System;

namespace Munchy.Units
{
    public class Health
    {
        public int maxHealth;
        public int health { get; private set; }
        public bool dead { get; private set; }

        public Action onKilled;
        public Action onInjured;

        public Health(int maxHealth)
        {
            this.maxHealth = maxHealth;
            this.health = maxHealth;

        }

        public bool TakeDamage(int damage)
        {
            if (dead) return false;

            health -= damage;
            if (health <= 0)
            {
                onKilled?.Invoke();
                dead = true;
                return true;
            }

            onInjured?.Invoke();
            return false;
        }
    }
}