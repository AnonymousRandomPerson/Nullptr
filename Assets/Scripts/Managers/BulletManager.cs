using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Managers
{
    public class BulletManager : EntityManager
    {
        public bool Shoot(Transform loc, Enums.BulletTypes type)
        {
            return AquireEntity(loc, (int)type);
        }
    }
}
