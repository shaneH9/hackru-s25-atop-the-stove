using System.Collections.Generic;

public class CardCollection
{
    public List<Card> ownedCards;  // List of cards the player owns
    public int totalPoints;        // Total points the player has accumulated

    public CardCollection()
    {
        ownedCards = new List<Card>();
        totalPoints = 0;
    }

    // Add a card to the collection
    public void AddCard(Card card)
    {
        ownedCards.Add(card);
        totalPoints += card.rating;  // Update total points when a card is added
    }

    // Remove a card from the collection
    public void RemoveCard(Card card)
    {
        if (ownedCards.Contains(card))
        {
            ownedCards.Remove(card);
            totalPoints -= card.rating;  // Update total points when a card is removed
        }
    }
}
