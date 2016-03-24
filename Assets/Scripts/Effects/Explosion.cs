using UnityEngine;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Effects
{
    class Explosion : Entity
    {
        [SerializeField]
        private Util.SoundPlayer sound;

        private bool animDone;

        public override void InitData()
        {
            animDone = false;
            sound.PlaySong(0);
        }

        public override void RunEntity()
        {
            if (animDone)
                Die();
        }

        public override void HitByEntity(Entity col)
        {
        }

        public void AnimDetector()
        {
            animDone = true;
        }
    }
}
