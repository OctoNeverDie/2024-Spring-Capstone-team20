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

# region Init
def init_npc(npc_data, request):  
    global original_decide_prompt
    global original_bargain_prompt
    
    original_decide_prompt = read_system_message('CGPT/decide_system_original.txt')
    original_bargain_prompt = read_system_message('CGPT/bargain_system_original.txt')


    clear_everything(request)

    append_system_message('CGPT/decide_system.txt', npc_data)
    append_system_message('CGPT/bargain_system.txt', npc_data)
    print({'reply': '@ Npc Attached.'})
    return

def init_item(item_data, request):
    append_system_message('CGPT/bargain_system.txt', item_data)
    '''
    if 'concern' in request.session and request.session['concern']:
        first_concern = request.session['concern'][0] 
        lastConversation = "#Last Conversation:\n" + str(first_concern)
    else:
        lastConversation = "#Last Conversation:\n"  # 값이 없을 때 기본값
    append_system_message('CGPT/bargain_system.txt', lastConversation)
    '''
    print({'reply': '@ Item Attached.'})  
    return
# endregion

# region history manage
def clear_everything(request):
    clear_prompt()
    clear_session(request)
    return JsonResponse({'reply': '@ Data cleared.'})

def clear_prompt():
    global original_decide_prompt
    global original_bargain_prompt
    
    if 'original_decide_prompt' not in globals() or 'original_bargain_prompt' not in globals():
        print("init_npc 함수가 호출되지 않아 초기화되지 않았습니다.")
        return
    
    if original_decide_prompt is not None:
        overwrite_system_message('CGPT/decide_system.txt', original_decide_prompt)

    if original_bargain_prompt is not None:
        overwrite_system_message('CGPT/bargain_system.txt', original_bargain_prompt)
    
def clear_session(request):
    print("clear session")
    if 'chat_history' in request.session and request.session['chat_history']:
        request.session['chat_history'] = []
    if 'concern' in request.session and request.session['concern']:
        request.session['concern']=[]

    request.session.modified = True

def update_history(prompt, request, role, sessionKey):
    if sessionKey not in request.session:
        request.session[sessionKey] = []

    sessionLog = request.session[sessionKey]
    sessionLog.append({"role": role, "content": prompt})
    request.session[sessionKey] = sessionLog
    return sessionLog
# endregion

def get_completion(input, sendType):
    if(sendType == "NpcInit"): 
        system_message_content = read_system_message('CGPT/decide_system.txt')
    elif(sendType in ["ItemInit", "ChatBargain", "Endpoint"]):
        system_message_content = read_system_message('CGPT/bargain_system.txt')

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

@csrf_exempt
def query_view(request):
    if request.method == 'POST':
        try:
            data = json.loads(request.body)
            prompt = data.get('Request')
            sendType = data.get('SendType')
            dataInit = data.get('DataInit')
            
            if sendType == "NpcInit":
                init_npc(dataInit, request)
                response = get_completion(prompt, sendType)
                update_history(response, request, "assistant", "concern")
                
                return JsonResponse({'reply': response})
            
            elif sendType == "ItemInit":
                request.session.modified = True

                init_item(dataInit, request)
                response = get_completion(prompt, sendType)
                update_history(response, request, "assistant", "chat_history")

                return JsonResponse({'reply': response})
            
            elif sendType == "ChatBargain":
                messages = update_history(prompt, request, "user", "chat_history")

                if isinstance(messages, list):
                    str_messages = str(messages)
                    response = get_completion(str_messages, sendType)
                    request.session['chat_history'].append({"role": "assistant", "content": response})

                    return JsonResponse({'reply': response})

            elif sendType == "Endpoint" :#prompt : $buy, $reject, $leave, $clear
                response = "$clear"
                if prompt.strip() != "$clear":
                    response = get_completion(prompt, sendType)
                clear_everything(request)
                
                return JsonResponse({'reply': response})

            else:
                return JsonResponse({'reply' : 'Please Select Proper SendType'})

        except json.JSONDecodeError:
            print("에러났다!")
            return JsonResponse({'reply': 'Invalid JSON.'}, status=400)

    return JsonResponse({'reply': 'Bottom Code'})
