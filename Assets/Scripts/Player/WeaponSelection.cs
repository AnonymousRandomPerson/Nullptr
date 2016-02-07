using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Player
{
    /// <summary> Simple class for Player Weapon management. </summary>
    class WeaponSelection : MonoBehaviour
    {
        /// <summary> The player's currently selected weapon. </summary>
        private int currentWeapon;

        /// <summary> Buffer holding the current weapons for this level. </summary>
        private Enums.BulletTypes[] weapons;

        /// <summary> Switches one weapon left. </summary>
        internal void SwitchLeft()
        {
            currentWeapon--;
            if (currentWeapon < 0)
                currentWeapon = weapons.Length - 1;
        }

        /// <summary> Switches one weapon right. </summary>
        internal void SwitchRight()
        {
            currentWeapon++;
            if (currentWeapon >= weapons.Length)
                currentWeapon = 0;
        }

        /// <summary> Gets the current weapons from the GameManager and stores it in the buffer. </summary>
        internal void GetWeapons()
        {
            weapons = FindObjectOfType<Managers.GameManager>().Weapons;
        }

        /// <summary> Returns the currently selected weapon. </summary>
        /// <returns> The currently selected weapon as a Enums.BulletTypes. </returns>
        internal Enums.BulletTypes GetWeapon()
        {
            return weapons[currentWeapon];
        }
    }
}
