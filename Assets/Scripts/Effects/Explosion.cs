using Assets.Scripts.Managers;

namespace Assets.Scripts.Effects
{
    class Explosion : Entity
    {
        private bool animDone;

        public override void InitData()
        {
            animDone = false;
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
