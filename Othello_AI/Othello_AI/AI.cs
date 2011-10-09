using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace AIothello
{
    class AI
    {
        struct Board
        {
            public int[,] board;
            public List<string> nextSlot;
            public int parent;
            public int[] score;
        };

        public static string Search(int[,] oriBoard)
        {
            string ans = "";
            int index = 0,
                turn = 0,
                tempScore = 0,
                limit = 4,
                count = 0;
            bool timeout = false;
            List<List<Board>> layerList = new List<List<Board>>();
            List<Board> boardList = new List<Board>();
            Stopwatch stopWatch = new Stopwatch();
            TimeSpan ts;

            Board tempBoard = new Board();

            // initialize first board
            tempBoard.board = oriBoard;
            tempBoard.nextSlot = new List<string>();
            SearchNext(oriBoard, tempBoard.nextSlot, turn);
            tempBoard.parent = 0;
            tempBoard.score = new int[1];

            boardList.Add(tempBoard);
            layerList.Add(boardList);

            // if no move
            if (tempBoard.nextSlot.Count == 0)
                return "0 0";

            // if near to end game -> search only 1 max score move
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (oriBoard[i, j] == 8)
                        count++;
                }
            }
            if (count <= 41)
                limit = 1;
            // if last move
            if (count == 37)
            {
                return tempBoard.nextSlot[0];
            }

            // if next move has corner slot
            for (int i = 0; i < tempBoard.nextSlot.Count; i++)
            {
                if (tempBoard.nextSlot[i] == "1 1" || tempBoard.nextSlot[i] == "1 8" ||
                    tempBoard.nextSlot[i] == "8 1" || tempBoard.nextSlot[i] == "8 8")
                    return tempBoard.nextSlot[i];
            }

            #region generate tree
            stopWatch.Start();

            while (true)
            {
                // all board in layer
                boardList = new List<Board>();

                // for each board in previous layer
                for (int b = 0; b < layerList[index].Count; b++)
                {
                    // for each next move of this board
                    for (int s = 0; s < layerList[index][b].nextSlot.Count; s++)
                    {
                        tempBoard = new Board();
                        tempBoard.nextSlot = new List<string>();
                        tempBoard.score = new int[1];
                        tempBoard.parent = b;
                        int[,] tmp = new int[10, 10];

                        #region time check

                        ts = stopWatch.Elapsed;
                        if ((ts.Seconds >= 1 && ts.Milliseconds >= 500) || index == limit)
                        {
                            stopWatch.Stop();
                            timeout = true;
                            break;
                        }

                        #endregion

                        #region copy board

                        for (int i = 0; i < 10; i++)
                        {
                            for (int j = 0; j < 10; j++)
                            {
                                tmp[i, j] = layerList[index][b].board[i, j];
                            }
                        }

                        #endregion

                        // place new
                        string[] tempStr = layerList[index][b].nextSlot[s].Split();
                        int row = int.Parse(tempStr[0]),
                            col = int.Parse(tempStr[1]);
                        tmp[row, col] = turn;

                        // update after place
                        Update(tmp, row, col, turn);

                        #region print
                        /*
                        for (int i = 1; i < 9; i++)
                        {
                            for (int j = 1; j < 9; j++)
                            {
                                Console.Write(tmp[i,j] + " ");
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                        */
                        #endregion

                        // new slot position for black turn
                        SearchNext(tmp, tempBoard.nextSlot, (turn + 1) % 2);
                        // if next move has corner slot
                        for (int i = 0; i < tempBoard.nextSlot.Count; i++)
                        {
                            if (tempBoard.nextSlot[i] == "1 1" || tempBoard.nextSlot[i] == "1 8" ||
                                tempBoard.nextSlot[i] == "8 1" || tempBoard.nextSlot[i] == "8 8")
                            {
                                if (turn == 0)
                                    tempBoard.score[0] -= 5;
                            }
                        }

                        tempBoard.board = tmp;
                        boardList.Add(tempBoard);
                    }
                    if (timeout)
                        break;
                }
                if (timeout)
                    break;

                turn = (turn + 1) % 2;
                layerList.Add(boardList);
                index++;

                #region time check

                ts = stopWatch.Elapsed;
                if ((ts.Seconds >= 1 && ts.Milliseconds >= 500) || index == limit)
                {
                    stopWatch.Stop();
                    timeout = true;
                    break;
                }

                #endregion
            }
            #endregion

            #region calculate score in last layer
            // for each board in last layer
            for (int b = 0; b < layerList[index].Count; b++)
            {
                tempScore = 0;

                for (int i = 1; i < 9; i++)
                {
                    for (int j = 1; j < 9; j++)
                    {
                        if (layerList[index][b].board[i, j] == 0)
                        {
                            tempScore++;
                        }
                        else if (layerList[index][b].board[i, j] == 1)
                        {
                            tempScore--;
                        }
                    }
                }
                layerList[index][b].score[0] = tempScore;
            }
            #endregion

            #region calculate minimax
            int parentNum,
                minimax;
            // each layer from leave to root
            for (int l = index; l > 0; l--)
            {
                parentNum = 0;

                if (turn == 1) // find min, set max value
                {
                    minimax = int.MaxValue;
                }
                else // find max, set min value
                {
                    minimax = int.MinValue;
                }

                // each board in layer
                for (int b = 0; b < layerList[l].Count; b++)
                {
                    if (parentNum != layerList[l][b].parent) // parent has changed, reset minimax
                    {
                        layerList[l - 1][layerList[l][b].parent].score[0] += minimax;
                        parentNum++;
                        if (turn == 1) // find min, set max value
                        {
                            minimax = int.MaxValue;
                        }
                        else // find max, set min value
                        {
                            minimax = int.MinValue;
                        }
                    }

                    if (turn == 1) // find min
                    {
                        minimax = Math.Min(minimax, layerList[l][b].score[0]);
                    }
                    else // find max
                    {
                        minimax = Math.Max(minimax, layerList[l][b].score[0]);
                    }
                }

                layerList[l - 1][parentNum].score[0] += minimax;
            }
            #endregion

            #region set ans to string
            for (int b = 0; b < layerList[1].Count; b++)
            {
                if (layerList[1][b].score[0] == layerList[0][0].score[0])
                {
                    ans = layerList[0][0].nextSlot[b];
                }
            }
            #endregion

            return ans;
        }

        static void Update(int[,] board, int row, int col, int value)
        {
            int turn = board[row, col];

            //check 8 direction
            #region left
            if (board[row, col - 1] != value && board[row, col - 1] != 8)
            {
                for (int i = col - 2; i >= 1; i--)
                {
                    if (board[row, i] == 8)
                        break;
                    if (board[row, i] == value)
                    {
                        for (int j = i + 1; j <= col - 1; j++)
                        {
                            board[row, j] = value;
                        }
                        break;
                    }
                }
            }
            #endregion

            #region right
            if (board[row, col + 1] != value && board[row, col + 1] != 8)
            {
                for (int i = col + 2; i <= 8; i++)
                {
                    if (board[row, i] == 8)
                        break;
                    if (board[row, i] == value)
                    {
                        for (int j = i - 1; j >= col + 1; j--)
                        {
                            board[row, j] = value;
                        }
                        break;
                    }
                }
            }
            #endregion

            #region up
            if (board[row - 1, col] != value && board[row - 1, col] != 8)
            {
                for (int i = row - 2; i >= 1; i--)
                {
                    if (board[i, col] == 8)
                        break;
                    if (board[i, col] == value)
                    {
                        for (int j = i + 1; j <= row - 1; j++)
                        {
                            board[j, col] = value;
                        }
                        break;
                    }
                }
            }
            #endregion

            #region down
            if (board[row + 1, col] != value && board[row + 1, col] != 8)
            {
                for (int i = row + 2; i <= 8; i++)
                {
                    if (board[i, col] == 8)
                        break;
                    if (board[i, col] == value)
                    {
                        for (int j = i - 1; j >= row + 1; j--)
                        {
                            board[j, col] = value;
                        }
                        break;
                    }
                }
            }
            #endregion

            #region left up
            if (board[row - 1, col - 1] != value && board[row - 1, col - 1] != 8)
            {
                int j = col - 2;
                for (int i = row - 2; i >= 1; i--)
                {
                    if (board[i, j] == 8)
                        break;
                    if (board[i, j] == value)
                    {
                        int n = j + 1;
                        for (int m = i + 1; m <= row - 1; m++)
                        {
                            board[m, n] = value;
                        }
                        break;
                    }
                    j--;
                    if (j < 1)
                        break;
                }
            }
            #endregion

            #region right up
            if (board[row - 1, col + 1] != value && board[row - 1, col + 1] != 8)
            {
                int j = col + 2;
                for (int i = row - 2; i >= 1; i--)
                {
                    if (board[i, j] == 8)
                        break;
                    if (board[i, j] == value)
                    {
                        int n = j - 1;
                        for (int m = i + 1; m <= row - 1; m++)
                        {
                            board[m, n] = value;
                        }
                        break;
                    }
                    j++;
                    if (j > 8)
                        break;
                }
            }
            #endregion

            #region right down
            if (board[row + 1, col + 1] != value && board[row + 1, col + 1] != 8)
            {
                int j = col + 2;
                for (int i = row + 2; i <= 8; i++)
                {
                    if (board[i, j] == 8)
                        break;
                    if (board[i, j] == value)
                    {
                        int n = j - 1;
                        for (int m = i - 1; m >= row + 1; m--)
                        {
                            board[m, n] = value;
                        }
                        break;
                    }
                    j++;
                    if (j > 8)
                        break;
                }
            }
            #endregion

            #region left bottom
            if (board[row + 1, col - 1] != value && board[row + 1, col - 1] != 8)
            {
                int j = col - 2;
                for (int i = row + 2; i <= 8; i++)
                {
                    if (board[i, j] == 8)
                        break;
                    if (board[i, j] == value)
                    {
                        int n = j + 1;
                        for (int m = i - 1; m >= row + 1; m--)
                        {
                            board[m, n] = value;
                        }
                        break;
                    }
                    j--;
                    if (j < 1)
                        break;
                }
            }
            #endregion
        }

        static void SearchNext(int[,] board, List<string> next, int value)
        {
            for (int a = 1; a <= 8; a++)
            {
                for (int b = 1; b <= 8; b++)
                {
                    if (board[a, b] == 8)
                    {
                        #region left
                        if (board[a, b - 1] != value && board[a, b - 1] != 8)
                        {
                            for (int i = b - 2; i >= 1; i--)
                            {
                                if (board[a, i] == 8)
                                    break;
                                if (board[a, i] == value)
                                {
                                    CheckCollision(next, a.ToString(), b.ToString());
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region right
                        if (board[a, b + 1] != value && board[a, b + 1] != 8)
                        {
                            for (int i = b + 2; i <= 8; i++)
                            {
                                if (board[a, i] == 8)
                                    break;
                                if (board[a, i] == value)
                                {
                                    CheckCollision(next, a.ToString(), b.ToString());
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region up
                        if (board[a - 1, b] != value && board[a - 1, b] != 8)
                        {
                            for (int i = a - 2; i >= 1; i--)
                            {
                                if (board[i, b] == 8)
                                    break;
                                if (board[i, b] == value)
                                {
                                    CheckCollision(next, a.ToString(), b.ToString());
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region down
                        if (board[a + 1, b] != value && board[a + 1, b] != 8)
                        {
                            for (int i = a + 2; i <= 8; i++)
                            {
                                if (board[i, b] == 8)
                                    break;
                                if (board[i, b] == value)
                                {
                                    CheckCollision(next, a.ToString(), b.ToString());
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region left top
                        if (board[a - 1, b - 1] != value && board[a - 1, b - 1] != 8)
                        {
                            int j = b - 2;
                            for (int i = a - 2; i >= 1; i--)
                            {
                                if (board[i, j] == 8)
                                    break;
                                if (board[i, j] == value)
                                {
                                    CheckCollision(next, a.ToString(), b.ToString());
                                    break;
                                }
                                j--;
                                if (j < 1)
                                    break;
                            }
                        }
                        #endregion

                        #region right top
                        if (board[a - 1, b + 1] != value && board[a - 1, b + 1] != 8)
                        {
                            int j = b + 2;
                            for (int i = a - 2; i >= 1; i--)
                            {
                                if (board[i, j] == 8)
                                    break;
                                if (board[i, j] == value)
                                {
                                    CheckCollision(next, a.ToString(), b.ToString());
                                    break;
                                }
                                j++;
                                if (j > 8)
                                    break;
                            }
                        }
                        #endregion

                        #region right bottom
                        if (board[a + 1, b + 1] != value && board[a + 1, b + 1] != 8)
                        {
                            int j = b + 2;
                            for (int i = a + 2; i <= 8; i++)
                            {
                                if (board[i, j] == 8)
                                    break;
                                if (board[i, j] == value)
                                {
                                    CheckCollision(next, a.ToString(), b.ToString());
                                    break;
                                }
                                j++;
                                if (j > 8)
                                    break;
                            }
                        }
                        #endregion

                        #region left up
                        if (board[a + 1, b - 1] != value && board[a + 1, b - 1] != 8)
                        {
                            int j = b - 2;
                            for (int i = a + 2; i <= 8; i++)
                            {
                                if (board[i, j] == 8)
                                    break;
                                if (board[i, j] == value)
                                {
                                    CheckCollision(next, a.ToString(), b.ToString());
                                    break;
                                }
                                j--;
                                if (j < 1)
                                    break;
                            }
                        }
                        #endregion
                    }
                }
            }
        }

        static void CheckCollision(List<string> next, string a, string b)
        {
            for (int i = 0; i < next.Count; i++)
            {
                string[] temp = next[i].Split();
                if (temp[0] == a && temp[1] == b)
                {
                    return;
                }
            }

            next.Add(a + " " + b);
        }
    }
}
