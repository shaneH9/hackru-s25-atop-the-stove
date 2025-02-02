from tensorflow.keras.models import load_model
import cv2
import numpy as np
from tensorflow.keras.applications.mobilenet_v2 import preprocess_input
import sys

from groq import Groq
import os
from dotenv import load_dotenv

import requests
import re
import json


# api setup
load_dotenv()
groq_api_key = os.getenv('GROQ_API_KEY')
if not groq_api_key:
    raise ValueError("GROQ_API_KEY not found in .env file")
os.environ['GROQ_API_KEY'] = groq_api_key
unsplash_access_key = os.getenv('UNSPLASH_API_KEY')
if not unsplash_access_key:
    raise ValueError("UNSPLASH_API_KEY not found in .env file")

client = Groq()

def load_trained_model(model_path):
    return load_model(model_path)

def preprocess_image(image_path):
    image = cv2.imread(image_path)  # Load image
    image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)  # Convert to RGB
    image = cv2.resize(image, (224, 224))  # Resize to 224x224 (MobileNetV2 input size)
    image = preprocess_input(image)  # Normalize using MobileNetV2 preprocessing
    image = np.expand_dims(image, axis=0)  # Add batch dimension
    return image

def predict_food_item(model, image_path):
    processed_image = preprocess_image(image_path)
    predictions = model.predict(processed_image)
    predicted_class = np.argmax(predictions)  

    class_labels = ['bread', 'dairy', 'dessert', 'egg', 'fried food', 'meat', 'noodles-pasta', 'rice', 'seafood', 'soup', 'vegetable-fruit']  # Replace with actual class names

    return class_labels[predicted_class], predictions[0][predicted_class]

def generate_content(prompt):
    response = client.chat.completions.create(
        messages = [{'role':'user','content':prompt}],
        model = 'llama3-70b-8192'
    )
    return response

# def produce_json_object(food_item, confidence):
    
#     # return {
#     #     "food_item": food_item,
#     #     "confidence": confidence
#     # }

def get_unsplash_image(query):
    """Fetch an image URL from Unsplash based on a food name."""
    url = "https://api.unsplash.com/search/photos"
    params = {
        "query": query,
        "client_id": unsplash_access_key,
        "per_page": 1  # Get the first image
    }
    response = requests.get(url, params=params)
    
    if response.status_code == 200:
        data = response.json()
        if data["results"]:
            return data["results"][0]["urls"]["regular"]  # Extract the image URL
        else:
            return None  # No image found
    else:
        raise ValueError(f"Error fetching image: {response.status_code}, {response.text}")

def create_food_card(food_item_name, response_text, output_file):
    json_match = re.search(r'```json\n({.*?})\n```', response_text, re.DOTALL)
    if not json_match:
        raise ValueError("JSON block not found in response.")
    json_data = json_match.group(1)

    try:
        # Load JSON as dictionary
        card_data = json.loads(json_data)

        image_url = get_unsplash_image(food_item_name)
        if image_url:
            card_data["image_url"] = image_url
        
        # Save JSON data to a file
        with open(output_file, "w") as file:
            json.dump(card_data, file, indent=2)

        print(f"Food card saved to {output_file}")

        return card_data  # Return the parsed dictionary

    except json.JSONDecodeError:
        raise ValueError("Failed to parse JSON from response.")

def main(image_path):
    model_path = "fic_tuned.h5"  # Path to your saved model
    model = load_trained_model(model_path)
    predicted_class, confidence = predict_food_item(model, image_path)
    print(f"Predicted food item: {predicted_class}")
    print(f"Confidence: {confidence*100:.2f}%")

    prompt = 'You are an AI system designed to generate a structured food item playing card based on an input derived from an image models prediction. The input consists of:  - **food_item_name** (string): The predicted name of the food item from an image recognition model.  - **number_of_items** (integer): The estimated count of the food item, inferred using logical reasoning.  - **estimated_calories** (integer): The estimated total calorie count for the given quantity, based on nutritional data.  ### **Instructions:**  1. **Input Handling:**  - You will receive the **food_item_name** from an image models prediction.  - Your task is to **estimate** the number of items and calorie count using logical reasoning based on portion sizes and typical nutritional values.  2. **Contextual Description:**  - Briefly describe the predicted food item, its general perception (e.g., healthy, indulgent), and common uses. 3. **Card Generation:**  - Generate a structured playing card with the following fields:  - **card_name** (string): A concise, playful name for the card (max 5 words).  - **card_rating** (integer): A **health score from 1 to 10**, where:  - 10 = Extremely healthy (low calories, high nutrition)  - 7–9 = Healthy (good balance of nutrients)  - 4–6 = Moderate (some nutritional benefits but has drawbacks)  - 1–3 = Indulgent (high in calories, low in nutrients)  - **special_technique** (string): A unique move inspired by the food item, similar to Pokémon abilities.  4. **Consistent JSON Format:**  - The final JSON output **must be structured exactly as follows**:  ```json{"card_name": "string","card_rating": integer,"special_technique": "string"} always return this json at the end of your response, this is the food item name you should base your output on:' + ' ' + f'{{"food_item_name": "{predicted_class}"}}'
    response = generate_content(prompt)
    response_text = response.choices[0].message.content
    create_food_card(predicted_class, response_text, "food_card.json")


if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: python identify_food.py <image_path>")
        sys.exit(1)
    image_path = sys.argv[1]
    main(image_path)

    # main('./evaluation/Dessert/0.jpg')
