using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  //씬 전환용

public class PresentControl : MonoBehaviour{

    private TextScript textScript;
    private BoidsManager boidsManager;

    void Start(){
        sliderObj.SetActive(false);
        textScript = GameObject.Find("TextScript").GetComponent<TextScript>();
        boidsManager = GameObject.Find("BoidsManager").GetComponent<BoidsManager>();
    }

    public GameObject Player;        //거리계산 산타 오브젝트 Player
    public int getCount = 0;           //현재까지 얻은 선물의 개수 getCount 0으로 초기화


    ////////////////FOR FINDING PRESENTS////////////////////////////
    public GameObject[] Presents = new GameObject[5];    //거리 계산 선물들 저장할 배열 Presents
    public GameObject prefab;   //선물 제거용 빈 객체 prefab
    public float distance_1;  //선물<>산타 거리 계산
    public bool[] alreadyGet = { false, false, false, false, false }; //이미 찾은 선물 배열에 저장

    public bool finding = true;   //선물 찾고있는지에 대한 변수 finding



    ///////////////FOR DELIVERING PRESENTS//////////////////////////
    public Transform[] house = new Transform[5];    //선물 나타날 위치 ~ 집 문 앞
    public GameObject presentPre;   //선물 생성용 객체 presentPre
    public float distance_2;    //문앞<>산타 거리 계산
    public bool[] alreadyD = { false, false, false, false, false }; //이미 배송한 선물 배열에 저장

    public Slider sliderD;  //배송 게이지 UI
    public GameObject sliderObj;    //배송 게이지 GameObject 게이지 안 보이게 하려고...
    float ING = 0f; //배송 게이지

    public bool delivery = false;  //선물 배송하고 있는지에 대한 변수 delivery


    //////////////////////////////////////찾기///////////////////////////////////
    ////////선물 찾으러 다닐때 호출되는 거리계산 함수/////////
    public int ReturnPresentToGet(GameObject[] Presents)
    {
        for (int i = 0; i < 5; i ++)
        {
            textScript.DelPutUpText();         //Z버튼 누르라는 글씨 없애기
            if(getCount >=  5)     //찾은 개수가 5보다 커질때 즉, 다 찾았을때
            {
                textScript.DeliveryStartText(); //Delivery Start! 텍스트 띄우기
                getCount = 0;           //찾은 선물 개수 초기화 -> 배송할 선물 개수로 사용
                finding = false;        //찾기 종료
                delivery = true;        //배송 시작
            }
            if (alreadyGet[i] == false)
            {
                //각 선물과의 거리 모두 계산
                distance_1 = Vector3.Distance(Player.transform.position, Presents[i].transform.position);

                //계산 도중 일정 거리 이하만큼 가까워지면
                if (distance_1 <= 3.0f)
                {
                    textScript.PutUpText(); //Z버튼 눌러서 선물 얻으라는 글씨 띄우기
                    return i; //선물 배열 인덱스 값 반환
                }
            }
        }
        
        return -1;
    }

    public void ToFindPresent(){
        ///////////선물 찾으러 다니기/////////
        int index = ReturnPresentToGet(Presents);       //거리 계산반환 함수 호출
        if (index != -1)
        {
            //z키 활성화
            if (Input.GetKeyDown(KeyCode.Z))            //Z버튼이 눌리면
            {
                prefab = Presents[index];      //prefab에 Present[index]찾아서 참조하도록함
                Destroy(prefab);                    //prefab객체 제거시키기
                getCount += 1;  //getCount 1 증가시키기
                textScript.GetText(getCount); //1/5 Found 화면상에 글씨 띄우기
                alreadyGet[index] = true;
            }

        }
    }
    ////////선물 찾으러 다니기/////////


    ///////////////////////////////////////배송///////////////////////////////////////////
    ////////선물 배송할때 호출되는 거리계산 함수/////////
    public int ReturnHouseToDeliver(Transform[] house)
    { 
        for(int i = 0; i <5; i++)
        {
            textScript.DelPutUpText();         //Z버튼 누르라는 글씨 없애기
            if (getCount >= 5)     //배송한 개수가 5보다 커질 때 즉, 다 배송완료했을때!!!!!
            {
                textScript.ClearText();       //Delivery Start글씨 띄우기
                SceneManager.LoadScene("GameClear");        //GameClear화면(엔딩)으로 넘어가기
                
            }
            if (alreadyD[i] == false)
            {
                //각 집 문 앞과의 거리를 모두 계산
                distance_2 = Vector3.Distance(Player.transform.position, house[i].position);

                //계산 도중 일정 거리 이하만큼 가까워지면
                if(distance_2 <= 3.0f)
                {
                    textScript.PutUpText(); //Z버튼 눌러서 선물 얻으라는 글씨 띄우기
                    return i;   //집 배열 인덱스 값 반환
                }
            }
        }
        return -1;
    }

    public void ToDelieverPresent(){
        ///////선물 배송하러 다니기//////////
            int index = ReturnHouseToDeliver(house);
            if(index != -1)
            {
                //Z키 활성화
                if(Input.GetKey(KeyCode.Z))  
                {
                    ////////Z버튼이 눌리면//////////
                    sliderObj.SetActive(true);      //슬라이더 띄우기
                    ING += Time.deltaTime;              //시간에 따라,,,
                    sliderD.value = (float)ING / 2;

                    //print(ING);
                    if (ING >= 2)
                    {
                        //문앞 position(아까 앞에서 할당한 값)에 선물 나타남.
                        getCount += 1;  //getCount 1 증가시키기
                        textScript.DeliverText(getCount); //1/5 Delivered 화면상에 글씨 띄우기

                        Instantiate(presentPre, house[index].position, new Quaternion());
                        alreadyD[index] = true;
                        boidsManager.selectChild();
                        sliderObj.SetActive(false);
                        ING = 0;
                        sliderD.value = 0;
                    }
                }
            }
            else if(index == -1)
            {
                sliderObj.SetActive(false);
                ING = 0;
                sliderD.value = 0;
            }
        ///////////선물 배송하러 다니기////////////
    }

}