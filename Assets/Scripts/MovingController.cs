using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingController : MonoBehaviour
{
    /**棋盘布局
     * 0    1   2   3
     * 4    5   6   7
     * 8    9   10  11
     * 12   13  14  15
     * */

    //表示编号为0的棋子的坐标位置
    private int startX = -7;
    private int startY = 3;

    //表示棋子对象预制体
    public GameObject[] ChessPrefab;

    //表示棋子的对象
    private GameObject[] Chess = new GameObject[16];
    //存储已经使用过的棋子
    private List<int> usedChess = new List<int>(16);
    //当前0所在的位置
    private int currentZero;
    private int initZero;

    //表示当前矩阵
    private List<int> initMatrix = new List<int>(16);
    //表示目标矩阵
    private List<int> goalMatrix = new List<int> { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,0};

    private int[,] position = new int[16,2];

    //深度限制
    private int limit,leafLevel;
    //找到在 maxlevel步内的最优解
    public static int maxLevel = 30;
    //记录下所有的步骤,0,1,2,3分别代表上，下，左，右
    //private List<int> rec;
    private List<int> rec = new List<int>(maxLevel+10);

    public Text hasSolutionText;
    public Text maxLevelText;
    public Text findSolutionText;

    public bool isMoving = false;

    private int animationCount =0;

    /**功能简介
     * 1.随机出一个初始棋盘（在16个格子里面找出一个空格子，其余位置随便摆放棋子
     * 2.判断该棋盘能否完成15数码
     * 3.如果可以完成，则自动移动棋子，使其完成15数码
     * 4.如果不能完成，反馈给玩家。
     * */
    // Start is called before the first frame update
    void Start()
    {
        //init();
        //create();
        //System.Threading.Thread.Sleep(5000); //参数为ms 即左边暂停2s
        //findSolution();
        //Debug.Log("是否有解： "+ hasSolution());
        //moveAnimation();
        
        //moveToBlank(p + 1);
        
        //print();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //在屏幕上显示15个棋子
    //并且将usedChess这个List中填入16个数字，list下标顺序对应每一个数字，其中没有数字的那个位置填为0
    public void init()
    {
        destroyAll();
        findSolutionText.text = "";
        hasSolutionText.text = "";
        //定义当前的XY坐标位置
        Vector2 currentXY = new Vector2(startX,startY);
        //int currentX,currentY;
        //currentX = startX;
        //currentY = startY;
        
        //产生一个随机数，表示棋盘的哪一个位置是空的
        int r = Random.Range(0, 16);

        //初始化之前清空所有棋子
        usedChess.Clear();

        //在其他不为空的位置，每一个位置随机摆上棋子，而且每一个随机的棋子和之前不能重复
        for(int i = 0; i < 16; i++)
        {
            if (i != r)
            {
                //随机一个1-15的数字，代表棋子
                int chess = Random.Range(1, 16);
                //这段据说是判断List里面是否存在某个值的代码（网上copy的
                //如果list里面已经存在这个值，就再次随机，如果没有，跳出循环，将该数字加入list
                while (usedChess.Exists(o => o == chess))
                {
                    chess = Random.Range(1, 16);
                }
                usedChess.Add(chess);
                position[chess, 0] = i / 4;
                position[chess, 1] = i % 4;
                //Debug.Log("i=" + i + ",position = " + position[chess, 0] + "," + position[chess, 1]);

                //实例化相应的数字棋子
                Chess[i] = Instantiate(ChessPrefab[chess - 1], currentXY, transform.rotation);
            }
            else
            {
                usedChess.Add(0);
                currentZero = i;
                position[0, 0] = i / 4;
                position[0, 1] = i % 4; 
            }
            //修改下一个实例化的棋子
            if ((i - 3) % 4 == 0)
            {
                currentXY.x = startX;
                currentXY.y -= 2;
            }
            else
            {
                currentXY.x += 2;
            }


        }
    }

    public void create()
    {
        animationCount = 0;
        rec.Clear();
        usedChess.Clear();
        usedChess.Add(1);
        usedChess.Add(2);
        usedChess.Add(3);
        usedChess.Add(4);
        usedChess.Add(9);
        usedChess.Add(5);
        usedChess.Add(7);
        usedChess.Add(8);
        usedChess.Add(0);
        usedChess.Add(6);
        usedChess.Add(10);
        usedChess.Add(12);
        usedChess.Add(13);
        usedChess.Add(14);
        usedChess.Add(11);
        usedChess.Add(15);

        /*position[0, 0] = 1;
        position[0, 1] = 0;

        position[1, 0] = 0;
        position[1, 1] = 0;

        position[2, 0] = 0;
        position[2, 1] = 1;

        position[3, 0] = 0;
        position[3, 1] = 2;

        position[4, 0] = 0;
        position[4, 1] = 3;

        position[5, 0] = 1;
        position[5, 1] = 1;

        position[6, 0] = 2;
        position[6, 1] = 1;

        position[7, 0] = 1;
        position[7, 1] = 2;

        position[8, 0] = 1;
        position[8, 1] = 3;

        position[9, 0] = 2;
        position[9, 1] = 0;

        position[10, 0] = 2;
        position[10, 1] = 2;

        position[11, 0] = 3;
        position[11, 1] = 2;

        position[12, 0] = 2;
        position[12, 1] = 3;

        position[13, 0] = 3;
        position[13, 1] = 0;

        position[14, 0] = 3;
        position[14, 1] = 1;

        position[15, 0] = 3;
        position[15, 1] = 3;*/

        destroyAll();
        findSolutionText.text = "";
        hasSolutionText.text = "";
        initMatrix = usedChess;
        //定义当前的XY坐标位置
        Vector2 currentXY = new Vector2(startX, startY);
        

        //在其他不为空的位置，每一个位置随机摆上棋子，而且每一个随机的棋子和之前不能重复
        for (int i = 0; i < 16; i++)
        {
            if (usedChess[i] != 0)
            {
                //实例化相应的数字棋子
                Chess[i] = Instantiate(ChessPrefab[usedChess[i] - 1], currentXY, transform.rotation);
                position[usedChess[i], 0] = i / 4;
                position[usedChess[i], 1] = i % 4;
            }
            else
            {
                initZero = i;
                currentZero = i;
                position[0, 0] = i / 4;
                position[0, 1] = i % 4;
            }
            //修改下一个实例化的棋子
            if ((i - 3) % 4 == 0)
            {
                currentXY.x = startX;
                currentXY.y -= 2;
            }
            else
            {
                currentXY.x += 2;
            }


        }
    }

    //设初始状态0所在的行数为i,目标状态0所在的行数为j，两者之差的绝对值为k。
    //若k为奇数，则两个矩阵相应的逆序数的奇偶性相异才有解。
    //若k为偶数，则两个矩阵的逆序数必须相同才有解。
    //不是上述两种情况即为无解。通过初始判定就可以不用搜索就能直接否定无解情况。
    private bool hasSolution()
    {
        //初始矩阵就是usedChess
        /**目标矩阵 0在第四行
         * 1    2   3   4
         * 5    6   7   8
         * 9    10  11  12
         * 13   14  15  0
         * */
        int k=0;
        int inversionNumber = 0;
        for(int i = 0; i < 16; i++)
        {
            if(usedChess[i] == 0)
            {
                currentZero = i;
                k = Mathf.Abs( i / 4 + 1 -4);
                //Debug.Log("k = " + k);
            }
        }
        //计算逆序数
        for(int i = 0; i < 16; i++)
        {
            if (usedChess[i] == 0) continue;
            for(int j = i; j < 16; j++)
            {
                if (usedChess[j] == 0) continue;
                //如果后面的数字比前面的数字大，则逆序数+1
                if (usedChess[j] < usedChess[i])
                {
                    inversionNumber++;
                }
            }
        }
        Debug.Log("逆序数为" + inversionNumber);
        //k为偶数
        if(k%2 == 0)
        {
            //逆序数相同
            if (inversionNumber %2 == 0)
                return true;
            //不同
            else return false;
        }

        //k为奇数
        else
        {
            //逆序数奇偶性相同,即都为偶数,则有解
            if (inversionNumber % 2 != 0)
                return true;
            //否则无解
            else return false;
        }
        
    }

    //将第n个棋子移动到空白位置，并实时更新当前的位置信息
    private void moveToBlank(int sequence)
    {
        Vector3 direction;
        //待移动的棋子和空位 有四种情况
        switch(sequence - currentZero)
        {
            case 1:
                direction = Vector3.left;
                break;
            case -1:
                direction = Vector3.right;
                break;
            case 4:
                direction = Vector3.up;
                break;
            case -4:
                direction = Vector3.down;
                break;
            default:
                direction = Vector3.down;
                break;
        }
        //Debug.Log(direction);
        //Vector3 goal = Chess[sequence].transform.position + direction * 2;
        //Chess[sequence].GetComponent<ChessMove>().MoveChess(goal);

        //交换sequence和0的行列值
        int tempr = position[usedChess[sequence], 0];
        int tempc = position[usedChess[sequence], 1];

        position[usedChess[sequence], 0] = position[0, 0];
        position[usedChess[sequence], 1] = position[0, 1];

        position[0, 0] = tempr;
        position[0, 1] = tempc;
        
        //将usedChess中0和sequence两个数字交换
        usedChess[currentZero] = usedChess[sequence];
        usedChess[sequence] = 0;
       
        currentZero = sequence;
        
    }
    private void moveToBlankWithAnimation(int sequence)
    {
        Vector3 direction;
        //待移动的棋子和空位 有四种情况
        switch (sequence - initZero)
        {
            case 1:
                direction = Vector3.left;
                break;
            case -1:
                direction = Vector3.right;
                break;
            case 4:
                direction = Vector3.up;
                break;
            case -4:
                direction = Vector3.down;
                break;
            default:
                direction = Vector3.down;
                break;
        }
        //Debug.Log(direction);
        Vector3 goal = Chess[sequence].transform.position + direction * 2;
        Chess[sequence].GetComponent<ChessMove>().MoveChess(goal);
        initZero = sequence;
    }
    public int fv(List<int> c,List<int> g)
    {//曼哈顿距离
        int cost = 0;
        //计算1-15每一个数字的曼哈顿距离
        for (int i = 0; i < 16; i++)
        {
            if(c[i] != 0)
            {
                //找到目标矩阵中和相同数字所在的位置
                for (int j = 0; j < 16; j++)
                {
                    if (c[i] == g[j])
                    {
                        cost += Mathf.Abs(Mathf.Abs(i / 4 - (g[j]-1) / 4) + Mathf.Abs(i % 4 - (g[j]-1) % 4));
                        //Debug.Log("ci,gj " + c[i]+ " "+g[j]);
                        break;
                    }
                }
            }
        }
        return cost;
    }
    private bool dfs(int level,int pmove)
    {
        int val;
        //如果当前层数等于深度限制
        if(level == limit)
        {
            //计算当前矩阵的曼哈顿距离，如果距离等于0，说明15数码已经完成，返回true
            val = fv(usedChess, goalMatrix);
            if(val == 0)
            {
                leafLevel = level;
                return true;
            }
            
            return false;
        }

        int newsr, newsc;
        int rawsr, rawsc;

        for (int i = 0; i < 4; i++)
        {
            //if (pmove + i == 3 && level > 0) continue;
            //获取0的行和列
            rawsr = position[0,0];
            rawsc = position[0,1];
            newsr = rawsr;
            newsc = rawsc;
            //循环4次。分别将0向上下左右移动一次
            switch (i)
            {
                case 0:
                    newsr = rawsr - 1;
                    newsc = rawsc;
                    break;
                case 1:
                    newsr = rawsr + 1;
                    newsc = rawsc;
                    break;
                case 2:
                    newsr = rawsr;
                    newsc = rawsc - 1;
                    break;
                case 3:
                    newsr = rawsr;
                    newsc = rawsc + 1;
                    break;
            }
            //如果移动过后的0 还在0-3区间内，即没有超出边界 即可。
            if (0 <= newsr && newsr < 4 && 0 <= newsc && newsc < 4)
            {
                switch (i)
                {
                    case 0:
                        Debug.Log("往上走");
                        moveZero(Vector3.up);
                        break;
                    case 1:
                        Debug.Log("往下走");
                        moveZero(Vector3.down);
                        break;
                    case 2:
                        Debug.Log("往左走");
                        moveZero(Vector3.left);
                        break;
                    case 3:
                        Debug.Log("往右走");
                        moveZero(Vector3.right);
                        break;
                }
                val = fv(usedChess,goalMatrix);
                //如果移动成功，则记录下步骤
                if (level + val <= limit)
                {
                    Debug.Log("成功，记录");
                    Debug.Log("level = " + level);
                    rec.Add(i);//记录移动步骤
                    if (dfs(level + 1, i)) return true;
                }
                //没有移动成功，再移动回去
                switch (i)
                {
                    case 0:
                        Debug.Log("往上走失败，撤销");
                        moveZero(Vector3.down);
                        break;
                    case 1:
                        Debug.Log("往下走失败，撤销");
                        moveZero(Vector3.up);
                        break;
                    case 2:
                        Debug.Log("往左走失败，撤销");
                        moveZero(Vector3.right);
                        break;
                    case 3:
                        Debug.Log("往右走失败，撤销");
                        moveZero(Vector3.left);
                        break;
                }
            }

        }
        return false;
    }

    //由于每一次移动0都会动，所以相当于每一次都在移动0
    //函数功能：输入一个方向x，将这个方向上的棋子的坐标和0的交换，并将该棋子朝x的反方向移动
    private void moveZero(Vector3 direction)
    {
        //判断能不能移动的语句写在主函数里面了，因此这里不需要判断
        //如果0要往左走，那么就是0左边的数字移到空白处，即序列数-1
        if(direction == Vector3.left)
        {
            moveToBlank(currentZero - 1);
        }
        else if(direction == Vector3.up)
        {
            moveToBlank(currentZero - 4);
        }
        else if(direction == Vector3.down)
        {
            moveToBlank(currentZero + 4);
        }
        else
        {
            moveToBlank(currentZero + 1);
        }
    }

    private void moveZeroWithAnimation(Vector3 direction)
    {
        //判断能不能移动的语句写在主函数里面了，因此这里不需要判断
        //如果0要往左走，那么就是0左边的数字移到空白处，即序列数-1
        if (direction == Vector3.left)
        {
            moveToBlankWithAnimation(initZero - 1);
        }
        else if (direction == Vector3.up)
        {
            moveToBlankWithAnimation(initZero - 4);
        }
        else if (direction == Vector3.down)
        {
            moveToBlankWithAnimation(initZero + 4);
        }
        else
        {
            moveToBlankWithAnimation(initZero + 1);
        }
    }
    private void print()
    {
        for (int i = 0; i < 16; i++)
        {
            Debug.Log(i+"的位置" + position[i, 0] +","+ position[i, 1]);
        }
    }

    public void findSolution()
    {
        findSolutionText.text = "正在寻找可用解";
        leafLevel = -1;
        Debug.Log(hasSolution());
        //如果初始矩阵有解
        if (hasSolution())
        {
            limit = fv(usedChess, goalMatrix);
            //Debug.Log("limit=" + limit);
            while (limit <= maxLevel && !dfs(0, 0))
            {
                limit++;
            }
            if (leafLevel != -1)
            {
                string s = "在" + rec.Count + "步内找到解！";
                findSolutionText.text = s;
                Debug.Log(s);
                foreach (int i in rec)
                {
                    Debug.Log(i);
                }
            }
            else
            {
                string s = "在" + maxLevel + "步中找不到解，放弃寻找！";
                findSolutionText.text = s;
                Debug.Log(s);
            }
        }
        else
        {
            findSolutionText.text = "无解";
            Debug.Log("Problem has no solution!");
        }
    }

    //动画演示
    public void moveAnimation()
    {
        
        switch (rec[animationCount])
        {
            case 0:
                moveZeroWithAnimation(Vector3.up);
                break;
            case 1:
                moveZeroWithAnimation(Vector3.down);
                break;
            case 2:
                moveZeroWithAnimation(Vector3.left);
                break;
            case 3:
                moveZeroWithAnimation(Vector3.right);
                break;
        }
        animationCount++;
    }

    public void hasSolutionButtonClicked()
    {
        if (hasSolution()) hasSolutionText.text = "有解";
        else
        {
            hasSolutionText.text = "无解";
        }
    }

    public void destroyAll()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Chess");
        foreach(GameObject go in gos)
        {
            Destroy(go);
        }
    }
}
