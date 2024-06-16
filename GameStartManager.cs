using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //유니티 UI사용
using TMPro;            //textmeshpro 사용



public class GameStartManager : MonoBehaviour
{
    private TextScript textScript;

    void Start()
    {
        textScript = GameObject.Find("TextScript").GetComponent<TextScript>();
        StartCoroutine(textScript.FadeTextToZero_GS());
    }

}