using System;
using UnityEngine;

//열거형
public enum CharaterAttribute
{
    HP,
    Cost
}

[Serializable]
public class ItemBuff
{
    //캐릭터의 어떤 수치에 영향을 주는지 선택하는 변수
    public CharaterAttribute state;
    //수치의 변화량
    int value;

}
