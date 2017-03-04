using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public int width;
    public int height;
    private int[,] map;
    public GameObject BuildPiece;
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public int pathsGenerated = 2;

    // Use this for initialization
    void Awake () {
        spawnPoint1.transform.position = new Vector3(0, spawnPoint1.transform.position.y, -height / 2);
        spawnPoint2.transform.position = new Vector3(0, spawnPoint2.transform.position.y, height / 2);
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

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void GenerateMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (map[x, z] != 0)
                {
                    GameObject piece = (GameObject)Instantiate(BuildPiece, new Vector3(x - width / 2, transform.position.y, z - height / 2), Quaternion.identity);
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
        Random.InitState(System.DateTime.Now.Millisecond);
        while (!reachedEnd)
        {
            map[x, z] = 0;
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
