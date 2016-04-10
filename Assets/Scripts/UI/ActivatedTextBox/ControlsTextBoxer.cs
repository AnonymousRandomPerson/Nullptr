using Assets.Scripts.Util;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Scripts.UI.ActivatedTextBox {

    public class ControlsTextBoxer : MonoBehaviour {
        [SerializeField]
        private TextBox[] textBoxes;

        void Start() {

        }

        void Update() {
            int index = 0;
            foreach (CustomInput.UserInput input in Enum.GetValues(typeof(CustomInput.UserInput))) {
                string key = CustomInput.UsingPad ? CustomInput.gamepadButton(input) : CustomInput.keyboardKey(input).ToString();
                textBoxes[index++].Text = string.Format("{0} - {1}", key, SplitCamelCase(input.ToString()));
            }
        }

        public static string SplitCamelCase(string str) {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
    }
}
