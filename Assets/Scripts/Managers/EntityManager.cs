using UnityEngine;
using Assets.Scripts.Util;

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
        protected struct EntityData { public bool active; public Entity entity; public Callback callback; }

        private bool inited = false;

        /// <summary> Data structure that holds all entities that this GameObject manages. </summary>
        protected EntityData[][] entities;

        public virtual void Start()
        {
            if (!inited)
                Init();
        }

        protected void Init()
        {
            inited = true;
            //use raged array due to possible uneven counts. 
            entities = new EntityData[entityPrefabs.Length][];
            for (int i = 0; i < entityPrefabs.Length; i++)
            {
                entities[i] = new EntityData[entityCounts[i]];
                for (int j = 0; j < entityCounts[i]; j++)
                {
                    entities[i][j].active = false;
                    entities[i][j].entity = Instantiate(entityPrefabs[i]);
                    entities[i][j].entity.transform.position = INIT_OBJECT_SPAWN;
                    entities[i][j].entity.gameObject.SetActive(false);
                    entities[i][j].callback = null;
                }
            }
        }


        /// <summary> Readys an entity if one is available. </summary>
        /// <param name="type"> The type of entity to aquire. </param>
        /// <param name="loc"> The location to spawn the entity in. </param>
        /// <param name="direction"> The direction the entity should initially face. </param>
        /// <param name="callback"> The object to send a message to upon the aquired entites death. </param>
        /// <returns> True if an entity was able to be readied. </returns>
        protected bool AquireEntity(int type, Transform loc, Enums.Direction direction, Callback callback = null) 
        {
            if (type >= entities.Length || type < 0)
                throw new System.IndexOutOfRangeException("Invalid Entity Type. ");
            for(int i = 0; i < entities[type].Length; i++)
            {
                if(!entities[type][i].active)
                {
                    entities[type][i].active = true;
                    entities[type][i].entity.Init(loc, this, type, i, direction, false);
                    entities[type][i].entity.gameObject.SetActive(true);
                    entities[type][i].callback = callback;
                    return true;
                }
            }
            return false;
        }

        /// <summary> Readys an entity if one is available. </summary>
        /// <param name="type"> The type of entity to aquire. </param>
        /// <param name="loc"> The location to spawn the entity in. </param>
        /// <param name="direction"> The direction the entity should initially face. </param>
        /// <param name="callback"> The object to send a message to upon the aquired entites death. </param>
        /// <returns> True if an entity was able to be readied. </returns>
        protected bool AquireEntityCutScene(int type, Transform loc, Enums.Direction direction, Callback callback = null)
        {
            if (type >= entities.Length || type < 0)
                throw new System.IndexOutOfRangeException("Invalid Entity Type. ");
            for (int i = 0; i < entities[type].Length; i++)
            {
                if (!entities[type][i].active)
                {
                    entities[type][i].active = true;
                    entities[type][i].entity.Init(loc, this, type, i, direction, true);
                    entities[type][i].entity.gameObject.SetActive(true);
                    entities[type][i].callback = callback;
                    return true;
                }
            }
            return false;
        }

        /// <summary> Releases the given entity if it is active and sends a message to the callback if one was given. </summary>
        /// <param name="type"> The type of entity to release. </param>
        /// <param name="instance"> The specific instance to release. </param>
        /// <returns> True if an entity was able to be released. </returns>
        public virtual bool ReleaseEntity(int type, int instance)
        {
            if (type >= entities.Length || type < 0)
                throw new System.IndexOutOfRangeException("Invalid Entity Type. ");
            if (instance >= entities[type].Length || instance < 0)
                throw new System.IndexOutOfRangeException("Invalid Entity instance. ");
            if(entities[type][instance].active)
            {
                entities[type][instance].entity.transform.position = INIT_OBJECT_SPAWN;
                entities[type][instance].entity.gameObject.SetActive(false);
                if(entities[type][instance].callback != null)
                {
                    entities[type][instance].callback.entityDied(entities[type][instance].entity);
                    entities[type][instance].callback = null;
                }
                entities[type][instance].active = false;
                return true;
            }
            return false;
        }
    }
}
