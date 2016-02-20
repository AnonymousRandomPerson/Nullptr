using UnityEngine;

namespace Assets.Scripts.Platforms
{
    public class GarbageFloor : MonoBehaviour
    {
        [SerializeField]
        private GameObject wall;
        [SerializeField]
        private int wallWidth;
        /// <summary> Array of "infinite" tiles. 0 = behind, length = in front. </summary>
        [SerializeField]
        private GameObject[] rows;
        /// <summary> Width of each row used for spaceing. </summary>
        [SerializeField]
        private float rowWidth;
        /// <summary> The initial position of the first row. </summary>
        [SerializeField]
        private Vector3 initPosition;


        /// <summary> The current player position. </summary>
        public Transform target;
        /// <summary> The current center of the tile field. </summary>
        private Vector3 currentPlayerPosRef;

        private int distanceModifier;

        void Start()
        {
            currentPlayerPosRef = Vector3.zero;
            distanceModifier = 1;
        }

        void Update()
        {
            if (target == null)
                return;
            if (Mathf.Abs(target.transform.position.x - rows[0].transform.position.x) < 1)
                shiftUp();
            if (Mathf.Abs(target.position.x - wall.transform.position.x) < wallWidth)
                wall.transform.position = new Vector3(target.transform.position.x + wallWidth, wall.transform.position.y, wall.transform.position.z);
        }

        /// <summary> Resets the Tile field to its initial position. </summary>
        public void ResetTiles()
        {
            for (int i = 0; i < rows.Length; i++)
                rows[i].transform.position = new Vector3(initPosition.x - rowWidth * i, initPosition.y, initPosition.z);
            currentPlayerPosRef = Vector3.zero;
        }

        /// <summary> Move the Tile field up one row. </summary>
        private void shiftUp()
        {
            distanceModifier = 1;
            GameObject temp = rows[0];
            for (int i = 1; i < rows.Length; i++)
                rows[i - 1] = rows[i];
            rows[rows.Length - 1] = temp;
            DestroyablePlatform p = rows[rows.Length - 1].GetComponent<DestroyablePlatform>();
            p.posToGoTo = new Vector3(rows[rows.Length - 2].transform.position.x + rowWidth, rows[rows.Length - 2].transform.position.y, rows[rows.Length - 2].transform.position.z);
            p.DestroyThis();
            currentPlayerPosRef.x += rowWidth;
        }
    }
}
