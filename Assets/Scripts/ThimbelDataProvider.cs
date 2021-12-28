using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;

public class ThimbelDataProvider : InputSystemGlobalHandlerListener, IMixedRealityInputHandler
{

    [Header("MRTK Input Actions")]
    [SerializeField] private MixedRealityInputAction outputAction;


    [SerializeField] private bool thimbelTouched;

    public MixedRealityInputAction inputAction;

    private void Update()
    {
        foreach (var controller in CoreServices.InputSystem.DetectedControllers)
        {
            if (controller.InputSource.SourceType == InputSourceType.Controller)
            {
                if (Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.JoystickButton1) || Input.GetKey(KeyCode.JoystickButton2))
                {
                    CoreServices.InputSystem?.RaiseOnInputDown(controller.InputSource, Handedness.Any, inputAction);
                }
                else
                {
                    CoreServices.InputSystem?.RaiseOnInputUp(controller.InputSource, Handedness.Any, inputAction);
                }
            }
        }
    }

    #region On Input Up/Down
    public void OnInputUp(InputEventData eventData)
    {
        if (eventData.MixedRealityInputAction == outputAction) thimbelTouched = false;
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (eventData.MixedRealityInputAction == outputAction) thimbelTouched = true;
    }
    #endregion

    #region Handlers
    protected override void RegisterHandlers()
    {
        CoreServices.InputSystem.RegisterHandler<IMixedRealityInputHandler>(this);
    }

    protected override void UnregisterHandlers()
    {

    }
    #endregion
}