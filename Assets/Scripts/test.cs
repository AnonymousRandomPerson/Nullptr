using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts
{
    class test : MonoBehaviour
    {
        public Managers.ExplosionManager mngr;

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.T))
                mngr.SpawnExplosion(1, this.transform, Enums.Direction.None);
            if (Input.GetKeyUp(KeyCode.P))
                mngr.SpawnExplosion(2, this.transform, Enums.Direction.None);
        }
    }
}
