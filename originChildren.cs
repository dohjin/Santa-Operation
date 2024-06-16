using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/////////////////////using TMPro;            //textmeshpro 사용
using UnityEngine.SceneManagement;  //씬 전환용

public class originChildren : MonoBehaviour
{
    //[SerializeField] public Transform targetTransform;
    NavMeshAgent navMeshAgent;
    public Transform santa_transform;

    float speed = 5.5f;
    float rotSpeed = 50.0f;

    public List<Children> _boids;
    public float separationWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float alignmentWeight = 1.0f;

    private TextScript textScript;


    //씬 전환용 함수//
    void LoadStartScene()
    {
        SceneManager.LoadScene("GameStart");        //GameStart화면(초기화면)으로 넘어가기
    }

    float Return_RotSpeed() //랜덤 회전 값 return 랜덤하게 돌아다니게 하기 위해
    {
        rotSpeed = Random.Range(0.0f, 30.0f) + 3.0f;

        return rotSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Santa") // 산타와 충돌한다면
        {
            /////////////GameOver.text = "GameOver";       //게임 오버 글씨 띄우기
            textScript.GameOverText();
            Invoke("LoadStartScene", 2f);        //2초 지연시키기
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        textScript = GameObject.Find("TextScript").GetComponent<TextScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //떨어지지 않게
        //..떨어지면 올리거나 벽 세워서 충돌 확인해야 할 듯


        float separationRadius = 5;
        float cohesionRadius = 20;
        //default vars

        var steering = Vector3.zero;

        var separationDirection = Vector3.zero;
        var separationCount = 0;
        var alignmentDirection = Vector3.zero;
        var alignmentCount = 0;
        var cohesionDirection = Vector3.zero;
        var cohesionCount = 0;

        foreach (Children boid in _boids)
        {
            //skip self
            if (boid == this)
                continue;
            var distance = Vector3.Distance(boid.transform.position, this.transform.position);

            if (distance < separationRadius)
            {
                separationDirection += (transform.position - boid.transform.position) / distance;
                separationCount++;
            }

            //identify local neighbour
            if (distance < cohesionRadius)
            {
                alignmentDirection += boid.transform.forward;
                alignmentCount++;

                cohesionDirection += boid.transform.position;
                cohesionCount++;
            }

        }

        if (separationCount > 0)
            separationDirection /= separationCount;

        if (alignmentCount > 0)
            alignmentDirection /= alignmentCount;

        if (cohesionCount > 0)
            cohesionDirection /= cohesionCount;

        alignmentDirection -= transform.forward;

        cohesionDirection = cohesionDirection - transform.position;

        steering += separationDirection * separationWeight;
        steering += alignmentDirection * alignmentWeight;
        steering += cohesionDirection * cohesionWeight;
        steering.Normalize();


        float distance_range = Vector3.Distance(transform.position, santa_transform.position);
        //print(distance_range);
        if (distance_range < 8.5f)
        {
            navMeshAgent.enabled = true;
            navMeshAgent.Move(steering * Time.deltaTime);
            navMeshAgent.destination = santa_transform.position;
        }
        else
        {
            navMeshAgent.enabled = false;
            rotSpeed = Return_RotSpeed();
            var move = speed * Time.deltaTime;
            var rot = rotSpeed * Time.deltaTime;

            transform.Translate(Vector3.forward * move);
            transform.Rotate(new Vector3(0, rot, 0));
        }
    }
}