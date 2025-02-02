using UnityEngine;
using TMPro;

public class FoodInput : MonoBehaviour
{
    public TMP_InputField foodNameInput;
    public TMP_InputField numOfItemsInput;
    public TMP_InputField caloriesInput;

    private string foodName;
    private int numOfItems;
    private int calories;

    public void onSubmit()
    {
        Debug.Log("Food submitted!");

        // Check if input fields are not empty and parse the values
        if (string.IsNullOrEmpty(foodNameInput.text) || string.IsNullOrEmpty(numOfItemsInput.text) || string.IsNullOrEmpty(caloriesInput.text))
        {
            Debug.LogError("All fields must be filled!");
            return;
        }

        // Store the input data in variables
        foodName = foodNameInput.text;
        numOfItems = int.Parse(numOfItemsInput.text);  // Convert to integer
        calories = int.Parse(caloriesInput.text);      // Convert to integer

        Debug.Log($"Food Name: {foodName}, Number of Items: {numOfItems}, Calories: {calories}");

        // Show CardOptions UI
        UIManager.Instance.ShowCanvas("CardOptions");

        // Get the CardOptions component and pass the data
        CardOptions cardOptions = FindFirstObjectByType<CardOptions>();
        if (cardOptions != null)
        {
            cardOptions.FillAndSpawnCards(foodName, numOfItems, calories);
        }
        else
        {
            Debug.LogError("CardOptions component not found in the scene!");
        }
    }
}
