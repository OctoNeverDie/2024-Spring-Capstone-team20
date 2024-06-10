using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

//suggest �տ� �����Ѵ�, price ���� �־�����Ѵ�
public class Test : MonoBehaviour
{
    enum Relation
    {
        crazyLove,
        hotLike,
        like,
        neutral,
        dislike,
        fuckOff
    };

    struct RelationPrice
    {
        public Relation relateLevel;
        public int expectPrice;
        public int plusAlpha;
    }

    RelationPrice relationPrice;
    int affinitySum = 0;
    int usefulnessSum = 0;
    int countConv = 0;

    private void Start()
    {
        ResetRelationPrice(); //�Լ� �ʱ�ȭ
        for (int i = 0; i < 10; i++) {
            bool isContinue= TestMain();
            if (!isContinue) break;
        }
    }
    #region MainTest
    bool TestMain() {

        string userInput = "{ @affinity: +4  @usefulness: +3 @thought: 45 ũ�����̶��? ����� �� �غ��߰ھ�. @reason: �̹� �տ��� ������ ������ ������ ������ ���� ���� ���� @emotion: ���}";

        int affinity = ExtractValue(userInput, "affinity");
        int usefulness = ExtractValue(userInput, "usefulness");

        //Debug.Log("affinity: " + affinity);
        //Debug.Log("usefulness: " + usefulness);
        //--------------------------------------------------------
        addNumber(affinity, ref affinitySum);
        addNumber(usefulness, ref usefulnessSum);
        countConv++;

        //Debug.Log("affinitySum: " + affinitySum);
        //Debug.Log("usefulnessSum: " + usefulnessSum);
        //Debug.Log("countConv: " + countConv);
        //--------------------------------------------------------
        UpdateRelationPrice(affinitySum, usefulnessSum);

        //Debug.Log("expect price: " + relationPrice.expectPrice);
        //Debug.Log("plus alpha: " + relationPrice.plusAlpha);
        //Debug.Log("relation level:" + relationPrice.relateLevel);
        //--------------------------------------------------------
        //�� expectPrice ����
        if (relationPrice.relateLevel == Relation.fuckOff)
        {
            //fuckOff
            string Bye = "����";
            Debug.Log(Bye);
            return false;
        }
        else if (relationPrice.relateLevel == Relation.crazyLove)
        {
            //crazyLove
            string Bye = "�����";
            Debug.Log(Bye);
            return false;
        }
        //--------------------------------------------------------
        else
        {
            int suggestPrice = 30; //suggestPrice = 30�̶�� �� �� ��
                                   //countCon =10�̶�� ���̸� ��, �ƴϸ� �Ʒ�
            bool isOkay = comparePrice(relationPrice.expectPrice, relationPrice.plusAlpha, suggestPrice);
            if (countConv >= 10)
            {
                string Bye = Byebye(isOkay);
                Debug.Log(Bye);
                return false;
            }
            //--------------------------------------------------------
            else
            {
                string codeOutput = IntToString();
                Debug.Log(codeOutput);
                string AIOutput = "��! ���� �� �Ǵ� �Ҹ�!";//AI���� ����
                                                   //�ش� ���� ���� userInput�� ���ؼ� ���
                string FinalOutput = AppendString(userInput, AIOutput);
                Debug.Log(FinalOutput);
                
            }
        }
        Debug.Log(countConv + "------------------------------------------------");
        return true;
    }
    #endregion

    #region String Concatnation
    int ExtractValue(string input, string keyword)
    {
        string pattern = $@"@{keyword}: ?([+-]?\d+)";
        Match match = Regex.Match(input, pattern);
        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value);
        }
        return 0;
    }

    string IntToString() {
        return "{ "+$"@Relationship: {relationPrice.relateLevel}"+
            $" @Expect price: {relationPrice.expectPrice}" +
            $" @Plus alpha: {relationPrice.plusAlpha}"+" }";
    }

    string AppendString(string userInput, string AIOutput) {
        string pattern = @"@\b(?:affinity|usefulness)\b: ?[+-]?\d+";
        string cleanedInput = Regex.Replace(userInput, pattern, "").Trim();
        cleanedInput = Regex.Replace(cleanedInput, @"\s{2,}", " "); // Replace multiple spaces with a single space
        cleanedInput = cleanedInput.Replace("{ ", "{").Replace(" }", "}"); // Clean up extra spaces around braces
        return $"{cleanedInput} {AIOutput}";
    }
    #endregion

    #region ScoreUpdate
    // affinity/usefulness ��� ���ϴ� �Լ�(score : affinitySum)
    void addNumber(int number, ref int numberSum)
    {
        numberSum += number;
    }
    #endregion
    //Relation.fuckOff/crazyLove, plusAlpha=0/-1,expectPrice 0/-1
    #region RelationPrice Update
    void ResetRelationPrice()
    {
        relationPrice.expectPrice = 15;//�� ���ǿ� ���� ������ ����
        relationPrice.plusAlpha = 0;
        relationPrice.relateLevel = Relation.neutral;
    }

    void UpdateRelationPrice(int affinitySum, int usefulnessSum) {
        relationPrice.expectPrice = DecidePrice(usefulnessSum, relationPrice.expectPrice);
        DefineRelation(affinitySum);
    }
    void DefineRelation(int affinitySum) {
        if (affinitySum < -15)
        {
            relationPrice.relateLevel = Relation.fuckOff;
            relationPrice.plusAlpha = 0;
            // TODO : ������ ��ȭ ������ �Լ�
        }
        else if (affinitySum < -10)
        {
            relationPrice.relateLevel = Relation.dislike;
            relationPrice.plusAlpha = (int)(relationPrice.expectPrice * 0.1);
        }
        else if (affinitySum < 0)
        {
            relationPrice.relateLevel = Relation.neutral;
            relationPrice.plusAlpha = (int)(relationPrice.expectPrice * 0.2);
        }
        else if (affinitySum < 10)
        {
            relationPrice.relateLevel = Relation.like;
            relationPrice.plusAlpha = (int)(relationPrice.expectPrice * 0.3);
        }
        else if (affinitySum < 20)
        {
            relationPrice.relateLevel = Relation.hotLike;
            relationPrice.plusAlpha = (int)(relationPrice.expectPrice * 0.4);
        }
        else
        {
            relationPrice.relateLevel = Relation.crazyLove;
            relationPrice.plusAlpha = -1;
        }
    }

    int DecidePrice(int usefulnessSum, int expectPrice) {
        if (usefulnessSum < -15)
        {
            expectPrice = 0;
            // TODO : ������ ��ȭ ������ �Լ�
        }
        else if (usefulnessSum < -10)
        {
            expectPrice = (int)(expectPrice * 0.2);
        }
        else if (usefulnessSum < 0)
        {
            expectPrice = (int)(expectPrice * 1);
        }
        else if (usefulnessSum < 10)
        {
            expectPrice = (int)(expectPrice * 1.2);
        }
        else if (usefulnessSum < 20)
        {
            expectPrice = (int)(expectPrice * 1.5);
        }
        else
        {
            expectPrice = -1;
        }

        return expectPrice;
    }
    #endregion

    #region Result
    string Byebye(bool isOkay)
    {
        string bye;
        if (isOkay) { bye = "��Կ�!"; }
        else { bye = "�Ȼ��"; }
        return bye;
    }

    //suggestPrice�� plusAlpha ��
    bool comparePrice(int expectPrice, int plusAlpha, int suggestPrice)
    {//���õ� ���ݰ� �� �� bool return
        return (expectPrice + plusAlpha) >= suggestPrice;
    }
    #endregion

}
