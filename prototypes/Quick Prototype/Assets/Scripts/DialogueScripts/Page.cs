using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class Page
{
    public string pageName;
    public string charName;
    [TextArea(3,10)]
    public string dialogueText;
    public List<string> responses;
    public List<string> responseNextPages;
}
