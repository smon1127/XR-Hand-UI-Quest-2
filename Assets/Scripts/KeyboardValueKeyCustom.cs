// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    /// <summary>
    /// Represents a key on the keyboard that has a string value for input.
    /// </summary>

    public class KeyboardValueKeyCustom : MonoBehaviour
    {
        /// <summary>
        /// The default string value for this key.
        /// </summary>
        [Experimental]
        public string Value;

        /// <summary>
        /// The shifted string value for this key.
        /// </summary>
        public string ShiftValue;

        /// <summary>
        /// Reference to child text element.
        /// </summary>
        private TextMeshPro m_Text;

        /// <summary>
        /// Reference to the GameObject's button component.
        /// </summary>
        private Interactable m_Button;

        /// <summary>
        /// Get the button component.
        /// </summary>
        private void Awake()
        {
            m_Button = GetComponent<Interactable>();
        }

        /// <summary>
        /// Initialize key text, subscribe to the onClick event, and subscribe to keyboard shift event.
        /// </summary>
        private void Start()
        {
            m_Text = gameObject.GetComponentInChildren<TextMeshPro>();
            m_Text.text = Value;

            m_Button.OnClick.RemoveAllListeners();
            m_Button.OnClick.AddListener(FireAppendValue);


            NonNativeKeyboardCustom.Instance.OnKeyboardShifted += Shift;
        }

        /// <summary>
        /// Method injected into the button's onClick listener.
        /// </summary>
        private void FireAppendValue()
        {
            NonNativeKeyboardCustom.Instance.AppendValue(this);
        }

        /// <summary>
        /// Called by the Keyboard when the shift key is pressed. Updates the text for this key using the Value and ShiftValue fields.
        /// </summary>
        /// <param name="isShifted">Indicates the state of shift, the key needs to be changed to.</param>
        public void Shift(bool isShifted)
        {
            // Shift value should only be applied if a shift value is present.
            if (isShifted && !string.IsNullOrEmpty(ShiftValue))
            {
                m_Text.text = ShiftValue;
            }
            else
            {
                m_Text.text = Value;
            }
        }
    }
}
