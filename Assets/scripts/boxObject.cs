using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxObject : MonoBehaviour
{

    Vector2 targetPos;
    float time;
    bool isMove = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    public void move()
    {
        if (!isMove) return;

        time += Time.deltaTime * 10;
        this.transform.position = Vector2.Lerp(this.transform.position, targetPos, time);
        if(1 < time)
        {
            time = 0;
            isMove = false;
        }
    }

    public void setMovePoint(Vector2 pos)
    {
        targetPos = pos;
        isMove = true;
    }
}
