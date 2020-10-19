using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;

public class inGameMgr : MonoBehaviour
{
    public int moveCount = 10;
    public int moveSpeed = 10;

    dialogMgr dialogMgr;
    GameObject player;
    Vector2 targetMove;
    Vector2 powerVector = new Vector2(0, 0);
    float time = 0f;
    float power = 1f;
    bool isMove = false;
    bool isDialog = false;

    List<List<TileBase>> fields = new List<List<TileBase>>();
    List<GameObject> boxList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        dialogMgr = GameObject.Find("Dialog").GetComponent<dialogMgr>();
        setField();
        Screen.SetResolution(1280, 720, false);
        //setDialog("Test");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void setField()
    {
        Tilemap tilemap = transform.Find("Stage").Find("Grid").Find("Tilemap").gameObject.GetComponent<Tilemap>();
        Vector3Int size = tilemap.size;
        //TileBase[] allTiles = tilemap.GetTilesBlock(bounds);


        for (int x = 0, stack = 0; x < size.x; x++)
        {
            fields.Add(new List<TileBase>());
            for (int y = 0; y < size.y; y++, stack++)
            {
                fields[x].Add(tilemap.GetTile(new Vector3Int(x, y, 0)));
                //player pos
                TileBase plpos = tilemap.GetTile(new Vector3Int(x, y, 1));
                if (plpos != null)
                {
                    switch (plpos.name)
                    {
                        case "playerStartPoint":
                            //delete point and create player prefab(to do)
                            tilemap.SetTile(new Vector3Int(x, y, 1), null);

                            player = (GameObject)Instantiate(Resources.Load("prefab/player", typeof(GameObject)));
                            player.transform.position = new Vector2(x, y);
                            player.transform.SetParent(transform.Find("Stage").transform);

                            Debug.Log("PlayerPos::" + x + "," + y);
                            break;
                        case "box":
                            tilemap.SetTile(new Vector3Int(x, y, 1), null);

                            GameObject box = (GameObject)Instantiate(Resources.Load("prefab/box", typeof(GameObject)));
                            box.transform.position = new Vector2(x, y);
                            box.transform.SetParent(transform.Find("Stage").transform);
                            boxList.Add(box);
                            break;
                    }
                }
            }
        }
    }
    void setDialog(string path)
    {
        isDialog = true;
        dialogMgr.runDialog(path);
    }
    void Move()
    {
        if (isDialog) return;
        if (isMove)
        {
            Tile tile = (Tile)fields[(int)targetMove.x][(int)targetMove.y];
            GameObject targetBox = null;
            boxList.ForEach((box) => {
                if (box.transform.position.Equals(targetMove))
                {
                    targetBox = box;
                    return;
                }
            });

            if (tile == null || targetBox != null)
            {
                Debug.Log("Cant move Field");
                time = 0;
                isMove = false;
                if (targetBox != null)
                {
                    //TO Do :: BOX MOVE 
                    //TO Do :: Check boxmove
                    Debug.Log("BOX!");
                    Vector2 boxPos = targetMove + powerVector;
                    Tile tileForbox = (Tile)fields[(int)boxPos.x][(int)boxPos.y];
                    bool cantMove = false;
                    boxList.ForEach((box) => {
                        if (box.transform.position.Equals(boxPos))
                        {
                            cantMove = true;
                            return;
                        }
                    });

                    if (tileForbox == null || cantMove)
                    {
                        Debug.Log("Cant Move Box Blocking!");
                        return;
                    }
                    targetBox.GetComponent<boxObject>().setMovePoint(boxPos);
                }
                return;
            }

            time += Time.deltaTime * moveSpeed;
            player.transform.position = Vector2.Lerp(player.transform.position, targetMove, time);
            if (1 <= time)
            {
                time = 0;
                isMove = false;
            }

        }
        else if (Input.anyKeyDown && !isMove)
        {
            targetMove = player.transform.position;
            isMove = true;
            if (Input.GetKeyDown(KeyCode.UpArrow)) { targetMove.y++; powerVector.Set(0, power); }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) { targetMove.y--; powerVector.Set(0, -power); }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) { targetMove.x--; powerVector.Set(-power, 0); }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) { targetMove.x++; powerVector.Set(power, 0); }
            else isMove = false;
        }
        
    }
}
