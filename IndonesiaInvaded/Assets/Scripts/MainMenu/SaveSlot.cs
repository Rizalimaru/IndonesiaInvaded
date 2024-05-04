using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId  = "";

    [Space(10)]
    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private RawImage screenShoot;
    [SerializeField] private TextMeshProUGUI MissionText;
    [SerializeField] private TextMeshProUGUI CityMissionText;
    [SerializeField] private TextMeshProUGUI percentageCompeleteText;

    public bool hasData {get; private set;} = false;
    private Button saveSlotButton;
    
    private void Awake(){
        saveSlotButton = this.GetComponent<Button>();
    }
    
    public void SetData(GameData data){
        if(data == null)
        {
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        else{
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            percentageCompeleteText.text = data.GetPercentageComplete() + "% COMPLETE";
        }
    }

    public string GetProfileId(){
        return this.profileId;
    }

    public void SetInteractable(bool interactable){
        saveSlotButton.interactable = interactable;
    } 
}
