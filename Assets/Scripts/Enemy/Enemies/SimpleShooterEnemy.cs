using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Enemy.Enemies
{
    class SimpleShooterEnemy : Enemy
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private float wait;
        [SerializeField]
        private Transform barrel;
        [SerializeField]
        private float LifeTime;
        [SerializeField]
        private Enums.BulletTypes bullet;

        private float currWait;
        private float currLifeTime;
        private GameObject player;
        private Managers.BulletManager bulletManager;

        public override void InitData()
        {
            base.InitData();
            player = FindObjectOfType<Managers.PlayerManager>().GetPlayer().gameObject;
            bulletManager = FindObjectOfType<Managers.BulletManager>();
            currWait = 0;
            currLifeTime = 0;
        }

        public override void RunEntity()
        {
            base.RunEntity();
            transform.Translate(GetForward() * speed * Time.deltaTime);
            if ((currWait += Time.deltaTime) > wait)
            {
                bulletManager.Shoot(bullet, barrel, Enums.Direction.Down);
                currWait = 0;
            }
            currLifeTime += Time.deltaTime;
            if (currLifeTime > LifeTime)
                Die();
        }
    }
}
