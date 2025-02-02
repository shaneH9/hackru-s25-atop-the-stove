using UnityEngine;

public class FoodInput : MonoBehaviour
{
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