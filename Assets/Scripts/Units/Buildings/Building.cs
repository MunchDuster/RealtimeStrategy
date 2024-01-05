using System.Linq;
using UnityEngine;

namespace Munchy.Units.Buildings
{
    public abstract class Building : Unit
    {
        [Header("Building")]
        public int[] upgradeCosts = {50, 200};

        

        protected override void Awake()
        {
            base.Awake();

            // Payment here as entities will be payed for when put in queue
            if(isLocal) // You build it you pay for it
                money -= cost;
        }

        protected override void Init()
        {
            base.Init();
            Communication.Listen((LocalUnitCreatedMessage m) => UpdateFood());
            Communication.Listen((LocalUnitDestroyedMessage m) => UpdateFood());
        }

        private static void UpdateFood()
        {
            foodUsed = LocalUnits.Sum(unit => unit.foodCost);
            food = LocalUnits.FindAll(unit => unit as Farm != null).Sum(farmUnit => ((Farm)farmUnit).foodStored);
            Communication.Call(new FoodChangedMessage(food, foodUsed));
        }
    }
}