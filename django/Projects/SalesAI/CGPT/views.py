from django.shortcuts import render
from django.http import JsonResponse
from openai import OpenAI
import json
import os
from django.views.decorators.csrf import csrf_exempt

client = OpenAI(api_key = os.environ.get('OPENAI_API_KEY'),)

# 파일에서 시스템 메시지 읽기
def read_system_message(file_path):
    with open(file_path, 'r', encoding='utf-8') as file:
        return file.read()

def get_completion(input):
    print("user"+input)

    system_message_content = read_system_message('CGPT/system_message.txt')

    query = client.chat.completions.create(
        model="gpt-4o-mini",
        messages = [
            {"role": "system", "content":system_message_content},
            {"role": "user", "content": input}
        ],
        max_tokens=1024,
        n=1,
        stop=None,
        temperature=0.5,
    )
    response = query.choices[0].message.content
    print("AI"+response)
    return response

def update_history(prompt, request):
    # 세션에 'chat_history' 키가 없으면 빈 리스트로 초기화
    if 'chat_history' not in request.session:
        request.session['chat_history'] = []

    # 세션에서 대화 히스토리 가져오기
    chat_history = request.session['chat_history']

    # 현재 사용자의 입력을 대화 히스토리에 추가
    chat_history.append({"role": "user", "content": prompt})

    # 대화 히스토리를 세션에 다시 저장
    request.session['chat_history'] = chat_history

    return chat_history


@csrf_exempt
def query_view(request):
    if request.method == 'POST':
        print("Received POST data:", request.body)  # 수신된 데이터 출력
        try:
            data = json.loads(request.body)
            prompt = data.get('request')

            if prompt and prompt.split()[-1].lower() == "clear":
                if 'chat_history' in request.session and request.session['chat_history']:
                    request.session['chat_history'] = []
                    return JsonResponse({'reply': 'Chat history cleared.'})
                else:
                    return JsonResponse({'reply': 'No chat history to clear.'})

            messages = update_history(prompt, request)

            if isinstance(messages, list):
                str_messages = str(messages)
                response = get_completion(str_messages)
                print("뭐야"+response)
                # GPT 모델의 응답을 대화 히스토리에 추가
                # 업데이트된 대화 히스토리를 세션에 저장
                request.session['chat_history'].append({"role": "assistant", "content": response})
                return JsonResponse({'reply': response})

        except json.JSONDecodeError:
            print("에러났다!")
            return JsonResponse({'error': 'Invalid JSON.'}, status=400)

    return render(request, 'index.html')
