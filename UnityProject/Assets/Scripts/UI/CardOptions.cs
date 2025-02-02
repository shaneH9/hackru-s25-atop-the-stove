using UnityEngine;

public class CardOptions : MonoBehaviour
{
    public string cardPrefabPath = "Prefabs/CardPrefab"; // Path to the card prefab in Resources
    public Transform cardContainer; // The container to hold the cards

    // This method will be called externally to spawn the cards
    public void FillAndSpawnCards()
    {
        // Define 3 sample cards (you can modify this to load from a database or user data)
        Card[] cardsToSpawn = new Card[]
        {
            new Card("Pizza", 50, "A delicious cheesy pizza!", "Images/Pizza"),
            new Card("Burger", 40, "Juicy and tasty burger with fries.", "Images/Burger"),
            new Card("Ice Cream", 30, "Sweet and creamy dessert.", "Images/IceCream")
        };

        // Ensure the card container is assigned
        if (cardContainer == null)
        {
            Debug.LogError("Card Container is not assigned in the Inspector!");
            return;
        }

        // Loop through the array of cards and instantiate each card prefab
        for (int i = 0; i < cardsToSpawn.Length; i++)
        {
            // Load the card prefab from Resources
            GameObject cardPrefab = Resources.Load<GameObject>(cardPrefabPath);

            if (cardPrefab != null)
            {
                Debug.Log($"Card prefab loaded: {cardPrefab.name}");

                // Instantiate the card prefab as a child of the card container
                GameObject cardInstance = Instantiate(cardPrefab, cardContainer);

                // Assign a unique name to the card (avoid name conflicts)
                cardInstance.name = $"{cardPrefab.name}_{i}"; // CardPrefab_0, CardPrefab_1, ...

                // Position the cards dynamically (evenly spaced)
                float spacing = 2.5f; // Adjust spacing between cards
                cardInstance.transform.localPosition = new Vector3(i * spacing, 0, 0); // Position relative to container

                // Get the CardDisplay component and initialize it
                CardDisplay cardDisplay = cardInstance.GetComponent<CardDisplay>();
                if (cardDisplay != null)
                {
                    Debug.Log("CardDisplay component found, initializing card.");
                    cardDisplay.InitializeCard(cardsToSpawn[i]);
                }
                else
                {
                    Debug.LogError("CardDisplay component not found on the card prefab!");
                }
            }
            else
            {
                Debug.LogError("Card prefab not found in Resources! Ensure the prefab path is correct.");
            }
        }
    }
}
