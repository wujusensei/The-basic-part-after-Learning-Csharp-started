using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace 小项目__飞行棋
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int weigth=50,heigth=30;
            ConsoleInit(weigth, heigth);
            E_SceneType sceneType = E_SceneType.Begin;
            bool isWin=false;
            
            while(true)
            {
                switch(sceneType)
                {
                    case E_SceneType.Begin:
                        // 开始场景逻辑
                        Console.Clear();
                        BeginScene(weigth, heigth,ref sceneType);
                        break;
                    case E_SceneType.Game:
                        // 游戏场景逻辑
                        Console.Clear();
                        GameScene(weigth, heigth, ref sceneType,ref isWin);
                        break;
                    case E_SceneType.End:
                        // 结束场景逻辑
                        Console.Clear();
                        EndScene(weigth, heigth, ref sceneType, ref isWin);
                        break;
                }
            }
        }

        #region 函数_控制台的初始化
        static void ConsoleInit(int weigth, int heigth)
        {
            //基础设置
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetWindowSize(weigth, heigth);
            Console.SetBufferSize(weigth, heigth);
        }
        #endregion

        #region 函数_开始场景逻辑
        static void BeginScene(int weigth,int heigth,ref E_SceneType sceneType)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(weigth / 2 - 3, 5);
            Console.Write("飞行棋");
            int nowSelection=0;
            bool isbreak=false;
            while (true)
            {
                Console.SetCursorPosition(weigth / 2 - 4, 10);
                Console.ForegroundColor = nowSelection == 0 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("我要起飞");
                Console.SetCursorPosition(weigth / 2 - 4, 12);
                Console.ForegroundColor = nowSelection == 1 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("不想起飞");
                
                switch(Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        nowSelection--;
                        if (nowSelection < 0) nowSelection = 1;
                        break;
                    case ConsoleKey.DownArrow:
                        nowSelection++;
                        if (nowSelection > 1) nowSelection = 0;
                        break;
                    case ConsoleKey.Z:
                        isbreak = true;
                        if (nowSelection == 0) sceneType = E_SceneType.Game;
                        else Environment.Exit(0);
                            break;
                }

                if (isbreak) break;
            }
        }
        #endregion

        #region 函数_游戏场景逻辑
        static void GameScene(int weigth,int heigth,ref E_SceneType sceneType,ref bool isWin)
        {
            DrawUnvisibleGameContent(weigth, heigth);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2,heigth-5);
            Console.Write("请按任意键开始投色子");

            Map map = new Map(15, 3, 80);
            map.Draw();

            ChessPiece player = new ChessPiece(E_ChessPieceType.Player,0);
            ChessPiece computer = new ChessPiece(E_ChessPieceType.Computer, 0);
            DrawChessPiece(player, computer,map);
            bool isGameOver = false;

            while (true)
            {
                //按任意键继续
                Console.ReadKey(true);
                //扔一次色子
                isGameOver = MoveAndPrint(weigth, heigth, ref player, ref computer, map, ref isWin);
                //更新地图与玩家
                map.Draw();
                DrawChessPiece(player, computer, map);
                //判断游戏是否结束
                if(isGameOver)
                {
                    Console.ReadKey(true);
                    sceneType = E_SceneType.End;
                    break;
                }

                //按任意键继续
                Console.ReadKey(true);
                //扔一次色子
                isGameOver = MoveAndPrint(weigth, heigth, ref computer, ref player, map, ref isWin);
                //更新地图与玩家
                map.Draw();
                DrawChessPiece(player, computer, map);
                //判断游戏是否结束
                if (isGameOver)
                {
                    Console.ReadKey(true);
                    sceneType = E_SceneType.End;
                    break;
                }
            }
        }
        #endregion

        #region 函数_结束场景逻辑
        static void EndScene(int weigth, int heigth, ref E_SceneType sceneType,ref bool isWin)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(weigth / 2 - 4, 5);
            Console.Write(isWin?"完美航线":"遗憾坠机");
            int nowSelection = 0;
            bool isbreak = false;
            while (true)
            {
                Console.SetCursorPosition(weigth / 2 - 4, 10);
                Console.ForegroundColor = nowSelection == 0 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("再次起飞");
                Console.SetCursorPosition(weigth / 2 - 4, 12);
                Console.ForegroundColor = nowSelection == 1 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("起个屁飞");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        nowSelection--;
                        if (nowSelection < 0) nowSelection = 1;
                        break;
                    case ConsoleKey.DownArrow:
                        nowSelection++;
                        if (nowSelection > 1) nowSelection = 0;
                        break;
                    case ConsoleKey.Z:
                        isbreak = true;
                        if (nowSelection == 0) sceneType = E_SceneType.Game;
                        else Environment.Exit(0);
                        break;
                }

                if (isbreak) break;
            }
        }
        #endregion

        #region 函数_绘制游戏场景内不变的内容
        static void DrawUnvisibleGameContent(int weigth, int heigth)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < heigth; i++)
            {
                if (i == 0 || i == heigth - 11 || i == heigth - 6 || i == heigth - 1)
                    for (int j = 0; j < weigth; j += 2)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write("■");
                    }
                else
                    Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(weigth - 1, i);
                Console.Write("■");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2, heigth - 10);
            Console.Write("路:普通格子-无效果");
            Console.SetCursorPosition(2, heigth - 9);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("止:空中拦截-暂停一回合");
            Console.SetCursorPosition(2, heigth - 8);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("道:时空隧道-随机前进后退交换");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(weigth/2+5, heigth - 10);
            Console.Write("我:表示玩家");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(weigth/2+5, heigth - 9);
            Console.Write("敌:表示电脑");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(weigth / 2 + 5, heigth - 8);
            Console.Write("乱:表示玩家电脑重合");
        }
        #endregion

        #region 函数_绘制 玩家 电脑 的位置图标
        static void DrawChessPiece(ChessPiece player,ChessPiece computer,Map map)
        {
            if(player.positionIndex==computer.positionIndex)
            {
                Console.SetCursorPosition(map.grids[player.positionIndex].position.x, map.grids[player.positionIndex].position.y);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("乱");
            }
            else
            {
                player.Draw(map);
                computer.Draw(map);
            }
        }
        #endregion

        #region 函数_扔色子加描述信息

        static bool MoveAndPrint(int weigth,int heigth,ref ChessPiece chess,ref ChessPiece chessAnother,Map map, ref bool isWin)
        {
            string nowWhoAction = chess.chessPieceType == E_ChessPieceType.Player ? "您" : "电脑";
            string nowAnotherAction = chess.chessPieceType == E_ChessPieceType.Computer ? "您" : "电脑";
            ClearInformation(heigth);

            Console.ForegroundColor = chess.chessPieceType == E_ChessPieceType.Player ? ConsoleColor.Cyan : ConsoleColor.Green;

            //扔色子前判断当前角色是否处于暂停状态
            if (chess.isStoped)
            {
                Console.SetCursorPosition(2,heigth - 5);
                Console.Write("处于暂停点，{0}需要暂停一回合再行动", nowWhoAction);
                chess.isStoped = false;
                return false;
            }

            //扔色子 随机骰出一个1~6的数 成为当前行动的角色前进步数
            Random random = new Random();
            int randomNumber = random.Next(1, 7);
            chess.positionIndex += randomNumber;

            Console.SetCursorPosition(2, heigth - 5);
            Console.Write("{0}扔出了{1}点，前进了{2}步", nowWhoAction, randomNumber, randomNumber);

            //如果扔完色子到了终点
            if (chess.positionIndex >= map.grids.Length-1)
            {
                //到了终点  准备退出
                chess.positionIndex =map.grids.Length-1;
                
                if(chess.chessPieceType==E_ChessPieceType.Player)
                {
                    Console.SetCursorPosition(2, heigth - 4);
                    Console.Write("恭喜恭喜，{0}到达了终点，赢得了胜利", nowWhoAction);
                    isWin = true;
                }
                else
                {
                    Console.SetCursorPosition(2, heigth - 4);
                    Console.Write("遗憾呐，{0}抢先一步到达了终点，您输了", nowWhoAction);
                    isWin = false;
                }
                Console.SetCursorPosition(2, heigth - 3);
                Console.Write("请按任意键结束");
                return true;
            }
            else
            {
                //如果扔完色子还没到终点  判断当前角色状态
                Grids grids = map.grids[chess.positionIndex];
                switch(grids.gridType)
                {
                    case E_GridType.Normal:
                        //普通格子 什么都不处理
                        Console.SetCursorPosition(2, heigth - 4);
                        Console.Write("{0}当前的位置一切安好", nowWhoAction);
                        Console.SetCursorPosition(2, heigth - 3);
                        Console.Write("接下来是{0}的回合，请按任意键继续", nowAnotherAction);
                        break;
                    case E_GridType.Stop:
                        //被拦截 暂停一回合
                        Console.SetCursorPosition(2, heigth - 4);
                        Console.Write("哇偶，{0}被拦截了！暂停行动一回合！", nowWhoAction);
                        Console.SetCursorPosition(2, heigth - 3);
                        Console.Write("接下来是{0}的回合，请按任意键继续", nowAnotherAction);
                        chess.isStoped = true;
                        break;
                    case E_GridType.Special:
                        //步入时空隧道 随机前进or后退or交换
                        randomNumber = random.Next(-5,9);
                        Console.SetCursorPosition(2, heigth - 4);
                        Console.Write("{0}步入了时空隧道", nowWhoAction);
                        if (randomNumber<=5)
                        {
                            chess.positionIndex += randomNumber;
                            if (chess.positionIndex < 0)
                                chess.positionIndex = 0;
                            if (chess.positionIndex >= map.grids.Length - 1)
                                chess.positionIndex = map.grids.Length - 1;
                            if(randomNumber<0)
                            {
                                Console.SetCursorPosition(2, heigth - 3);
                                Console.Write("时空紊乱了，{0}倒退了{1}格", nowWhoAction,-randomNumber);
                            }else if(randomNumber>0)
                            {
                                Console.SetCursorPosition(2, heigth - 3);
                                Console.Write("时空紊乱了，{0}前进了{1}格", nowWhoAction,randomNumber);
                            }else
                            {
                                Console.SetCursorPosition(2, heigth - 3);
                                Console.Write("时空略微动荡，但无事发生");
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(2, heigth - 3);
                            Console.Write("时空剧烈波动，{0}与{1}的位置被交换了！", nowWhoAction,nowAnotherAction);
                            int temp = chess.positionIndex;
                            chess.positionIndex = chessAnother.positionIndex;
                            chessAnother.positionIndex = temp;
                        }
                        Console.SetCursorPosition(2, heigth - 2);
                        Console.Write("接下来是{0}的回合，请按任意键继续", nowAnotherAction);
                        break;
                }
            }

                return false; 
        }
        #endregion

        #region 函数_擦除信息栏内描述信息
        static void ClearInformation(int heigth)
        {
            //擦除提示栏内上次的描述内容
            Console.SetCursorPosition(3, heigth - 5);
            Console.Write("                                              ");
            Console.SetCursorPosition(3, heigth - 4);
            Console.Write("                                              ");
            Console.SetCursorPosition(3, heigth - 3);
            Console.Write("                                              ");
            Console.SetCursorPosition(3, heigth - 2);
            Console.Write("                                              ");
        }
        #endregion
    }
    #region 结构体_位置信息
    struct Positions
    {
        public int x;
        public int y;

        public Positions (int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    #endregion

    #region 结构体_游戏格子
    struct Grids
    {
        public E_GridType gridType;
        public Positions position;

        public Grids (E_GridType gridType,int x,int y)
        {
            this.gridType = gridType;
            position.x = x;
            position.y = y;
        }

        public void Draw ()
        {
            Console.SetCursorPosition(position.x, position.y);
            switch (gridType)
            {
                case E_GridType.Normal:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("路");
                    break;
                case E_GridType.Stop:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("止");
                    break;
                case E_GridType.Special:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("道");
                    break;
            }
        }
    }
    #endregion

        #region 结构体_地图 (格子数组)
    struct Map
    {
        public Grids[] grids;

        public Map(int x,int y,int num)
        {
            grids = new Grids[num];
            Random random = new Random();
            int randomNum;
            //x和y变化的次数，用变量来表示，方便后续进行变化
            int indexX = 0, indexY = 0, stepX = 2;

            for(int i=0;i<num;i++)
            {
                //初始化格子类型 地图随机是普通，炸弹，时空隧道中的一种  概率不同
                randomNum = random.Next(1, 101);
                if(randomNum<=85||i==0||i==num-1)
                {
                    grids[i].gridType = E_GridType.Normal;
                }else if(randomNum<=95)
                {
                    grids[i].gridType = E_GridType.Special;
                }else
                {
                    grids[i].gridType = E_GridType.Stop;
                }

                grids[i].position = new Positions(x, y);
                if(indexX==10)
                {
                    y++;
                    indexY++;
                    if (indexY == 2)
                    {
                        indexX = 0;
                        indexY = 0;
                        stepX = -stepX;
                    }
                }
                else
                {
                    indexX++;
                    x += stepX;
                }
            }

        }

        public void Draw()
        {
            for(int i=0;i< grids.Length; i++)
            {
                grids[i].Draw();
            }
        }
    }
    #endregion

    #region 结构体_棋子
    struct ChessPiece
    {
        public E_ChessPieceType chessPieceType;
        public int positionIndex;  // 一个索引  用来表示目前身处那个格子的所在地
        public bool isStoped;

        public ChessPiece(E_ChessPieceType chessPieceType, int positionIndex)
        {
            this.chessPieceType = chessPieceType;
            this.positionIndex = positionIndex;
            this.isStoped = false;
        }

        public void Draw(Map map)
        {
            Grids grids = map.grids[positionIndex];
            Console.SetCursorPosition(grids.position.x, grids.position.y);
            switch(chessPieceType)
            {
                case E_ChessPieceType.Player:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("我");
                    break;
                case E_ChessPieceType.Computer:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("敌");
                    break;
            }
        }
    }
    #endregion

    #region 枚举_棋子类型
    enum E_ChessPieceType
    {
        Player,
        Computer,
    }
    #endregion

    #region 枚举_游戏场景
    enum E_SceneType
    {
        Begin,
        Game,
        End,
    }
    #endregion

    #region 枚举_游戏格子
    enum E_GridType
    {
        Normal,
        Stop,
        Special,
    }

    #endregion

}
