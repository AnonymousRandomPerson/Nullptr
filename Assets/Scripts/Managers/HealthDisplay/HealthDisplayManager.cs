using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Managers
{

    class HealthDisplayManager : MonoBehaviour
    {
        public static HealthDisplayManager Instance;
        [SerializeField]
        private HealthDisplay leftHealthDisplay;
        [SerializeField]
        private HealthDisplay rightHealthDisplay;

        // Use this for initialization
        void Start()
        {
            Instance = this;
            SetLeftEntity(FindObjectOfType<PlayerManager>().GetPlayer());
        }

        public void SetLeftEntity(Entity e)
        {
            leftHealthDisplay.Entity = e;
        }

        public void SetRightEntity(Entity e)
        {
            rightHealthDisplay.gameObject.SetActive(true);
            rightHealthDisplay.Entity = e;
        }
    }
}
