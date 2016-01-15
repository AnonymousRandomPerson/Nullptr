using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary> Parent Class for all Entity Managers. </summary>
    public class EntityManager : MonoBehaviour
    {
        /// <summary> Default offscreen spawn position. </summary>
        public static Vector3 INIT_OBJECT_SPAWN = new Vector3(-5000, -5000, 0);

        /// <summary> The prefabs to spawn. </summary>
        [SerializeField]
        private Entity[] entityPrefabs;

        /// <summary> How many of each prefab to spawn. </summary>
        [SerializeField]
        private int[] entityCounts;

        /// <summary> internal struct to keep track of Entities. </summary>
        protected struct EntityData { public bool active; public float lifeTime; public Entity entity; }

        /// <summary> Number of active entities. </summary>
        protected int activeEntityCount;

        /// <summary> Data structure that holds all entities that this GameObject manages. </summary>
        protected EntityData[][] entities;

        void Start()
        {
            activeEntityCount = 0;
            //use raged array due to possible uneven counts. 
            entities = new EntityData[entityPrefabs.Length][];
            for (int i = 0; i < entityPrefabs.Length; i++)
            {
                entities[i] = new EntityData[entityCounts[i]];
                for(int j = 0; j < entityCounts[i]; j++)
                {
                    entities[i][j].active = false;
                    entities[i][j].lifeTime = 0f;
                    entities[i][j].entity = Instantiate(entityPrefabs[i]);
                    entities[i][j].entity.transform.position = INIT_OBJECT_SPAWN;
                }
            }
        }
        
        void Update()
        {
            if(GameManager.IsRunning && activeEntityCount > 0)
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    for (int j = 0; j < entities[i].Length; j++)
                    {
                        if (entities[i][j].active)
                        {
                            entities[i][j].lifeTime += Time.deltaTime;
                            entities[i][j].entity.Update(entities[i][j].lifeTime);
                            if(entities[i][j].entity.isDead)
                            {
                                entities[i][j].active = false;
                                entities[i][j].lifeTime = 0f;
                                entities[i][j].entity.transform.position = INIT_OBJECT_SPAWN;
                                activeEntityCount--;
                            }
                        }
                    }
                }
            }
        }

        protected bool AquireEntity(Transform loc, int type)
        {

        }
    }
}
