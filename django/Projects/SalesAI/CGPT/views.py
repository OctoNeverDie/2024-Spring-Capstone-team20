from django.http import JsonResponse
from openai import OpenAI
import json
import os
import re
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
        model="gpt-4o",
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

#region Mbti
def makeMbtiPrompt(data):
    mbtiInit = getMbti(data)

    mbtiPrompt = []
    path = 'CGPT/prompts/11mbti/'
    dimensions = ['emotional', 'logical', 'flatter', 'flirter']
    
    for dim in dimensions:
        value = mbtiInit.get(dim)
        if value:
            value = value.capitalize()
            if value in ["like", "dislike", "norm"]:
                filename = f"{path}{dim}{value}.txt"
                try:
                    mbtiPrompt.append(read_system_message(filename))
                except FileNotFoundError:
                    print(f"Error: File {filename} does not exist.")
            else:
                print(f"Warning: Unexpected value '{value}' for dimension '{dim}'")
        else:
            print(f"Warning: No value provided for dimension '{dim}'")
        mbtiPrompt.append('\n')
    
    print("MBTI prompt")
    return ''.join(mbtiPrompt)

def getMbti(data):
    mbti_data = {
    'emotional': data.get('emotional'),
    'logical': data.get('logical'),
    'flirter': data.get('flirter'),
    'flatter': data.get('flatter')
    }

    return mbti_data

#endregion

#region Prompt
def init_prompt(mbtiPrompt, npcInit, request):
    clear_everything(request)

    editPrompt(mbtiPrompt, npcInit)
    print({'reply': '@ Init Complete.'})
    return

def editPrompt(mbtiPrompt, npcPrompt):
    path = 'CGPT/prompts/PromptFile.txt'
    systemPrompt = read_system_message(path)
    systemPrompt = editMbtiPrompt(mbtiPrompt, systemPrompt)
    systemPrompt+='\n' + npcPrompt
    overwrite_system_message(path, systemPrompt)
    print(systemPrompt)

def editMbtiPrompt(mbtiPrompt, systemPrompt):
    pattern = '1\\.\\s*감정형.*?4\\.\\s*플러팅.*?감소시킵니다\\.'
    modified_text = re.sub(pattern, mbtiPrompt, systemPrompt, flags=re.DOTALL)
    return modified_text
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

                mbtiPrompt = makeMbtiPrompt(data)
                npcInit = data.get('NpcInit')

                init_prompt(mbtiPrompt, npcInit, request)

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

            else:
                return JsonResponse({'reply' : 'Please Select Proper SendType'})

        except json.JSONDecodeError:
            print("에러났다!")
            return JsonResponse({'reply': 'Invalid JSON.'}, status=400)

    return JsonResponse({'reply': 'Bottom Code'})
