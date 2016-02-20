using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Managers
{
    public class BulletManager : EntityManager
    {
        public bool Shoot(Enums.BulletTypes type, Transform loc, Enums.Direction direction, bool isCutScene = false)
        {
            try
            {
                if (isCutScene)
                    return AquireEntityCutScene((int)type, loc, direction);
                return AquireEntity((int)type, loc, direction);
            } catch (System.IndexOutOfRangeException ex)
            {
                Debug.LogError("Error: Invalid Bullet Type. : " + ex.Message);
                return false;
            }
        }
    }
}
