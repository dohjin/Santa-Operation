using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //유니티 UI사용
using TMPro;            //textmeshpro 사용


public class TextScript : MonoBehaviour
{
    public TextMeshProUGUI GameStart;      //게임 스타트 UI객체
    public TextMeshProUGUI GameOver;    //GameOver UI 객체

    public TextMeshProUGUI ItemCurrent;    //화면에 1/5 delivery 완료 띄우는 텍스트 ItemCurrent
    public TextMeshProUGUI PressToPutUp; //Z버튼 누르기 띄우는 텍스트 PressToPutUp



    ///////////////첫 화면 GameStart 텍스트 제어
    public IEnumerator FadeTextToZero_GS()  // 알파값 1에서 0으로 전환
    {
        GameStart.color = new Color(GameStart.color.r, GameStart.color.g, GameStart.color.b, 1);
        while (GameStart.color.a > 0.0f)
        {
            GameStart.color = new Color(GameStart.color.r, GameStart.color.g, GameStart.color.b, GameStart.color.a - (Time.deltaTime / 1.5f));
            yield return null;
        }
    }

    ///////////////오른쪽 위 텍스트 제어 ItemCurrent
    public void GetText(int getCount)         //GetText()함수는 현재까지 수집한 선물개수를 써주는 역할
    {
        ItemCurrent.text = getCount.ToString() + " / 5 Found";
    }
    public void DeliverText(int getCount)      //DeliverText()함수는 현재까지 배송한 선물개수를 써주는 역할
    {
        ItemCurrent.text = getCount.ToString() + " / 5 Delivered";
    }
    public void ClearText() 
    {
        ItemCurrent.text = "Game Clear!";       //Delivery Start글씨 띄우기
    }
    public void DeliveryStartText()
    {
        ItemCurrent.text = "Delivery Start!";       //Delivery Start글씨 띄우기
    }

    //////////////////////선물 줍는 키 설명 텍스트 제어 PressToPutUp
    public void PutUpText(){
        PressToPutUp.text = "Press \"Z\" To Put Up Gift"; 
    }
    public void DelPutUpText(){
        PressToPutUp.text = "";         //Z버튼 누르라는 글씨 없애기
    }
    

    //////////////////////게임 오버 / 게임 클리어 텍스트 제어 GameOver
    public void GameOverText()
    {
        GameOver.text = "GameOver";       //게임 오버 글씨 띄우기
    }

}