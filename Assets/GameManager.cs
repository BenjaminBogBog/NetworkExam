using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private float minPipeHeight;
    [SerializeField] private float maxPipeHeight;
    [SerializeField] private GameObject Scroller;
    [SerializeField] private Transform pipeDump;
    [SerializeField] private GameObject pipePrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnNewPipe()
    {
        GameObject pipeClone = Instantiate(pipePrefab, new Vector3(12f, Random.Range(minPipeHeight, maxPipeHeight), 0), Quaternion.identity);
        
        pipeClone.transform.SetParent(pipeDump);
    }
}
