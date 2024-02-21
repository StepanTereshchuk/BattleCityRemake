using UnityEngine;
using UnityEngine.InputSystem;

public class TankInputController : InputController
{
    private WeaponController _weaponController;
    private Player _player;

    public TankInputController(WeaponController weaponController, AudioManager audioManager, Player player) : base(audioManager)
    {
        _weaponController = weaponController;
        playerInputActions.Player.Enable();
        SubscribeActions();
        _player = player;
    }

    public Vector2 inputVector { get; private set; }
    public float shoot { get; private set; }

    private void Movement_performed(InputAction.CallbackContext context)
    {
        if (_player && _player.gameObject.activeSelf)
        {
            audioManager.PlaySound(SoundKey.MotorMove);
            inputVector = context.ReadValue<Vector2>();
            ModifyPLayerInput();
        }
    }
    private void Movement_canceled(InputAction.CallbackContext context)
    {
        if (_player && _player.gameObject.activeSelf)
        {
            audioManager.PlaySound(SoundKey.MotorIdle);
            inputVector = context.ReadValue<Vector2>();
            ModifyPLayerInput();
        }
    }
    private void Shoot_performed(InputAction.CallbackContext context)
    {
        if (_player && _player.gameObject.activeSelf)
        {
            shoot = context.ReadValue<float>();
            _weaponController.Fire();
        }
    }
    private void ModifyPLayerInput()
    {
        // cancel diagonal moving
        int _horizontal = (int)inputVector.x;
        int _vertical = (int)inputVector.y;

        if (_vertical > Mathf.Abs(_horizontal))
        {
            //Debug.Log("Up");
            _vertical = 1;
            _horizontal = 0;
        }
        else
        if (_horizontal > Mathf.Abs(_vertical))
        {
            //Debug.Log("Right");
            _horizontal = 1;
            _vertical = 0;
        }
        else
        if (_vertical < -Mathf.Abs(_horizontal))
        {
            //Debug.Log("DOWN");
            _vertical = -1;
            _horizontal = 0;
        }
        else
        if (_horizontal < -Mathf.Abs(_vertical))
        {
            //Debug.Log("LEFT");
            _horizontal = -1;
            _vertical = 0;
        }
        inputVector = new Vector2(_horizontal, _vertical);
    }

    protected override void SubscribeActions()
    {
        playerInputActions.Player.Shoot.performed += Shoot_performed;
        playerInputActions.Player.Movement.performed += Movement_performed;
        playerInputActions.Player.Movement.canceled += Movement_canceled;
    }
}
