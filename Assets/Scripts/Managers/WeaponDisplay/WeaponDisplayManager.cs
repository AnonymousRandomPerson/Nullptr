using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Managers
{
    class WeaponDisplayManager : MonoBehaviour
    {
        public WeaponDisplayManager Instance { get; private set; }

        [SerializeField]
        private WeaponDisplay weaponDisplay;
        [SerializeField]
        private PlayerManager playerManager;

        Player.Player player;

        public WeaponDisplay WeaponDisplay { get { return weaponDisplay; } }

        // Use this for initialization
        void Start()
        {
            this.Instance = this;
            this.player = playerManager.GetPlayer();
        }

        // Update is called once per frame
        void Update()
        {
            weaponDisplay.WeaponText = player.WeaponName;
        }
    }
}
