using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    /** Singleton pattern for a quick implementation */
    private static UIManager Instance;

    [SerializeField] private List<UIScreen> screens;
    private UIScreen active;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}