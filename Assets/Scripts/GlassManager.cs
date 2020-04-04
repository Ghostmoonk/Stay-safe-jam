using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GlassManager : MonoBehaviour
{
    // ------------------------ [ Init ] -------------------------------------- \\
    [SerializeField] GameObject[] glassPrefabs; // prefabs de verres (plusieurs ?)
    [SerializeField] SpawnPoint[] spawnPoints; // ou les verres peuvent spawn
    [SerializeField] GameObject glassHolder; // ou on range les instances des verres
    [SerializeField] bool isTheSpawnerStarting;


    // ------------------------ [ Force du verre ] ------------------------------ \\
    private bool isNextGlassJumping;

    // ------------------------ [ Difficultée ] ------------------------------ \\
    public static int nbrOfGlassPerMinutes = 10; // frequence à la minute d'apparition de verres;
    public static int minGlassLifeTime = 8;
    public static int maxGlassLifeTime = 12;
    public static int difficulty = 0;

    // --------------------------- [ Instance ] --------------------------------- \\
    private bool hasPlayerSpawned;
    [SerializeField] PlayerController playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        nbrOfGlassPerMinutes = 20;

        if (isTheSpawnerStarting)
            CreateAGlass();
    }

    //pour créer un verre
    void CreateAGlass()
    {
        //on va chercher un point pour faire spawn
        SpawnPoint spawnpoint = ChooseTargetPointToSpawn();

        //On choisit un verre prefab au hazard
        int rndGlass = Random.Range(0, glassPrefabs.Length);
        bool willJump = Mathf.Round(Random.Range(0, 10-difficulty)) == 0 ? true : false;

        // on crée un verre
        GameObject glassIns = Instantiate(glassPrefabs[rndGlass], transform.position, Quaternion.identity);
        glassIns.GetComponent<GlassBehaviour>().whereItSpawned = spawnpoint;
        glassIns.GetComponent<GlassBehaviour>().isjumpingAtStart = willJump;

        //on lui set son parent
        glassIns.transform.SetParent(glassHolder.transform);

        //on lui set sa position;
        glassIns.transform.position = spawnpoint.transform.position;

        if(!hasPlayerSpawned)
        {
            StartCoroutine(CreatePlayer(spawnpoint.transform.position));
        }

        //on répéte la création x secondes plus tard
        Invoke("CreateAGlass", 60f / nbrOfGlassPerMinutes);
    }

    SpawnPoint ChooseTargetPointToSpawn()
    {
        //on recupere dans un tableau tt les points disponibles 
        List<SpawnPoint> spawnPointsFree = new List<SpawnPoint>();

        foreach(SpawnPoint spawnpoint in spawnPoints)
        {
            if(!spawnpoint.isHavingAGlassAbove)
            {
                spawnPointsFree.Add(spawnpoint);
            }
        }

        //on choisit un chiffre aléatoire pour prendre un point du tableau
        int rndPoint = Random.Range(0, spawnPointsFree.Count);

        //on recupere le spawnpoint correspondant
        SpawnPoint targetSpawnPoint = spawnPointsFree[rndPoint];

        //le spawnpoint devient alors indisponible
        targetSpawnPoint.isHavingAGlassAbove = true;

        // on return un new vector2 avec la position
        return targetSpawnPoint;
    }

    [MenuItem("MenuDebug/GiveDebugInfos")]
    static void DebugInfos()
    {
        Debug.Log("Nombre de verre/mins ="+nbrOfGlassPerMinutes);
        Debug.Log("Diff ="+difficulty);
    }

    IEnumerator CreatePlayer(Vector2 position)
    {
        yield return new WaitForSeconds(3f);
        Instantiate(playerPrefab, new Vector2(position.x, 7f + position.y), Quaternion.identity);
        hasPlayerSpawned = true;
    }
}
