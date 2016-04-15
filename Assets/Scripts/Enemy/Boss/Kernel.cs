using UnityEngine;

namespace Assets.Scripts.Enemy.Boss
{
    class Kernel : Enemy
    {
        [SerializeField]
        private SpriteRenderer spriteR;
        [SerializeField]
        private Sprite sprite;
        [SerializeField]
        private Sprite spriteHurt;
        public KernelHand leftHand;
        public KernelHand rightHand;

        public override void InitData()
        {
            base.InitData();
            spriteR.sprite = sprite;
        }

        public override void RunEntity()
        {
            if(leftHand.knockedOut && rightHand.knockedOut)
            {
                if (invulerability <= 0)
                {
                    spriteR.sprite = spriteHurt;
                    invulerability = invulerabilityTime;
                }
                if (hit)
                {
                    currentHealth -= damage;
                }
                if (invulerability > 0)
                {
                    if (hit)
                    {
                        render = !render;
                        Render(render);
                    }
                    invulerability -= Time.deltaTime;
                    if (invulerability <= 0)
                    {
                        spriteR.sprite = sprite;
                        leftHand.Revive();
                        rightHand.Revive();
                    }
                }
                if (currentHealth <= 0)
                {
                    Render(true);
                    Die();
                }
            }
            else if (!render)
            {
                render = true;
                Render(true);
            }
            hit = false;
        }

        protected override void Render(bool render)
        {
            spriteR.enabled = render;
        }
    }
}
