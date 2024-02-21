using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public bool fire { get; private set; }
    private FixedJoystick _joystick;
    public float _horizontal { get; private set; }
    public float _vertical { get; private set; }
    public Vector2 inputDirection { get; private set; }
    private WeaponController _weaponController;
    private Button fireButton;
    private void Awake()
    {
        _joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FixedJoystick>();
        fireButton = GameObject.FindGameObjectWithTag("FireButton").GetComponent<Button>();

        _weaponController = GetComponentInChildren<WeaponController>();
        _joystick.gameObject.SetActive(true);
        fireButton.gameObject.SetActive(true);
    }

    private void Start()
    {
        fireButton.onClick.AddListener(delegate () { _weaponController.Fire(); });
    }

    private void Update()
    {
        GetDirectionInput();

        if (Input.GetKeyDown(KeyCode.Space))
            fire = true;
    }
    private void GetDirectionInput()
    {
        _horizontal = _joystick.Horizontal;
        _vertical = _joystick.Vertical;

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

        _vertical = Mathf.Round(_vertical);
        _horizontal = Mathf.Round(_horizontal);
        inputDirection = new Vector2(_horizontal, _vertical);
    }
}