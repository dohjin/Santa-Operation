using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Santa : MonoBehaviour
{
    private BoidsManager boidsManager;  //children 소환 위한 스크립트

    Animator animator;

    float speed = 10;           //산타 속도 변수 speed
    float rotSpeed = 50;         //산타 회전 속도 변수 rotSpeed

    private PresentControl presentControl; // 선물 찾기 및 배송 위한 스크립트

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //ItemCurrent = GetComponent<TextMeshProUGUI>(); //=inspector에서 컴포넌트 드래그앤드립
        ///////////////////sliderObj.SetActive(false);
        boidsManager = GameObject.Find("BoidsManager").GetComponent<BoidsManager>();
        presentControl = GameObject.Find("PresentControl").GetComponent<PresentControl>();
    }
////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////



    // Update is called once per frame
    void Update()
    {
        /////////산타 조작하기/////////
        var move = speed * Time.deltaTime;
        var rot = rotSpeed * Time.deltaTime;

        var ver = Input.GetAxis("Vertical");
        var hor = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * ver * move);
        transform.Rotate(new Vector3(0, hor * rot, 0));

        animator.SetFloat("speed", Mathf.Abs(ver * move));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
        }
        ////////////산타 조작하기//////////////
    
        //선물 찾기
        if(presentControl.finding == true){
            presentControl.ToFindPresent();
        }
        //선물 배송하기
        else if(presentControl.delivery == true){
            presentControl.ToDelieverPresent();
        }   
    }
}
