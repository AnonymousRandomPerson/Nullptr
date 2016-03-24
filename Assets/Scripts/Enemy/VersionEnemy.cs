using UnityEngine;

namespace Assets.Scripts.Enemy.Enemies
{
    /// <summary>
    /// An enemy that varies in health depending on a randomly chosen "version".
    /// </summary>
    class VersionEnemy : Enemy {

        /// <summary> The base name of the enemy. Will be suffixed by a version number upon initialization. </summary>
        private string baseName = "";
        /// <summary> The base health of the 1.0.0 version of the enemy. </summary>
        private int baseHealth = -1;
   
        /// <summary>
        /// Method to allow custom data initialization.
        /// </summary>
        public override void InitData()
        {
            if (baseName == "")
            {
                baseName = entityName;
            }
            if (baseHealth == -1)
            {
                baseHealth = totalHealth;
            }

            int version = Random.Range(0, 10);
            totalHealth = baseHealth + version;
            currentHealth = totalHealth;
            entityName = baseName + " 1." + version + ".0";
        }
    }
}