using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Util
{
    /// <summary>
    /// Prevents an object from rotating.
    /// </summary>
    class NoRotate : MonoBehaviour {
    	
    	/// <summary>
        /// Keeps the object at a certain rotation.
        /// </summary>
    	private void Update () {
            transform.rotation = Quaternion.identity;
    	}
    }
}