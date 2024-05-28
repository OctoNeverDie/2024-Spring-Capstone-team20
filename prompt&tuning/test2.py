import openai

def open_file(filepath):
    with open(filepath, 'r', encoding = 'utf-8') as infile:
        return infile.read()

def save_file(filepath, content):
    with open(filepath, 'a', encoding = 'utf-8') as outfile:
        outfile.write(content)

api_key = 'api 키 입력'

openai.api_key = api_key

#file_id = "file-69J7MsyJFQvXOg4jxmWxLDZ3" #test1
#file_id = "file-4QQ9hj8R088y6xDP2TueRF9M" #test2
#file_id = "file-lHXnP4y9Y6GUXCm2lMCfymwE" #개편된 test1
file_id = "file-xSHZt7XZ8WfDkww3G4LZT7Gz" #개편된 test2
model_name = "ft:gpt-3.5-turbo-0125:octdoc::9FFS6FKS"

response = openai.FineTuningJob.create(
    training_file = file_id,
    model = model_name
)

job_id = response['id']
print(job_id)