from groq import Groq
from dotenv import load_dotenv
import os
import re
import json
import sys
import requests

# required dependencies: python, re, json, os, sys, requests, groq (pip install groq), dotenv/load_dotenv (pip install python-dotenv)

# setup api's
load_dotenv()
groq_api_key = os.getenv('GROQ_API_KEY')
if not groq_api_key:
    raise ValueError("GROQ_API_KEY not found in .env file")
os.environ['GROQ_API_KEY'] = groq_api_key
unsplash_access_key = os.getenv('UNSPLASH_API_KEY')
if not unsplash_access_key:
    raise ValueError("UNSPLASH_API_KEY not found in .env file")
# os.environ['UNSPLASH_API_KEY'] = unsplash_access_key

client = Groq()

# Get the response from Groq
def generate_content(prompt):
    response = client.chat.completions.create(
        messages=[{'role': 'user', 'content': prompt}],
        model='llama3-70b-8192'  # Swap model based on your needs
    )
    return response

# Fetch image URL from Unsplash based on food item name
def get_unsplash_image_url(query):
    base_url = "https://api.unsplash.com/search/photos"
    params = {
        "query": query,
        "client_id": unsplash_access_key,
        "per_page": 1  # Fetch one image
    }
    response = requests.get(base_url, params=params)
    
    if response.status_code == 200:
        data = response.json()
        if data["results"]:
            return data["results"][0]["urls"]["regular"]  # Extract the image URL
        else:
            return None  # No image found
    else:
        raise ValueError(f"Error fetching image: {response.status_code}, {response.text}")

# Create and save the food card with image URL
def create_food_card(food_item_name, response_text, output_file):
    print("AI Response:", response_text)  # Debugging: Print the AI response

    # Attempt to find and extract JSON block from the AI's response
    json_match = re.search(r'```json\n({.*?})\n```', response_text, re.DOTALL)
    if not json_match:
        raise ValueError("JSON block not found in response.")

    json_data = json_match.group(1)

    try:
        card_data = json.loads(json_data)
        image_url = get_unsplash_image_url(food_item_name)
        if image_url:
            card_data["image_url"] = image_url
        else:
            card_data["image_url"] = None

        return card_data  # Return the generated card data
    except json.JSONDecodeError:
        raise ValueError("Failed to parse JSON from response.")

# Main function to generate two food cards based on input and AI suggestion
def main(food_item_name, number_of_items, estimated_calories, output_file):
    # Generate the first card based on the input food
    prompt_input = f'''
    You are an AI system designed to generate a structured food item playing card based on an input containing:
    - **food_item_name** (string): The name of the food item.
    - **number_of_items** (integer): The quantity of the food item.
    - **estimated_calories** (integer): The total estimated calories for the given quantity.

    ### **Instructions:**
    1. **Special Technique**: 
        - Briefly describe the food item, its general perception (e.g., healthy, indulgent), and special technique
        that lists the strengths and weaknesses of this dish based on the previous attributes.
    2. **Card Generation** (Ensure strict adherence to the following JSON format):
        - **card_name** (string): A concise, playful name for the card (max 5 words).
        - **card_rating** (integer): A **health score from 1 to 10**, where:
          - 10 = Extremely healthy (low calories, high nutrition)
          - 7–9 = Healthy (good balance of nutrients)
          - 4–6 = Moderate (some nutritional benefits but has drawbacks)
          - 1–3 = Indulgent (high in calories, low in nutrients)
        - **special_technique** (string): A unique move inspired by the food item, similar to Pokémon abilities.
    3. **Consistent JSON Format**:
        - The final JSON output **must be structured exactly as follows**:
        ```json
        {{
          "card_name": "string",
          "card_rating": integer,
          "special_technique": "string"
        }}
        ```

    Always return this JSON at the end of your response. These are the inputs:
    {{ "food_item_name": "{food_item_name}", "number_of_items": {number_of_items}, "estimated_calories": {estimated_calories} }}
    '''

    # Get response for the first card based on the input food item
    response_input = generate_content(prompt_input)
    response_text_input = response_input.choices[0].message.content
    card_data_input = create_food_card(food_item_name, response_text_input, output_file)

    # Now, generate an AI suggestion for an alternative food
    prompt_alternative = f'''
    You are an AI system designed to generate a structured food item playing card based on an input containing:
    - **food_item_name** (string): The name of the food item (must use the food item name or relate to real-world food)
    - **number_of_items** (integer): The quantity of the food item.
    - **estimated_calories** (integer): The total estimated calories for the given quantity.

    ### **Instructions:**
    1. **Special Technique**: 
        - Briefly describe the food item, its general perception. Then detail its special technique
        it must have a clear strength and weakness based on the previous attributes. The description
        should be engaging and provide a good understanding of the food item.
    2. **Card Generation** (Ensure strict adherence to the following JSON format):
        - **card_name** (string): A concise, playful name for the card (max 5 words).
        - **card_rating** (integer): A **health score from 1 to 10**, where:
          - 10 = Extremely healthy (low calories, high nutrition)
          - 7–9 = Healthy (good balance of nutrients)
          - 4–6 = Moderate (some nutritional benefits but has drawbacks)
          - 1–3 = Indulgent (high in calories, low in nutrients)
        - **special_technique** (string): A unique move inspired by the food item, similar to Pokémon abilities.
    3. **Consistent JSON Format**:
        - The final JSON output **must be structured exactly as follows**:
        ```json
        {{
          "card_name": "string",
          "card_rating": integer,
          "special_technique": "string"
        }}
        ```

    Suggest a healthy alternative to {food_item_name}. Ensure it follows the same JSON structure.
    The alternative food should be healthy and fit the same criteria as the input food.
    The response must return a valid JSON block that looks exactly like this:
    ```json
    {{
      "card_name": "string",
      "card_rating": integer,
      "special_technique": "string"
    }}
    ```
    '''

    # Get response for the alternative suggestion
    response_alternative = generate_content(prompt_alternative)
    response_text_alternative = response_alternative.choices[0].message.content
    card_data_alternative = create_food_card(food_item_name, response_text_alternative, output_file)
    # New image url for alternative food
    card_data_alternative["image_url"] = get_unsplash_image_url(card_data_alternative["card_name"])

    # Combine both cards in a list
    cards_data = [card_data_input, card_data_alternative]

    # Save both cards to a single JSON file
    with open(output_file, "w") as file:
        json.dump(cards_data, file, indent=2)

    print(f"Two food cards saved to {output_file}")
    return cards_data  # Return both generated cards

if __name__ == "__main__":
    if len(sys.argv) != 5:
        print("Usage: python create_card.py <food_item_name> <number_of_items> <estimated_calories> <output_file>")
        sys.exit(1)
    
    food_item_name = sys.argv[1]  # str
    number_of_items = int(sys.argv[2])  # int
    estimated_calories = int(sys.argv[3])  # int
    output_file = sys.argv[4]

    main(food_item_name, number_of_items, estimated_calories, output_file)
