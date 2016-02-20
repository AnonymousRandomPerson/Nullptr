using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{

    class HealthDisplay : MonoBehaviour
    {
        [SerializeField]
        private Text title; //Name of the entity
        [SerializeField]
        private Text fraction; //Number display on the bars
        [SerializeField]
        private Transform under; //The bar you see under
        [SerializeField]
        private Transform over; //The bar you see over

        public Entity Entity { set; get; }

        void Update()
        {
            if (Entity == null || Entity.CurrentHealth == 0)
            {
                this.gameObject.SetActive(false);
            } else
            {
                gameObject.SetActive(true);
                title.text = Entity.Name;
                SetHealth(Entity.CurrentHealth, Entity.TotalHealth);
            }
        }

        void SetHealth(int current, int total)
        {
            this.fraction.text = string.Format("{0}/{1}", current, total);
            this.over.localScale = new Vector2((current + 0.0f) / total, 1);
        }
    }
}
