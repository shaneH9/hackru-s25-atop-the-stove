using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    /** Singleton pattern for a quick implementation */
    public static UIManager Instance;

    [SerializeField] private List<Canvas> canvases;
    private Canvas active;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        active = canvases[0];
        for (int i = 1; i < canvases.Count; i++) {
            canvases[i].gameObject.SetActive(false);
        }
    }

    public void ShowCanvas(string canvasName)
    {
        // Deactivate the current active canvas
        if (active != null) active.gameObject.SetActive(false);

        // Find the requested canvas and activate it
        active = canvases.Find(c => c.name == canvasName);
        if (active != null) active.gameObject.SetActive(true);
    }
}