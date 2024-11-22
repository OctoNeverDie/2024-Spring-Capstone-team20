//using System.Collections.Generic;
//using UnityEngine;

////!!!TODO : 꼭!! 리팩토링 필수!!
//[SerializeField]
//public struct DayMbti
//{
//    public Define.Mbti mbti;
//    public bool isLike;

//    public DayMbti(Define.Mbti mbti, bool isLike)
//    {
//        this.mbti = mbti;
//        this.isLike = isLike;
//    }
//}

//public class NpcMbtiRandomer
//{
//    //private List<Define.Mbti> selectedMbtiList;

//    public List<DayMbti> GetMbti(int day = 0)
//    {
//        //selectedMbtiList = new List<Define.Mbti>();
//        List<DayMbti> dayMbtiList = new List<DayMbti>();
//        switch (day)
//        {
//            case 2:
//                dayMbtiList.Add(new DayMbti(Define.Mbti.emotional, true));
//                dayMbtiList.Add(new DayMbti(Define.Mbti.logical, true));
//                dayMbtiList.Add(new DayMbti(Define.Mbti.emotional, false));
//                //selectedMbtiList = new List<Define.Mbti> { Define.Mbti.emotional, Define.Mbti.logical };
//                break;
//            case 3:
//                dayMbtiList.Add(new DayMbti(Define.Mbti.flatter, true));
//                dayMbtiList.Add(new DayMbti(Define.Mbti.flirter, true));
//                dayMbtiList.Add(new DayMbti(Define.Mbti.flatter, false));
//                //selectedMbtiList = new List<Define.Mbti> { Define.Mbti.flatter, Define.Mbti.flirter };
//                break;
//            default:
//                dayMbtiList.Add(new DayMbti(Define.Mbti.flatter, true));
//                dayMbtiList.Add(new DayMbti(Define.Mbti.emotional, true));
//                dayMbtiList.Add(new DayMbti(Define.Mbti.logical, false));
//                //selectedMbtiList = new List<Define.Mbti> { Define.Mbti.emotional, Define.Mbti.logical, Define.Mbti.flatter, Define.Mbti.flirter };
//                break;
//        }

//        // 요구사항에 따라 3개의 DayMbti를 생성
//        //List<Define.Mbti> dayMbtiList = GenerateDayMbtiList(selectedMbtiList);

//        return dayMbtiList;
//    }

//    /*
//    private List<Define.Mbti> GetDay2()
//    {
//        //랜덤 1. 꼭 들어가는 거 빼고 나머지 뭐할지
//        //int idx = UnityEngine.Random.Range(0, selectedMbtiList.Count);
//        //랜덤 2. 3번 like, dislike 랜덤
//        //랜덤 3. 랜덤 1 거 순서
//        //return selectedMbtiList;
//    }
//    private List<Define.Mbti> GenerateDayMbtiList(List<Define.Mbti> availableMbti)
//    {
//        List<Define.Mbti> result = new List<Define.Mbti>();
//        if (availableMbti.Count < 2)
//        {
//            Debug.LogError("사용 가능한 Mbti가 충분하지 않습니다.");
//            return result;
//        }

//        // 그룹 A에서 하나, 그룹 B에서 하나 선택
//        Define.Mbti selectedA = GetRandomElement(groupAAvailable);
//        Define.Mbti selectedB = GetRandomElement(groupBAvailable);

//        // flags는 최소 하나 true, 하나 false
//        bool flagA = true;
//        bool flagB = false;

//        // 첫 번째 두 요소 추가
//        result.Add(new Define.Mbti(selectedA, flagA));
//        result.Add(new Define.Mbti(selectedB, flagB));

//        // 나머지 하나는 랜덤하게 true 또는 false
//        Define.Mbti selectedC = availableMbti.GetRandomThing();
//        bool flagC = UnityEngine.Random.value > 0.5f;

//        result.Add(new Define.Mbti(selectedC, flagC));

//        // 마지막으로, 리스트가 요구사항을 충족하는지 검증
//        bool hasTrue = false;
//        bool hasFalse = false;
//        HashSet<Define.Mbti> mbtiSet = new HashSet<Define.Mbti>();

//        foreach (var item in result)
//        {
//            if (item.flag) hasTrue = true;
//            else hasFalse = true;
//            mbtiSet.Add(item.mbti);
//        }

//        // true와 false가 모두 존재하지 않으면 수정
//        if (!hasTrue || !hasFalse)
//        {
//            // 예: 마지막 요소의 flag를 반전
//            DayMbti lastItem = result[result.Count - 1];
//            lastItem.flag = !lastItem.flag;
//            result[result.Count - 1] = lastItem;
//        }

//        // Mbti의 다양성이 부족하면 수정
//        if (mbtiSet.Count < 2)
//        {
//            // 다른 Mbti로 교체
//            Define.Mbti newMbti;
//            do
//            {
//                newMbti = availableMbti.GetRandomElement();
//            } while (mbtiSet.Contains(newMbti));

//            // 마지막 요소를 교체
//            result[result.Count - 1] = new DayMbti(newMbti, result[result.Count - 1].flag);
//        }

//        return result;
//    }
//    */
//    // 확장 메서드 대신 클래스 내부에 정의
//    private T GetRandomElement<T>(List<T> list)
//    {
//        if (list == null || list.Count == 0)
//        {
//            Debug.Log("list가 비어 있습니다.");
//            return default(T);
//        }

//        int index = UnityEngine.Random.Range(0, list.Count);
//        return list[index];
//    }
//}
