using System.Collections.Generic;
using UnityEngine;

public class PositionAdjuster
{
    private readonly List<(float PositionX, float Rotation)> positionData = new List<(float PositionX, float Rotation)> {
     (-26f, 6f),
     (28f, -2.4f)
 };

    private float heightOfElement;
    private float spacing;

    public void InitSpacing(int elementCount, RectTransform rt) {
        spacing = rt.rect.height / (elementCount + 2);

        heightOfElement = 0.0f;
        heightOfElement += spacing * (elementCount / 2 +1) ;
    }

    public void AdjustPos(int alignmentKey, RectTransform item) {
        heightOfElement -= spacing;
        int index = (alignmentKey % 2 == 0) ? 0 : 1;

        //position
        item.anchoredPosition = new Vector2(
            item.anchoredPosition.x + positionData[index].PositionX + Random.Range(-5.0f, 5.0f),
            item.anchoredPosition.y + heightOfElement
        );

        //rotation
        float adjustRotation = positionData[index].Rotation + Random.Range(-2.0f, 2.0f);
        item.rotation = Quaternion.Euler(0, 0, adjustRotation);
    }
}