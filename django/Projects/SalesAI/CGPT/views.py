import json
import os
from openai import OpenAI
from django.http import JsonResponse
from django.views.decorators.csrf import csrf_exempt
from datetime import datetime

client = OpenAI(api_key=os.environ.get('OPENAI_API_KEY'), )
cleanSystem = ""


file_path = 'CGPT/prompts/MuhanPromptFile.txt'
with open(file_path, 'r', encoding='utf-8') as f:
    muhan_system_message_content = f.read()

def timeMessage(strMessage):    
    # 현재 시간을 원하는 형식으로 포맷합니다.
    timestamp = datetime.now().strftime('[%d/%b/%Y %H:%M:%S]')

    # 시간과 메시지를 함께 출력합니다.
    print(f'{timestamp} {strMessage}')
    

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

# region history manage
def clear_everything(request):
    clear_prompt()
    clear_session(request)
    return JsonResponse({'reply': '@ Data cleared.'})

def clear_prompt():
    origin = read_system_message('CGPT/prompts/originalPromptFile.txt')
    overwrite_system_message('CGPT/prompts/PromptFile.txt', origin)
    
def clear_session(request):
    if 'chat_history' in request.session and request.session['chat_history']:
        request.session['chat_history'] = []

def update_history(prompt, request, role, sessionKey):
    if sessionKey not in request.session:
        request.session[sessionKey] = []

    sessionLog = request.session[sessionKey]
    sessionLog.append({"role": role, "content": prompt})
    request.session[sessionKey] = sessionLog
    return sessionLog
# endregion


def get_completion(input):
    path = 'CGPT/prompts/PromptFile.txt'
    system_message_content = read_system_message(path)

    query = client.chat.completions.create(
        model="gpt-4",
        messages=[
            {"role": "system", "content": system_message_content},
            {"role": "user", "content": input}
        ],
        max_tokens=500,
        n=1,
        stop=None,
        temperature=0.5,
        tool_choice=None)
    response = query.choices[0].message.content
    return response

def get_completion_muhan(input):
    timeMessage(input)
    query = client.chat.completions.create(
        model="gpt-4o",
        messages=[
            {"role": "system", "content": muhan_system_message_content},
            {"role": "user", "content": input}
        ],
        max_tokens=3000,
        n=1,
        stop=None,
        temperature=0.5
    )
    response = query.choices[0].message.content
    timeMessage(response)
    return response

#region Prompt
def init_prompt(npcInit, request):
    clear_everything(request)

    editPrompt(npcInit)
    timeMessage('@ Init Complete.')
    return

def editPrompt(npcPrompt):
    path = 'CGPT/prompts/PromptFile.txt'
    systemPrompt = read_system_message(path)
    systemPrompt+='\n' + npcPrompt
    overwrite_system_message(path, systemPrompt)
    timeMessage(systemPrompt)
# endregion

@csrf_exempt
def query_view(request):
    if request.method == 'POST':
        try:
            data = json.loads(request.body)
            userSend = data.get('Request')
            sendType = data.get('SendType')
            
            if sendType == "ChatInit":
                clear_everything(request)
                
                npcInit = data.get('NpcInit')

                init_prompt(npcInit, request)

                response = get_completion(userSend)
                update_history(response, request, "assistant", "chat_history")
                
                return JsonResponse({'reply': response})
            
            elif sendType == "Chatting":
                messages = update_history(userSend, request, "user", "chat_history")

                if isinstance(messages, list):
                    str_messages = str(messages)
                    response = get_completion(str_messages)
                    request.session['chat_history'].append({"role": "assistant", "content": response})

                    return JsonResponse({'reply': response})

            elif sendType == "MuhanInit":
                timeMessage(userSend)
                response = get_completion_muhan(userSend)
                timeMessage("이제 집 보냄!" + response)
                return JsonResponse({'reply' : response})

            else:
                return JsonResponse({'reply' : 'Please Select Proper SendType'})

        except json.JSONDecodeError:
            timeMessage("에러났다!")
            return JsonResponse({'reply': 'Invalid JSON.'}, status=400)

    return JsonResponse({'reply': 'Bottom Code'})
