using UnityEngine;
using UnityEngine.EventSystems;

public enum LaunchType
{
    StartGame,
    ExitGame
}

public class LaunchButton : CustomButton
{
    [SerializeField] private LaunchType buttonType;

    protected override void Start()
    {
        base.Start();
        menuManager = GetComponentInParent<HomeMenu>();
        audioManager = menuManager.audioManager;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);

        switch (buttonType)
        {
            case LaunchType.StartGame: menuManager.StartGame(); break;
            case LaunchType.ExitGame: menuManager.QuitGame(); break;
            default: Debug.Log("Unpredictable button type"); break;
        }
    }
}
