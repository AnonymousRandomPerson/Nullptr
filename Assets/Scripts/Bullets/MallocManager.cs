using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Bullets
{
    class MallocManager : MonoBehaviour
    {
        public static MallocManager instance;
        public ArrayList list;

        public void Init()
        {
            instance = this;
            list = new ArrayList();
        }

        internal void AddMalloc(Malloc malloc)
        {
            list.Add(malloc);
        }

        internal void RemoveMalloc(Malloc malloc)
        {
            list.Remove(malloc);
        }

        public ArrayList GetMallocs()
        {
            return list;
        }
        
        public void OnDestroy()
        {
            instance = null;
        }
    }
}
