using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts
{
    class test : MonoBehaviour
    {
        void Update()
        {
            Vector2 directionLeft = transform.right;
            directionLeft.x = -directionLeft.x;
            Vector2 vectLeft = Vector2.right;
            vectLeft.x = -vectLeft.x;
            transform.Translate(transform.right * Time.deltaTime, Space.World);
            Debug.DrawRay(transform.position, transform.right, Color.blue);
            Debug.DrawRay(transform.position, directionLeft, Color.red);
            Debug.DrawRay(transform.position, Vector2.right, Color.green);
            Debug.DrawRay(transform.position, vectLeft, Color.yellow);
        }
    }
}
