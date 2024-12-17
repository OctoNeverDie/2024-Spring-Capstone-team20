# AI NPC 설득 판매 게임 : 길거리 판매왕
## 프로젝트 소개

<span style='background-color: #fff5b1'>동적인 NPC 상호작용</span>을 원하는 유저를 위한 <span style='background-color: #fff5b1'>OpenAI와 STT 기술</span>을 활용한 
**AI🤖 NPC👤 설득💬 토킹👄 게임🎮**

>🐙문어 기업에 취업한 당신! 🥕 중고 거래 고객들을 빼앗자! 
과연 우리의 신입은 🥕 판매자인 척 위장하여 문어 기업의 물건을 팔고 해고를 면할 수 있을까?😳
<br>

## 목차
1. [프로젝트 구조](#1️⃣-프로젝트-구조)
2. [코드 설명](#2️⃣-코드-설명)
3. [프로젝트 사전 설치](#3️⃣-프로젝트-사전-설치)
4. [프로젝트 빌드 방법](#4️⃣-프로젝트-빌드-방법)
5. [프로젝트 실행 방법](#5️⃣-프로젝트-실행-방법)
6. [포스터](#6️⃣-요약-포스터)

---
### 1️⃣ 프로젝트 구조

#### 상위 폴더
`SalesKing` 는 front인 Unity project<br>
`django` 는 back인 Django project

#### 🎨 SalesKing : Unity
|유니티 프로젝트 Script 폴더|
| - |
|![](https://velog.velcdn.com/images/dubidubob/post/4709e387-4070-487c-ab6d-6f62adf3e125/image.png)|
|- ChatSystem : **Npc와 대화**를 관리하는 스크립트 폴더* NPC : **npc mesh 관리 및 3d 오브젝트의 이동 및 위치**를 관리하는 스크립트 폴더<br><br>- Player:  **Player의 위치 및 카메라를 관리**하는 스크립트 폴더<br><br> - Server : **django server**에 user input 등을 전송하는 스크립트 폴더<br><br>- STT : **user 마이크에서 음성**을 가져와 server에 넘기는 스크립트 폴더<br><br>- Data : **json 파일** 속 정보를 리스트 등의 자료구조로 바꿔 runtime에 올리는 스크립트 폴더|

Assets/Resources/Data/JsonFile에는 아이템, NPC 정보 json 파일이 담겨있다.
#### 💻 django : Django
|CGPT 폴더|config 폴더|
| - | - |
|![](https://velog.velcdn.com/images/dubidubob/post/bc7cd400-1b1f-4234-9e04-d064bf8e895c/image.png)|![](https://velog.velcdn.com/images/dubidubob/post/13bfb9c1-8a37-448f-b619-d2e2f1a70d47/image.png)|
* CGPT 폴더 : 게임이 사용할 앱의 내용
* views.py : gpt에 api를 요청하는 파일
* prompts 폴더 : gpt에 전송하는 prompt 파일 모아두는 폴더
* config 폴더 : 해당 django 프로젝트 기본 설정
* requirements.txt : python-dotenv, openai 등 django 서버 필요 라이브러리 요구 사항이 들어있음.

---
### 2️⃣ 코드 설명
#### Client
◾ UnityWebRequest 사용
> Unity to 장고 서버 통신을 위해 UnityWebRequest 클래스를 사용한다. Coroutine을 이용해 Django 서버에서 반응이 오기까지 기다린다. ResultInfo 클래스를 이용한 네트워크 에러 체크 후 각각 반응을 처리한다. 
Npc와의 채팅, 무한 Npc 모드 시 Npc 생성, 플레이어 음성 처리 시 사용한다.

```
protected virtual ResultInfo ResultCheck(UnityWebRequest req)
{
    ResultInfo res;
    switch (req.result)
    {
        case UnityWebRequest.Result.Success:
...
```

```
protected virtual IEnumerator SendRequest(string url, SendType sendType, JObject jobj, Action<ResultInfo> onSucceed, Action<ResultInfo> onFailed, Action<ResultInfo> onNetworkFailed)
{
    //check network connection
    yield return StartCoroutine(CheckNextwork());

    using (var req = new UnityWebRequest(url, sendType.ToString()))
    ...

```  
----
◾ JsonConvert 사용
> Json Data를 runtime 활용 자료구조에 넣기 위해 Newtonsoft.Json를 활용한다. Json 파일을 load해 동일한 구조의 class로 list에 deserialize한다. 혹은, string을 list화 하기 위해 중간에 json으로 변환, deserialize한다.
Npc, Item Data 로드, Gpt 답변 가공할 때 사용한다.

```
Loader LoadJson<Loader, DataFormat>(string path) where Loader : ILoader<DataFormat>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/JsonFile/{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
```
```
// JSON 객체에서 Npcs 배열 추출
var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(npcsStr);
var npcsArray = jsonObject["Npcs"].ToString();
```
---
◾ getByteFromAudioClip 함수 작성
> 프로젝트 실행 시 입력받을 마이크 ID를 설정해두고, 유저의 음성 input을 Unity에서 지원하는 AudioClip 형식으로 입력받는다. 그 후 getByteFromAudioClip 함수를 호출하여 헤더를 붙이고 바이트 형식으로 변환하여 Clova에 request를 보낸다.

```
private byte[] getByteFromAudioClip(AudioClip audioClip)
{
    MemoryStream stream = new MemoryStream();
    const int headerSize = 44;
    ushort bitDepth = 16;

    int fileSize = audioClip.samples * BlockSize_16Bit + headerSize;

    // audio clip의 정보들을 file stream에 추가(링크 참고 함수 선언)
    WriteFileHeader(ref stream, fileSize);
    WriteFileFormat(ref stream, audioClip.channels, audioClip.frequency, bitDepth);
    WriteFileData(ref stream, audioClip, bitDepth);

    // stream을 array형태로 바꿈
    byte[] bytes = stream.ToArray();

    return bytes;
}
```
---

#### Back
◾ request.session 사용
> Gpt가 전 대화를 기억하게 하기 위해 request.session을 사용한다. request.session의 고유 key를 이용해 유저별 대화 세션을 구분한다. 유저의 key가 request body의 key value로 들어오면, 해당하는 request.session을 탐색해 prompt, request.session, user input을 조합해 gpt api로 보낸다. user input과 gpt output을 해당 세션에 추가하여 업데이트한다.

```
def update_history(prompt, request, role, sessionKey):
    if sessionKey not in request.session:
        request.session[sessionKey] = []

    sessionLog = request.session[sessionKey]
    sessionLog.append({"role": role, "content": prompt})
    request.session[sessionKey] = sessionLog
    return sessionLog
```
---
### 3️⃣ 프로젝트 사전 설치
: How to install
* Unity Hub
* Unity Editor 2022.3.21f1

|Installs에서 Install Editor를 눌러|*Unity 2022.3.21f1*를 다운로드 받는다.| 
 | - | - |
 |![](https://velog.velcdn.com/images/dubidubob/post/4967ab37-2a03-4e83-a733-b109502e5f89/image.png)|![](https://velog.velcdn.com/images/dubidubob/post/4967ab37-2a03-4e83-a733-b109502e5f89/image.png)|![](https://velog.velcdn.com/images/dubidubob/post/a3d6d701-42b9-4136-b545-08c4efa1691a/image.png)|
 
이후 Project에서 Add 버튼을 눌러 2024-spring-capstone-team20\SalesKing 파일 선택 후 Unity Editor에서 해당 파일을 클릭한다.

---
### 4️⃣ 프로젝트 빌드 방법
: How to BUILD
![](https://velog.velcdn.com/images/dubidubob/post/a279a8b2-b60b-4635-bdc1-1137f72fa357/image.png)Unity Editor에서 Edit - Build Setting에 들어가 Build를 클릭![](https://velog.velcdn.com/images/dubidubob/post/73e23a5c-d1a4-4e2f-ab7a-808a9249e156/image.png)폴더 선택 후에 빌드가 된다.![](https://velog.velcdn.com/images/dubidubob/post/6b8959e3-59a8-4412-8d11-aafc757c7975/image.png)만들어진 폴더에서 .exe 파일을 눌러 게임 실행

---
### 5️⃣ 프로젝트 실행 방법
: How to TEST
시작 화면에서 스토리 모드 / 무한 모드를 클릭하여 시작.

조작법 : 기본 이동키는 WASD, 상호작용키는 E, 태블릿 정보키는 Tab
<br>
스토리 모드 : 12명의 Npc간 관계가 있고, 완성된 스토리를 체험할 수 있는 모드.
<br>
무한 모드 : OpenAI가 생성한 Npc와 단적으로 물건을 판매하는 모드.

세부 사항은 다음 링크를 참고<br>
https://emerald-blender-41c.notion.site/15f360ae54ce8009b3d3ee1c583c3b11?pvs=4

---
### 6️⃣ 요약 포스터
![20-무너지지않는문어-포스터파일-이주연](https://github.com/user-attachments/assets/d31c2e21-7742-4da7-a4d4-450fe0819593)
