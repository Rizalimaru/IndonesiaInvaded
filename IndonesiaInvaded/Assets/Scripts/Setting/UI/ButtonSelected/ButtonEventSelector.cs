using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEventSelector: MonoBehaviour
{
    private Button button;
    private EventTrigger eventTrigger;

    void Start()
    {
        button = GetComponent<Button>();
        eventTrigger = GetComponent<EventTrigger>();

        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        AddEventTrigger(OnPointerEnter, EventTriggerType.PointerEnter);
    }

    private void AddEventTrigger(UnityEngine.Events.UnityAction<BaseEventData> action, EventTriggerType triggerType)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = triggerType };
        entry.callback.AddListener(action);
        eventTrigger.triggers.Add(entry);
    }

    private void OnPointerEnter(BaseEventData data)
    {
        if (button.interactable)
        {
            Debug.Log("Pointer entered the button area!");
            // Lakukan aksi yang diinginkan di sini
        }
    }
}
