using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;
using System.Collections.Generic;  // For using List<Card>

public class PythonExecutor : MonoBehaviour
{
    private string pythonPath; // Path to the Python executable
    private string scriptPath; // Path to your Python script
    private string projectRootPath;  // Root directory of Unity project

    // Initialize the paths
    void Start()
    {
        // Get the root directory of the Unity project (which will help navigate to parent dirs)
        projectRootPath = Directory.GetParent(Application.dataPath).ToString();
        UnityEngine.Debug.Log("Project Root Path: " + projectRootPath);

        // Determine the Python executable based on the OS
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            // Windows path to Python executable inside .venv
            pythonPath = Path.Combine(projectRootPath, "python_LLM_CV_integration/.venv", "Scripts", "python.exe");
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
        {
            // macOS/Linux path to Python executable inside .venv
            pythonPath = Path.Combine(projectRootPath, "python_LLM_CV_integration/.venv", "bin", "python3");
        }
        else
        {
            UnityEngine.Debug.LogError("Unsupported platform: " + Application.platform);
            return;
        }

        // Check if Python executable exists
        if (!File.Exists(pythonPath))
        {
            UnityEngine.Debug.LogError("Python executable not found: " + pythonPath);
        }
    }

    // Load the food card data from JSON file
    public List<Card> LoadFoodCardData()
    {
        // Load the JSON file from Resources
        TextAsset jsonText = Resources.Load<TextAsset>("food_card_data");  // No need for extension (.json)
        if (jsonText == null)
        {
            UnityEngine.Debug.LogError("Food card JSON not found in Resources!");
            return null;
        }

        // Deserialize the JSON string into a list of Card objects
        List<Card> foodCards = JsonUtility.FromJson<CardListWrapper>(jsonText.ToString()).cards;
        return foodCards;
    }

    // Call this method to execute the Python script
    // Run Python script and handle potential errors
    public void RunPythonScript(string foodItemName, int numOfItems, int estimatedCalories, string outputFile)
    {
        // Ensure the Python executable exists
        if (!File.Exists(pythonPath))
        {
            UnityEngine.Debug.LogError("Python executable not found! Please ensure the virtual environment is set up correctly.");
            return;
        }

        string scriptFilePath = Path.Combine(projectRootPath, "python_LLM_CV_integration/create_card.py");

        if (!File.Exists(scriptFilePath))
        {
            UnityEngine.Debug.LogError("Python script not found! Ensure create_card.py is in the correct location.");
            return;
        }

        try
        {
            // Set up ProcessStartInfo
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = $"{scriptFilePath} {foodItemName} {numOfItems} {estimatedCalories} {outputFile}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            // Start the process and capture output
            using (Process process = Process.Start(start))
            {
                if (process != null)
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        UnityEngine.Debug.Log(result);  // Output for debugging
                    }
                    process.WaitForExit();  // Optional: Add a timeout or further handling for waiting
                }
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Error executing Python script: {ex.Message}");
        }
    }
}

// Wrapper class for deserialization of a list of Card objects
[System.Serializable]
public class CardListWrapper
{
    public List<Card> cards;  // This holds the list of cards from the JSON
}
