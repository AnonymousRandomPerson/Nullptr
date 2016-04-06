using UnityEngine;

namespace Assets.Scripts.UI
{
    class WeaponCutScene : MonoBehaviour
    {
        [SerializeField]
        private GameObject background;
        [SerializeField]
        private GameObject foreground;
        [SerializeField]
        private Transform barrel;
        [SerializeField]
        private float timeBetweenShots;
        [SerializeField]
        private float timeBeforeLoad;
        [SerializeField]
        private Managers.BulletManager bulletManager;
        [SerializeField]
        private Util.Enums.BulletTypes ShootBullet;
        [SerializeField]
        private Util.Enums.BulletTypes AddBullet;
        [SerializeField]
        private int numberToShoot;
        [SerializeField]
        private string levelToLoad;
        [SerializeField]
        private bool load;

        private float wait;
        private int count;

        void Start()
        {

            wait = 0f;
            count = 0;
            Managers.GameManager.CutScene();
            Managers.GameManager.instance.AddWeapon(AddBullet);
            gameObject.transform.position = new Vector3(FindObjectOfType<Camera>().transform.position.x, 0, -5);
        }

        void Update()
        {
            if ((background.transform.localPosition.x > 0) || (foreground.transform.localPosition.x < 0))
            {
                if (background.transform.localPosition.x > 0)
                    background.transform.Translate(-Vector2.right * 5 * Time.deltaTime);
                if (foreground.transform.localPosition.x < 0)
                    foreground.transform.Translate(Vector2.right * 5 * Time.deltaTime);
            }
            else
            {
                if (count < numberToShoot && (wait += Time.deltaTime) > timeBetweenShots)
                {
                    wait = 0;
                    count++;
                    bulletManager.Shoot(ShootBullet, barrel, Util.Enums.Direction.Right, true);
                }
                if(load && count >= numberToShoot && (wait += Time.deltaTime) > timeBeforeLoad)
                    UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad);
            }
        }        
    }
}
