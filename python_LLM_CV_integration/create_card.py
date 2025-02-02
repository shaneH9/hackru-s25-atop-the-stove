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

# get the response
def generate_content(prompt):
  response = client.chat.completions.create(
      messages = [{'role':'user','content':prompt}],
      model = 'llama3-70b-8192'
      # swap model, based on needs, ex:
      # llama3-70b-8192
      # llama-3.1-8b-instant
  )
  return response

def get_unsplash_image_url(query):
    base_url = "https://api.unsplash.com/search/photos"
    params = {
        "query": query,
        "client_id": unsplash_access_key,
        "per_page": 1 
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


def create_food_card(food_item_name, response_text, output_file):
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
        with open(output_file, "w") as file:
            json.dump(card_data, file, indent=2)
        # print(f"Food card saved to {output_file}")
        return card_data  
    except json.JSONDecodeError:
        raise ValueError("Failed to parse JSON from response.")

def main(food_item_name, number_of_items, estimated_calories, output_file):
    prompt =  'You are an AI system designed to generate a structured food item playing card based on an input containing:  - **food_item_name** (string): The name of the food item.  - **number_of_items** (integer): The quantity of the food item.  - **estimated_calories** (integer): The total estimated calories for the given quantity.  ### **Instructions:**  1. **Contextual Description**:  - Briefly describe the food item, its general perception (e.g., healthy, indulgent), and common uses.  2. **Card Generation** (Ensure strict adherence to the following JSON format):  - **card_name** (string): A concise, playful name for the card (max 5 words).  - **card_rating** (integer): A **health score from 1 to 10**, where:  - 10 = Extremely healthy (low calories, high nutrition)  - 7–9 = Healthy (good balance of nutrients)  - 4–6 = Moderate (some nutritional benefits but has drawbacks)  - 1–3 = Indulgent (high in calories, low in nutrients)  - **special_technique** (string): A unique move inspired by the food item, similar to Pokémon abilities.  3. **Consistent JSON Format**:  - The final JSON output **must be structured exactly as follows**:  ```json{"card_name": "string","card_rating": integer,"special_technique": "string"} always return this json at the end of your response, these are the inputs:' + ' ' + f'{{"food_item_name": "{food_item_name}","number_of_items": {number_of_items},"estimated_calories": {estimated_calories}}}'
    response = generate_content(prompt)
    response_text = response.choices[0].message.content
    create_food_card(food_item_name, response_text, output_file)
    # card_data = parse_food_card(response_text, output_file)
    # print(card_data)

if __name__ == "__main__":
    if len(sys.argv) != 5:
        print("Usage: python create_card.py <food_item_name> <number_of_items> <estimated_calories> <output_file>")
        sys.exit(1)
    
    food_item_name = sys.argv[1] # str
    number_of_items = int(sys.argv[2]) # str
    estimated_calories = int(sys.argv[3]) # int
    output_file = sys.argv[4]
    
    # main('pizza', 2, 400, 'food_card_test1.json')
    
    main(food_item_name, number_of_items, estimated_calories, output_file)