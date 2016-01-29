using UnityEngine;

namespace Assets.Scripts.Platforms
{
    class InfiniteFloor : MonoBehaviour
    {
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
        private Transform player;
        /// <summary> The current center of the tile field. </summary>
        private Vector3 currentPlayerPosRef;

        private int distanceModifier;

        void Start()
        {
            currentPlayerPosRef = Vector3.zero;
            player = FindObjectOfType<Player.Player>().transform;
            distanceModifier = 1;
        }

        void Update()
        {
            if (Mathf.Abs(player.position.x - currentPlayerPosRef.x) > rowWidth * distanceModifier)
            {
                if (player.position.x > currentPlayerPosRef.x)
                    shiftUp();
                else
                    shiftDown();
            }
        }

        /// <summary> Resets the Tile field to its initial position. </summary>
        public void ResetTiles()
        {
            for (int i = 0; i < rows.Length; i++)
                rows[i].transform.position = new Vector3(initPosition.x - rowWidth * i, initPosition.y, initPosition.z);
            currentPlayerPosRef = Vector3.zero;
        }

        /// <summary> Move all Tile field down one row. </summary>
        private void shiftDown()
        {
            GameObject temp = rows[rows.Length - 1];
            for (int i = rows.Length - 1; i > 0; i--)
                rows[i] = rows[i - 1];
            rows[0] = temp;
            rows[0].transform.position = new Vector3(rows[1].transform.position.x - rowWidth, rows[1].transform.position.y, rows[1].transform.position.z);
            currentPlayerPosRef.x -= rowWidth;
        }

        /// <summary> Move the Tile field up one row. </summary>
        private void shiftUp()
        {
            distanceModifier = 1;
            GameObject temp = rows[0];
            for (int i = 1; i < rows.Length; i++)
                rows[i - 1] = rows[i];
            rows[rows.Length - 1] = temp;
            rows[rows.Length - 1].transform.position = new Vector3(rows[rows.Length - 2].transform.position.x + rowWidth, rows[rows.Length - 2].transform.position.y, rows[rows.Length - 2].transform.position.z);
            currentPlayerPosRef.x += rowWidth;
        }

        /// <summary> Handles a platform being destroyed. </summary>
        public void platformDestroyed()
        {
            GameObject temp = rows[0];
            for (int i = 1; i < rows.Length; i++)
                rows[i - 1] = rows[i];
            rows[rows.Length - 1] = temp;
            rows[rows.Length - 1].transform.position = new Vector3(rows[rows.Length - 2].transform.position.x + rowWidth, rows[rows.Length - 2].transform.position.y, rows[rows.Length - 2].transform.position.z);
            currentPlayerPosRef.x += rowWidth;
            distanceModifier++;
        }
    }
}