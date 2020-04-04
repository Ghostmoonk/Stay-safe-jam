using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsulesSpawner : MonoBehaviour
{
    private static CapsulesSpawner _instance;
    public static CapsulesSpawner Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] float spawnRate;
    [SerializeField] bool shouldSpawnCapsules;

    #region Components
    Collider2D spawnArea;
    #endregion
    float areaHeight;

    [SerializeField] GameObject capsulePrefab;

    private void OnEnable()
    {
        //Pour connaître la hauteur de la zone de spawn
        spawnArea = GetComponent<Collider2D>();
        areaHeight = spawnArea.bounds.max.y - spawnArea.bounds.min.y;
    }
    private void Start()
    {
        StartCoroutine(WaitSpawnCapsule());
    }

    //Fait spawn une capsule toutes les *spawnRate* secondes
    IEnumerator WaitSpawnCapsule()
    {
        while (shouldSpawnCapsules)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnCapsule(GetRandomSpawnPosition());
        }
    }

    void SpawnCapsule(Vector3 position)
    {
        GameObject capsuleToInstantiate = Instantiate(capsulePrefab, position, Quaternion.identity, GameObject.FindGameObjectWithTag("CapsulesHolder").transform);
        //Debug.Log(Mathf.InverseLerp(0f, areaHeight, position.y - spawnArea.bounds.min.y));
        if (Mathf.InverseLerp(0f, areaHeight, position.y - spawnArea.bounds.min.y) <= 0.3f)
        {
            capsuleToInstantiate.GetComponent<Capsule>().charges = 1;
        }
        else if (Mathf.InverseLerp(0f, areaHeight, position.y - spawnArea.bounds.min.y) > 0.3f && Mathf.InverseLerp(0f, areaHeight, position.y - spawnArea.bounds.min.y) <= 0.6f)
        {
            capsuleToInstantiate.GetComponent<Capsule>().charges = 2;
        }
        else if (Mathf.InverseLerp(0f, areaHeight, position.y - spawnArea.bounds.min.y) > 0.6f)
        {
            capsuleToInstantiate.GetComponent<Capsule>().charges = 3;
        }
    }

    Vector2 GetRandomSpawnPosition()
    {
        float randXPos = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        float randYPos = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        return new Vector2(randXPos, randYPos);
    }

}
