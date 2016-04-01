using UnityEngine;

/// <summary>
/// Tilt.
/// </summary>
public class Tilt : MonoBehaviour {

	/// <summary>
    /// Tilts the object slightly.
    /// </summary>
	private void Start() {
        Vector3 initAngle = transform.rotation.eulerAngles;
        initAngle.z += Random.Range(-20f, 20f);
        Quaternion initQuaternion = transform.rotation;
        initQuaternion.eulerAngles = initAngle;
        transform.rotation = initQuaternion;
	}
}
