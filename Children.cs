using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
///////////using TMPro;            //textmeshpro 사용
using UnityEngine.SceneManagement;  //씬 전환용


public class Children : MonoBehaviour
{
    [SerializeField] public Transform targetTransform;
    NavMeshAgent navMeshAgent;

    public List<Children> _boids;
    public float separationWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float alignmentWeight = 1.0f;
    
    private TextScript textScript;

    //////씬 전환용 함수/////////

    void LoadStartScene()              
    {
        SceneManager.LoadScene("GameStart");        //GameStart화면(초기화면)으로 넘어가기
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Santa") // player와 충돌
        {
            textScript.GameOverText();
            ////////////////GameOver.text = "GameOver";       //게임 오버 글씨 띄우기
            Invoke("LoadStartScene", 2f);   
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

        navMeshAgent.Move(steering * Time.deltaTime * 5);
        navMeshAgent.destination = targetTransform.position;
    }
}