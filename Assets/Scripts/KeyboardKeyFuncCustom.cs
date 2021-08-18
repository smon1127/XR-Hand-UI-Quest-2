// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    /// <summary>
    /// Represents a key on the keyboard that has a function.
    /// </summary>
    public class KeyboardKeyFuncCustom : MonoBehaviour
    {
        /// <summary>
        /// Possible functionality for a button.
        /// </summary>
        public enum Function
        {
            // Commands
            Enter,
            Tab,
            ABC,
            Symbol,
            Previous,
            Next,
            Close,
            Dictate,

            // Editing
            Shift,
            CapsLock,
            Space,
            Backspace,

            UNDEFINED,
        }

        /// <summary>
        /// Designer specified functionality of a keyboard button.
        /// </summary>
        [Experimental]
        [SerializeField, FormerlySerializedAs("m_ButtonFunction")] private Function buttonFunction = Function.UNDEFINED;

        public Function ButtonFunction => buttonFunction;

        /// <summary>
        /// Subscribe to the onClick event.
        /// </summary>
        private void Start()
        {
            Interactable m_Button = GetComponent<Interactable>();
            m_Button.OnClick.RemoveAllListeners();
            m_Button.OnClick.AddListener(FireFunctionKey);
        }

        /// <summary>
        /// Method injected into the button's onClick listener.
        /// </summary>
        private void FireFunctionKey()
        {
            NonNativeKeyboardCustom.Instance.FunctionKey(this);
        }
    }
}
