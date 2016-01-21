using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts
{
    class test : MonoBehaviour
    {
        public Managers.BulletManager bulletManager;
        public Transform barrel;
        void Update()
        {
            if(CustomInput.BoolFreshPress(CustomInput.UserInput.Attack))
            {
                bulletManager.Shoot(Enums.BulletTypes.Player, barrel, Enums.Direction.Right);
            }
        }
    }
}
