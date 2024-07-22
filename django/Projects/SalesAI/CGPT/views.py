from django.shortcuts import render
from django.http import JsonResponse
from rest_framework.decorators import api_view
from openai import OpenAI
import os



client = OpenAI(
    api_key = os.environ.get('OPENAI_API_KEY'),
)

@api_view(['GET'])
def get_completion(prompt):
    print("user"+prompt)
    query = client.chat.completions.create(
        model="gpt-4o-mini",
        messages = [
            {"role": "system", "content":"너는 내 파이썬 선생님이야. 모든 말 이후에 python 히히! 라고 말을 붙여야해. 예시 : 그렇습니다. python 히히!"},
            {"role": "user", "content": prompt}
        ],
        max_tokens=1024,
        n=1,
        stop=None,
        temperature=0.5,
    )
    response = query.choices[0].message.content
    print("AI"+response)
    return response


def query_view(request):
    if request.method == 'POST':
        prompt = request.POST.get('prompt')
        prompt = str(prompt)
        response = get_completion(prompt)
        return JsonResponse({'response': response})
    return render(request, 'index.html')
