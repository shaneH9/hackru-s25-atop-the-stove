using UnityEngine;

public class Recipe : MonoBehaviour
{
    [SerializeField] private GameObject[] recipe;
    private int currentIndex = 0; 
    private RectTransform uiPanel;


    private void SetCurrentObjectAtTop()
    {
        foreach (var obj in recipe)
        {
            obj.SetActive(false);
        }

        recipe[currentIndex].SetActive(true);
        
        RectTransform rectTransform = recipe[currentIndex].GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(0, uiPanel.rect.height / 2);
        }
    }

     public void NextRecipe()
    {
        currentIndex = (currentIndex + 1) % recipe.Length;
        SetCurrentObjectAtTop();
    }

    
    public void PreviousRecipe()
    {
        currentIndex = (currentIndex - 1 + recipe.Length) % recipe.Length;
        SetCurrentObjectAtTop();
    }
}