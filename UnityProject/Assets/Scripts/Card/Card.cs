[System.Serializable]
public class Card
{
    public string title;  // Name of the food item
    public int rating;       // Points associated with the card (e.g., score or ability points)
    public string desc;    
    public string imagePath; // Path or reference to the card's image (can be used for UI display)

    // Constructor
    public Card(string cardName, int rating, string desc, string imagePath)
    {
        this.title = cardName;
        this.rating = rating;
        this.desc = desc;
        this.imagePath = imagePath;
    }
}
