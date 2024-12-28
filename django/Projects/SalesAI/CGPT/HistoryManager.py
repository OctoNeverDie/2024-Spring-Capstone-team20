from . import SystemMessageManager as SMM

def init_session(request, npcPrompt):
    init_key_session(request, 'chat_history')
    init_key_session(request, 'prompt')

    edit_prompt(request, npcPrompt)

def edit_prompt(request, npcPrompt):
    path = 'CGPT/prompts/originalPromptFile.txt'
    request.session['prompt'] = SMM.read_system_message(path)
    request.session['prompt'] += str(npcPrompt)

def init_key_session(request, key):
    request.session[key] = []

def update_history(request, role, input):
    request.session['chat_history'].append( role + " : " + input + "\n")
    return str(request.session['chat_history'])