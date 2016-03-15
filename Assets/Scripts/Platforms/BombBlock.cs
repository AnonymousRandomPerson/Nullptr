using UnityEngine;

namespace Assets.Scripts.Platforms
{
    /// <summary>
    /// A block that explodes when destroyed with a destroyer bullet.
    /// Can be linked to other blocks to destroy them too.
    /// </summary>
    public class BombBlock : MonoBehaviour, Destroyable {

        /// <summary> The blocks that will be blown up alongside the bomb block. </summary>
        public StarBlock[] chainedBlocks;

        /// <summary>
        /// Explodes when the block is destroyed.
        /// </summary>
        public void Destroy()
        {
            //TODO Spawn explosion.
            foreach (StarBlock block in chainedBlocks) {
                if (block.enabled)
                {
                    block.StartCountdown();
                }
            }
        }
    }
}