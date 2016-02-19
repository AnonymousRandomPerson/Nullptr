using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponDisplay : MonoBehaviour
{
    [SerializeField]
    Text weaponText;
    public string WeaponText { set { weaponText.text = value; } }
}
