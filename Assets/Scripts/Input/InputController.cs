using UnityEngine;

public abstract class InputController
{
    protected PlayerInputActions playerInputActions;
    protected AudioManager audioManager;

    public InputController(AudioManager audioManager)
    {
        playerInputActions = new PlayerInputActions();
        this.audioManager = audioManager;
    }
    protected virtual void SubscribeActions()
    {

    }
}