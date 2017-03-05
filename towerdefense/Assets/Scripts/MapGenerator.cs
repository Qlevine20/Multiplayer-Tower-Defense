using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class MapGenerator : NetworkBehaviour {

    public int width;
    public int height;
    private int[,] map;
    public GameObject BuildPiece;
    public GameObject PathLoc;
    public NavMeshSurface nav;
    public int pathsGenerated = 2;
    [SyncVar]
    public int seed;
    public bool generated = false;


    // Use this for initialization


    void Start()
    {
        if (!isServer)
        {
            CmdGenerateSeed(System.DateTime.Now.Millisecond);
        }
        
        Random.InitState(seed);
        nav = GetComponent<NavMeshSurface>();
        map = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                map[i, j] = 1;
            }
        }
        for (int i = 0; i < pathsGenerated; i++)
        {
            GeneratePath();
        }
        GenerateMap();
        generated = true;



    }
	
	// Update is called once per frame
	void Update () {
		
	}


    [Command]
    public void CmdGenerateSeed(int s)
    {
        seed = s;
    }

    void GenerateMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {

                if (map[x, z] == 2)
                {
                    GameObject path = (GameObject)Instantiate(PathLoc, new Vector3(x * (BuildPiece.transform.localScale.x) - (width / 2 * BuildPiece.transform.localScale.x), transform.position.y, (z * BuildPiece.transform.localScale.x) - (height / 2 * BuildPiece.transform.localScale.x)), Quaternion.identity);
                }
                if (map[x, z] == 1)
                {
                    GameObject piece = (GameObject)Instantiate(BuildPiece, new Vector3(x * (BuildPiece.transform.localScale.x) - (width/2 * BuildPiece.transform.localScale.x), transform.position.y, (z * BuildPiece.transform.localScale.x) - (height / 2 * BuildPiece.transform.localScale.x)), Quaternion.identity);
                    piece.transform.parent = transform;
                    
                    bool checkBuild = false;
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (x + i < width && z + j < height && x + i > 0 && z + j > 0)
                            {
                                if (map[x + i, z + j] == 0)
                                {
                                    piece.tag = "buildPlace";
                                    piece.GetComponent<Renderer>().material.color = Color.blue;
                                    checkBuild = true;
                                    break;
                                }
                            }
                            else if (x + i < width && x + i > 0)
                            {
                                if (map[x + i, z] == 0)
                                {
                                    piece.tag = "buildPlace";
                                    piece.GetComponent<Renderer>().material.color = Color.blue;
                                    checkBuild = true;
                                    break;
                                }
                            }
                            else if (z + j > 0 && z + j < height)
                            {
                                if (map[x, z + j] == 0)
                                {
                                    piece.tag = "buildPlace";
                                    piece.GetComponent<Renderer>().material.color = Color.blue;
                                    checkBuild = true;
                                    break;
                                }
                            }

                        }
                        if (checkBuild)
                        {
                            break;
                        }
                    }

                }
            }
        }
    }



    void GeneratePath()
    {
        bool reachedEnd = false;
        int x = width/2;
        int z = 1;
        bool pathLocPlaced = false;
        while (!reachedEnd)
        {
            if (z == height/2 && !pathLocPlaced)
            {
                Debug.Log("place path");
                map[x, z] = 2;
                pathLocPlaced = true;
            }
            else
            {
                map[x, z] = 0;
            }
            
            int xChange = 0;
            int zChange = 0;
            //randomize change in x
            if (z < height - 2)
            {
                if (x + 1 < width - 1 && x - 1 > 1)
                {
                    if (map[x + 1, z] != 0 && map[x - 1, z] != 0)
                    {
                        //Debug.Log("first");

                        xChange = Random.Range(-1, 2);
                    }
                    else if (map[x + 1, z] != 0)
                    {
                        //Debug.Log("second");
                        xChange = Random.Range(0, 2);
                    }
                    else if (map[x - 1, z] != 0)
                    {
                        //Debug.Log("third");
                        xChange = Random.Range(-1, 1);
                    }

                }
                else if (x + 1 < width - 1 && map[x + 1, z] != 0)
                {
                    //Debug.Log("fourth");
                    xChange = Random.Range(0, 2);
                }
                else if (x - 1 > 1 && map[x - 1, z] != 0)
                {
                    //Debug.Log("fifth");
                    xChange = Random.Range(-1, 1);
                }




                //randomize change in z if x was not changed
                if (xChange == 0)
                {
                    zChange = 1;
                }
            }

            //z has reached opponent
            else
            {
                if (x < width/2)
                {
                    xChange = 1;
                }
                else if (x > width/2)
                {
                    xChange = -1;
                }
                else
                {
                    reachedEnd = true;
                } 
            }

            z += zChange;
            x += xChange;


        }
    }
}
