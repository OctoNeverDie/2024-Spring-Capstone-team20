import openai

def open_file(filepath):
    with open(filepath, 'r', encoding = 'utf-8') as infile:
        return infile.read()

def save_file(filepath, content):
    with open(filepath, 'a', encoding = 'utf-8') as outfile:
        outfile.write(content)

api_key = 'api key'

openai.api_key = api_key

with open("D:/2.project/2024-1/capstone/test2.jsonl", "rb") as file:
    response = openai.File.create(
        file = file,
        purpose = 'fine-tune'
    )

print(response)
