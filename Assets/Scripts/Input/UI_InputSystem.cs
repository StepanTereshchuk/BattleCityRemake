using System.Diagnostics;
using UnityEngine.InputSystem;

public class UI_InputSystem : InputController
{
    public UI_InputSystem(AudioManager audioManager) : base(audioManager)
    {
        playerInputActions.Player.Disable();
        playerInputActions.UI.Enable();
        SubscribeActions();
    }

    protected override void SubscribeActions()
    {
        //playerInputActions.UI.Navigate.performed += ButtonSelected_performed;
        //playerInputActions.UI.Submit.performed += ButtonPressed_performed;
    }

    private void ButtonSelected_performed(InputAction.CallbackContext context)
    {
        audioManager.PlaySound(SoundKey.ButtonSelected);
    }
    private void ButtonPressed_performed(InputAction.CallbackContext context)
    {
        audioManager.PlaySound(SoundKey.ButtonPressed);
    }
}
