using System;
using UnityEngine;

[Serializable]
public class Item
{
    public int id = -1;
    public string name;

    public ItemBuff[] buffs;

    public Item()
    {
        //기본적으로 아이템 생성자는 아이디를 -1로 설정 (비어있는 아이템이라는뜻)
        id = -1;
        name = "";
    }
    public Item(ItemObject itemObject)
    {
        id = itemObject.data.id;
        name = itemObject.name; ;
        
        buffs = new ItemBuff[itemObject.data.buffs.Length];
    }
}
