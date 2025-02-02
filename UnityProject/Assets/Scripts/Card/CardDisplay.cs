using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Image Image;       // The Image UI component to display the card's food image
    public TextMeshProUGUI TitleText;     // The Text UI component to display the card's name
    public TextMeshProUGUI RateText;   // The Text UI component to display the card's points
    public TextMeshProUGUI DescText; // The Text UI component to display the card's category

    // Method to initialize and populate the card data
    public void InitializeCard(Card card)
    {
        // Set the card name
        TitleText.text = card.title;

        // Set the points
        RateText.text = card.rating.ToString();

        // Set the category
        DescText.text = "Category: " + card.desc;

        // Set the card image (using Resources.Load to load the sprite dynamically)
        Sprite cardSprite = Resources.Load<Sprite>(card.imagePath);
        if (cardSprite != null)
        {
            Image.sprite = cardSprite;
        }
        else
        {
            Debug.LogError("Image for " + card.title + " not found!");
        }   
    }
}
