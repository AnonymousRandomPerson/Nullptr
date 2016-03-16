using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;

namespace Assets.Scripts.Platforms
{
    /// <summary>
    /// A block that explodes when destroyed with a destroyer bullet.
    /// Can be linked to other blocks to destroy them too.
    /// </summary>
    class BombBlock : MonoBehaviour, Destroyable {

        /// <summary> The blocks that will be blown up alongside the bomb block. </summary>
        public StarBlock[] chainedBlocks;

        /// <summary>
        /// Explodes when the block is destroyed.
        /// </summary>
        public void Destroy()
        {
            ExplosionManager.instance.SpawnExplosion(3, transform, Enums.Direction.None);
            foreach (StarBlock block in chainedBlocks) {
                if (block.enabled)
                {
                    block.StartCountdown();
                }
            }
        }
    }
}