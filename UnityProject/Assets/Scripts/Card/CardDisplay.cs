using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class CardDisplay : MonoBehaviour
{
    public Image Image;               // The Image UI component to display the card's food image
    public TextMeshProUGUI TitleText; // The Text UI component to display the card's name
    public TextMeshProUGUI RateText;  // The Text UI component to display the card's points
    public TextMeshProUGUI DescText;  // The Text UI component to display the card's category

    // Method to initialize and populate the card data
    public void InitializeCard(Card card)
    {
        // Debug
        Debug.Log("Initializing card: " + card.title);
        Debug.Log("Rating: " + card.rating + " Desc: " + card.desc + " Image URL: " + card.imageUrl);
        // Set the card name
        TitleText.text = card.title;

        // Set the points
        RateText.text = card.rating.ToString();

        // Set the category
        DescText.text = card.desc;

        // Start the image download coroutine
        if (card.imageUrl != null)
        {
            StartCoroutine(DownloadImage(card.imageUrl));  // Download the image from the URL
        }
    }

    // Coroutine to download the image and set it to the Image component
    private IEnumerator DownloadImage(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            UnityEngine.Debug.LogError("Error downloading image: " + request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); // Set the texture to the Image component
        }
    }
}
