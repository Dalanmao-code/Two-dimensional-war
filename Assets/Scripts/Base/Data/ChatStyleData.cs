using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ChatStyleData", menuName = "ScriptableObject/聊天npc数据", order = 0)]
public class ChatStyleData : ScriptableObject
{
    public List<ChatStyle> ChatStyleList; //聊天的格式
}
