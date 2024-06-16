using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//////////////////using TMPro;            //textmeshpro 사용

public class BoidsManager : MonoBehaviour
{
    public GameObject spawnRange; //스폰 범위: 이 범위 내에서만 스폰됨.
    BoxCollider rangeCollider;  //spawnRange에 적용된 collider
    private int index = 0;      //스폰되는 child prefab
    public Children[] boidPrefab = new Children[10];

    Vector3 RandomPosition() //random spawn
    {
        //spawnrange 위치
        Vector3 rangePosition = spawnRange.transform.position;
        //collider size 가져오기
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPosition = new Vector3(range_X, 0f, range_Z);

        Vector3 spawnPosition = rangePosition + RandomPosition;
        return spawnPosition;
    }

    public Transform targetTransform; //목표물: 산타
    public int boidsNum = 3; //3*3 군집 생성
    public int boidDistance = 2;
    private List<Children> _boids; //군집 안 children 객체 리스트

    public float separationWeight = 1.5f;
    public float cohesionWeight = 1.5f;
    public float alignmentWeight = 1.5f;

    ///////////////public TextMeshProUGUI GameOver;    //GameOver

    private void Awake()
    {
        rangeCollider = spawnRange.GetComponent<BoxCollider>();
    }

    public void selectChild()   //애들 고르기
    {
        _boids = new List<Children>();
            for (int k = 0; k< 2; k++)
            {
                Vector3 random_transform = RandomPosition();
                for (int i = 0; i<boidsNum; i++)
                {
                    for (int j = 0; j<boidsNum; j++)
                    {
                        if (index > 8)
                        {
                            index = 0;
                        }
                        SpawnBoid(boidPrefab[index], random_transform + new Vector3((i - boidsNum / 2) * boidDistance, 0.0f, (j - boidsNum / 2) * boidDistance));
                        index++;
                    }
                }
            }
    }
    void SpawnBoid(Children boidPrefab, Vector3 position)   //boid 스폰
    {
        var boidInstance = Instantiate(boidPrefab, position, new Quaternion());
        boidInstance.GetComponent<Children>().targetTransform = targetTransform;
        boidInstance.GetComponent<Children>()._boids = _boids;
        boidInstance.GetComponent<Children>().separationWeight = separationWeight;
        boidInstance.GetComponent<Children>().cohesionWeight = cohesionWeight;
        boidInstance.GetComponent<Children>().alignmentWeight = alignmentWeight;
        ////////////////boidInstance.GetComponent<Children>().GameOver = GameOver;
        _boids.Add(boidInstance);
    }
}