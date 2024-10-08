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

def overwrite_system_message(file_path, original_prompt):
    with open(file_path, 'w', encoding='utf-8') as file:

        file.write(original_prompt)

# endregion

# region Data Manager
def init_npc(npc_data):  
    global original_decide_prompt
    global original_bargain_prompt
    original_decide_prompt = read_system_message('CGPT/decide_system.txt')
    original_bargain_prompt = read_system_message('CGPT/bargain_system.txt')

    append_system_message('CGPT/decide_system.txt', npc_data)
    append_system_message('CGPT/bargain_system.txt', npc_data)
    return JsonResponse({'reply': '@ Npc Attached.'})

def init_item(item_data):
    append_system_message('CGPT/bargain_system.txt', item_data)
    return JsonResponse({'reply': '@ Item Attached.'})  

def clear_everything(request):
    print(original_decide_prompt)
    print(original_bargain_prompt)
    overwrite_system_message('CGPT/decide_system.txt', original_decide_prompt)
    overwrite_system_message('CGPT/bargain_system.txt', original_bargain_prompt)

    clear_chatHistory(request)
    return JsonResponse({'reply': '@ Data cleared.'})
    
def clear_chatHistory(request):
    if 'chat_history' in request.session and request.session['chat_history']:
        request.session['chat_history'] = []
        #return JsonResponse({'reply': 'Chat history cleared.'})
    #else:
        #return JsonResponse({'reply': 'No chat history to clear.'})

def update_history(prompt, request):
    if 'chat_history' not in request.session:
        request.session['chat_history'] = []

    chat_history = request.session['chat_history']
    chat_history.append({"role": "user", "content": prompt})
    request.session['chat_history'] = chat_history
    return chat_history
# endregion

def get_completion(input, sendType):
    if(sendType == "ChatSale"): 
        system_message_content = read_system_message('CGPT/decide_system.txt')
    elif(sendType in ["ChatBargain", "Endpoint"]):
        system_message_content = read_system_message('CGPT/bargain_system.txt')

    query = client.chat.completions.create(
        model="gpt-4o-mini",
        messages=[
            {"role": "system", "content": system_message_content},
            {"role": "user", "content": input}
        ],
        max_tokens=500,
        n=1,
        stop=None,
        temperature=0.5
    )
    response = query.choices[0].message.content
    return response

@csrf_exempt
def query_view(request):
    if request.method == 'POST':
        try:
            data = json.loads(request.body)
            prompt = data.get('Request')
            sendType = data.get('SendType')
            print(prompt)
            print(sendType)

            if sendType == "NpcInit":
                return init_npc(prompt)
            
            elif sendType == "ItemInit":
                clear_chatHistory(request)
                return init_item(prompt)
            
            elif sendType in ["ChatSale", "ChatBargain"]:
                messages = update_history(prompt, request)

                if isinstance(messages, list):
                    str_messages = str(messages)
                    response = get_completion(str_messages, sendType)
                    
                    request.session['chat_history'].append({"role": "assistant", "content": response})
                    return JsonResponse({'reply': response})

            elif sendType == "Endpoint" :#prompt : $buy, $reject, $leave, $clear
                response = "$clear"
                if prompt.strip() != "$clear":
                    response = get_completion(prompt ,sendType)
                clear_everything(request)
                return JsonResponse({'reply': response})

            else:
                return JsonResponse({'reply' : 'Please Select Proper SendType'})

        except json.JSONDecodeError:
            print("에러났다!")
            return JsonResponse({'reply': 'Invalid JSON.'}, status=400)

    return JsonResponse({'reply': 'Bottom Code'})
