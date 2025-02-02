[System.Serializable]
public class Card
{
    public string title;          // Name of the food item (mapped from card_name)
    public int rating;            // Points associated with the card (mapped from card_rating)
    public string desc;           // Description of the card (mapped from special_technique)
    public string imageUrl;       // Image URL for the card (mapped from image_url)

    // Constructor
    public Card(string cardName, int rating, string desc, string imageUrl)
    {
        title = cardName;
        this.rating = rating;
        this.desc = desc;
        this.imageUrl = imageUrl;
    }
}
