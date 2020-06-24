using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMove : MonoBehaviour
{
    public float speed = 5.0f;

    private bool isMoving = true;
    private Vector3 direction;

    private Vector3 startPosition;
    private Vector3 goalPosition;
    private bool isGoalSet;//判断目标是否设置好了，不然总是还没设置好目标，就开始移动，导致移动到原点
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        //goalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (isMoving && isGoalSet)
        {
            //Debug.Log("isMoving");
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, goalPosition, speed * Time.deltaTime);
        }
        if(transform.position == goalPosition)
        {
            //Debug.Log("MovingFinish");
            isMoving = false;
            isGoalSet = false;
        }
    }

    //输入两个方向，移动棋子
    public void MoveChess(Vector3 goal)
    {
        isMoving = true;
        //将一开始的位置存起来，

        setGoal(goal);

        //Debug.Log("startPosition" + startPosition);
        //Debug.Log("goalPosition" + goalPosition);

    }

    private void setGoal(Vector3 goal) {
        goalPosition = goal;
        isGoalSet = true;
    }

    
}
