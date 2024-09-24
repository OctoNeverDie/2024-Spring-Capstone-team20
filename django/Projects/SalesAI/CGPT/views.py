from django.http import JsonResponse
from openai import OpenAI
import json
import os
from django.views.decorators.csrf import csrf_exempt

client = OpenAI(api_key=os.environ.get('OPENAI_API_KEY'), )
cleanSystem = ""

# region System Message Manager

def read_system_message(file_path):
    with open(file_path, 'r', encoding='utf-8') as file:
        return file.read()
    
def append_system_message(file_path, text_to_append):
    with open(file_path, 'a', encoding='utf-8') as file:
        file.write(text_to_append)

def overwrite_system_message(file_path):
    with open(file_path, 'w', encoding='utf-8') as file:
        global cleanSystem
        print(cleanSystem)
        file.write(cleanSystem)

# endregion

# region Data Manager
def init_datas(itemAndnpc):  
    global cleanSystem
    cleanSystem = read_system_message('CGPT/system_message.txt')

    append_system_message('CGPT/system_message.txt', itemAndnpc)
    return JsonResponse({'reply': 'Item, Npc Attached.'})


def clear_datas(request):
    overwrite_system_message('CGPT/system_message.txt')
    print(read_system_message('CGPT/system_message.txt'))

    if 'chat_history' in request.session and request.session['chat_history']:
        request.session['chat_history'] = []
        return JsonResponse({'reply': 'Chat history cleared.'})
    else:
        return JsonResponse({'reply': 'No chat history to clear.'})

def update_history(prompt, request):
    if 'chat_history' not in request.session:
        request.session['chat_history'] = []

    chat_history = request.session['chat_history']
    chat_history.append({"role": "user", "content": prompt})
    request.session['chat_history'] = chat_history
    return chat_history
# endregion

def get_completion(input):
    print("user" + input)

    system_message_content = read_system_message('CGPT/system_message.txt')

    query = client.chat.completions.create(
        model="gpt-4o-mini",
        messages=[
            {"role": "system", "content": system_message_content},
            {"role": "user", "content": input}
        ],
        max_tokens=500,
        n=1,
        stop=None,
        temperature=0.5,
    )
    response = query.choices[0].message.content
    print("AI" + response)
    return response

@csrf_exempt
def query_view(request):
    if request.method == 'POST':
        try:
            data = json.loads(request.body)
            prompt = data.get('Request')
            sendType = data.get('SendType')

            if sendType == "Clear":
                return clear_datas(request)

            elif sendType == "Init":
                return init_datas(prompt)

            elif sendType == "Chat":
                messages = update_history(prompt, request)

                if isinstance(messages, list):
                    str_messages = str(messages)
                    response = get_completion(str_messages)
                    
                    request.session['chat_history'].append({"role": "assistant", "content": response})
                    return JsonResponse({'reply': response})
            else:
                return JsonResponse('Please Select Proper SendType')

        except json.JSONDecodeError:
            print("에러났다!")
            return JsonResponse({'error': 'Invalid JSON.'}, status=400)

    return JsonResponse({'reply': 'Bottom Code'})
    #return render(request, 'index.html')
