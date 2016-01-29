using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Enemy.Enemies
{
    class SimpleShooterEnemy : Enemy
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private float range;

        private GameObject player;
        private Managers.BulletManager bulletManager;

        public override void InitData()
        {
            base.InitData();
            player = FindObjectOfType<Player.Player>().gameObject;
            bulletManager = FindObjectOfType<Managers.BulletManager>();
        }

        public override void RunEntity()
        {
            base.RunEntity();
            transform.Translate(GetForward() * speed * Time.deltaTime);
            //if(Vector3.Distance(this.transform.position, player.transform.position) < range)
            //    bulletManager.Shoot(Enums.BulletTypes.Enemy, barrel, Enums.Direction.Right);
        }
    }
}
