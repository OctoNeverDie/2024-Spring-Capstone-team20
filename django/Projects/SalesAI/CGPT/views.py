import json
import os
from .HistoryManager import init_session, update_history

from openai import OpenAI
from django.http import JsonResponse
from django.views.decorators.csrf import csrf_exempt
from datetime import datetime

client = OpenAI(api_key=os.environ.get('OPENAI_API_KEY'), )

@csrf_exempt
def query_view(request):
    if request.method == 'POST':
        try:
            data = json.loads(request.body)
            userSend = data.get('Request')
            sendType = data.get('SendType')
            
            if sendType == "ChatInit":
                npcInit = data.get('NpcInit')
                init_session(request, npcInit)

                response = get_completion(request, userSend)

                update_history(request, "assistant", response)

                timeMessage(request.session['prompt'])
                return JsonResponse({'reply': response})
            
            elif sendType == "Chatting":
                messages = update_history(request, "user", userSend)
                
                json_messages = json.dumps(messages)
                response = get_completion(request, json_messages)

                update_history(request, "assistant", response)

                timeMessage(request.session['chat_history'])
                return JsonResponse({'reply': response})
                
            else:
                return JsonResponse({'reply' : 'Please Select Proper SendType'})

        except json.JSONDecodeError:
            timeMessage("에러 : JSONDecodeError")
            return JsonResponse({'reply': 'Invalid JSON.'}, status=400)

    return JsonResponse({'reply': 'Bottom Code'})


def get_completion(request, input):
    system_message_content = request.session['prompt']

    query = client.chat.completions.create(
        model="gpt-4o",
        messages=[
            {"role": "system", "content": system_message_content},
            {"role": "user", "content": input}
        ],
        max_tokens=500,
        n=1,
        stop=None,
        temperature=1.05,
        tool_choice=None)
    response = query.choices[0].message.content
    return response

def timeMessage(strMessage):
    timestamp = datetime.now().strftime('[%d/%b/%Y %H:%M:%S]')
    print(f'{timestamp} {strMessage}')