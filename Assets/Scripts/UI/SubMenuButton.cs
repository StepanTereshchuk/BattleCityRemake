using UnityEngine;
using UnityEngine.EventSystems;

public class SubMenuButton : CustomButton
{
    [SerializeField] private GameObject openTab;

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
        if (openTab != null)
            menuManager.OpenTab(openTab);
        else
            Debug.Log("No tab for opening added");
    }
}
