using UnityEngine;

namespace Munchy.Units.Buildings
{
    public class HQ : Farm
    {
        private static bool hasPlacedOne;

        [Header("HQ")]
        [SerializeField] private int startingMoney;


        protected override void Init()
        {
            base.Init();
            money = startingMoney;
        }

        protected override void Awake()
        {
            base.Awake();

            if(isLocal)
                hasPlacedOne = true;
        }

        public override bool CanSpawn()
        {
            if (!base.CanSpawn())
                return false;

            if (hasPlacedOne)
                Communication.Call(new ErrorMessage("Can only place one HQ"));

            return !hasPlacedOne;
        }
    }
}