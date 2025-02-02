using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodInput : MonoBehaviour
{
    public TMP_InputField foodNameInput;
    public TMP_InputField numOfItemsInput;
    public TMP_InputField caloriesInput;



    public void onSubmit()
    {
        Debug.Log("Food submitted!");
        UIManager.Instance.ShowCanvas("CardOptions");
        // Make CardOptions spawn cards
        CardOptions cardOptions = FindFirstObjectByType<CardOptions>();
        if (cardOptions != null)
        {
            cardOptions.FillAndSpawnCards();
        }
        else
        {
            Debug.LogError("CardOptions component not found in the scene!");
        }
    }
}