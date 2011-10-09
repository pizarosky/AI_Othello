using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Media;

namespace Othello_AI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand NavigateCommand = Thriple.Controls.ContentControl3D.RotateCommand;
        Dictionary<string, Button> button = new Dictionary<string, Button>();
        Dictionary<string, Grid> gridPaint = new Dictionary<string, Grid>();
        List<Grid> gridTemp = new List<Grid>();     //for Paint and clear Paint
        List<Grid> gridFlag = new List<Grid>();     //for set flag (small red rectangle)
        int[,] data = new int[10, 10];              //data 
        int value = 1;                              // 1 is black, 0 is white
        int scoreBlack = 2;
        int scoreWhite = 2;

        delegate void del();
        delegate void delUpdateScore(int a, int b, int c, int d);
        delegate bool delUpdate(int a, int b);
        delegate void delGen(int a, int b);
        delegate void delPaint();
        int count = 0;
        SoundPlayer player;
        public MainWindow()
        {
            InitializeComponent();
            player = new SoundPlayer("gun_shotgun1.wav");
            //ButtonTest.Template = (ControlTemplate)this.FindResource("ButtonWhite");
            
            //ButtonTest.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            
            //this.ButtonTest.Command.Execute(null);
            //NavigateCommand.Execute(null, ButtonTest);
            //NavigateCommand.Execute(null, data[1]);
            gridPaint["11"] = table11;
            gridPaint["12"] = table12;
            gridPaint["13"] = table13;
            gridPaint["14"] = table14;
            gridPaint["15"] = table15;
            gridPaint["16"] = table16;
            gridPaint["17"] = table17;
            gridPaint["18"] = table18;
            gridPaint["21"] = table21;
            gridPaint["22"] = table22;
            gridPaint["23"] = table23;
            gridPaint["24"] = table24;
            gridPaint["25"] = table25;
            gridPaint["26"] = table26;
            gridPaint["27"] = table27;
            gridPaint["28"] = table28;
            gridPaint["31"] = table31;
            gridPaint["32"] = table32;
            gridPaint["33"] = table33;
            gridPaint["34"] = table34;
            gridPaint["35"] = table35;
            gridPaint["36"] = table36;
            gridPaint["37"] = table37;
            gridPaint["38"] = table38;
            gridPaint["41"] = table41;
            gridPaint["42"] = table42;
            gridPaint["43"] = table43;
            gridPaint["44"] = table44;
            gridPaint["45"] = table45;
            gridPaint["46"] = table46;
            gridPaint["47"] = table47;
            gridPaint["48"] = table48;
            gridPaint["51"] = table51;
            gridPaint["52"] = table52;
            gridPaint["53"] = table53;
            gridPaint["54"] = table54;
            gridPaint["55"] = table55;
            gridPaint["56"] = table56;
            gridPaint["57"] = table57;
            gridPaint["58"] = table58;
            gridPaint["61"] = table61;
            gridPaint["62"] = table62;
            gridPaint["63"] = table63;
            gridPaint["64"] = table64;
            gridPaint["65"] = table65;
            gridPaint["66"] = table66;
            gridPaint["67"] = table67;
            gridPaint["68"] = table68;
            gridPaint["71"] = table71;
            gridPaint["72"] = table72;
            gridPaint["73"] = table73;
            gridPaint["74"] = table74;
            gridPaint["75"] = table75;
            gridPaint["76"] = table76;
            gridPaint["77"] = table77;
            gridPaint["78"] = table78;
            gridPaint["81"] = table81;
            gridPaint["82"] = table82;
            gridPaint["83"] = table83;
            gridPaint["84"] = table84;
            gridPaint["85"] = table85;
            gridPaint["86"] = table86;
            gridPaint["87"] = table87;
            gridPaint["88"] = table88;           
            
            for (int i = 0; i <= 9; i++)
                for (int j = 0; j <= 9; j++)
                    data[i, j] = 8;
            
            table45.Children.Add(GenerateButton("45", 4, 5));
            table45.IsEnabled = false;
            table44.Children.Add(GenerateButton("44", 4, 4));
            table44.IsEnabled = false;
            table54.Children.Add(GenerateButton("54", 5, 4));
            table54.IsEnabled = false;
            table55.Children.Add(GenerateButton("55", 5, 5));
            table55.IsEnabled = false;
            
            //UpdateScore();
            scoreBlack_text.Text = scoreBlack.ToString();
            scoreWhite_text.Text = scoreWhite.ToString();
            progressBlack.Value = scoreBlack;
            progressWhite.Value = scoreWhite;
            Paint();
            debug();
            BlackTurn();
        }

        #region Generate Button
        private Thriple.Controls.ContentControl3D GenerateButton(string hole, int row, int col)
        {            
            Thriple.Controls.ContentControl3D temp = new Thriple.Controls.ContentControl3D();
            temp.Cursor = Cursors.Hand;
            temp.Width = 80;
            temp.Height = 80;
            temp.EasingMode = Thriple.Controls.RotationEasingMode.RoundhouseKick;
            Button front = new Button();
            front.Width = 80; front.Height = 80;
            front.Command = Thriple.Controls.ContentControl3D.RotateCommand;
            if (value==0)
            {
                front.Template = (ControlTemplate)this.FindResource("ButtonWhite");
            }
            else
            {
                front.Template = (ControlTemplate)this.FindResource("ButtonBlack");
            }
            temp.Content = front;
            button[hole] = front;

            Button back = new Button();
            back.Width = 80; back.Height = 80;
            back.Command = Thriple.Controls.ContentControl3D.RotateCommand;
            if (value==0)
            {
                back.Template = (ControlTemplate)this.FindResource("ButtonBlack");
            }
            else
            {
                back.Template = (ControlTemplate)this.FindResource("ButtonWhite");
            }          
            temp.BackContent = back;

            data[row, col] = value;
            value += 1;
            if (value == 2)
                value = 0;
            return temp;
        }
        #endregion

        void debug()
        {
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    Debug.Write(data[i, j] + " ");
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine("value = " + value);
            //Debug.WriteLine("Return = " + AIothello.AI.Search(data));
            Debug.WriteLine("-------------------------------");
        }

        bool Update(int row, int col)
        {
            bool res = false;
            if (data[row, col - 1] != value && data[row, col - 1] != 8)
            {
                for (int i = col - 2; i >= 1; i--)
                {
                    if (data[row, i] == 8)
                        break;
                    if (data[row, i] == value)
                    {
                        for (int j = i + 1; j <= col - 1; j++)
                        {
                            data[row, j] = value;
                            if (value == 1)
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack + 1, scoreWhite - 1);
                            else
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack - 1, scoreWhite + 1);

                            NavigateCommand.Execute(null, button[row.ToString() + j.ToString()]);
                            res = true;
                            RemovePaint();
                        }
                        break;
                    }                 
                }
            }

            if (data[row, col + 1] != value && data[row, col + 1] != 8)
            {
                for (int i = col + 2; i <= 8; i++)
                {
                    if (data[row, i] == 8)
                        break;
                    if (data[row, i] == value)
                    {
                        for (int j = i - 1; j >= col + 1; j--)
                        {
                            data[row, j] = value;
                            if (value == 1)
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack + 1, scoreWhite - 1);
                            else
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack - 1, scoreWhite + 1);
                            NavigateCommand.Execute(null, button[row.ToString() + j.ToString()]);
                            res = true;
                            RemovePaint();
                        }
                        break;
                    }                   
                }
            }

            if (data[row - 1, col] != value && data[row - 1, col] != 8)
            {
                for (int i = row - 2; i >= 1; i--)
                {
                    if (data[i, col] == 8)
                        break;
                    if (data[i, col] == value)
                    {
                        for (int j = i + 1; j <= row - 1; j++)
                        {
                            data[j, col] = value;
                            if (value == 1)
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack + 1, scoreWhite - 1);
                            else
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack - 1, scoreWhite + 1);
                            NavigateCommand.Execute(null, button[j.ToString() + col.ToString()]);
                            res = true;
                            RemovePaint();
                        }
                        break;
                    }                  
                }
            }

            if (data[row + 1, col] != value && data[row + 1, col] != 8)
            {
                for (int i = row + 2; i <= 8; i++)
                {
                    if (data[i, col] == 8)
                        break;
                    if (data[i, col] == value)
                    {
                        for (int j = i - 1; j >= row + 1; j--)
                        {
                            data[j, col] = value;
                            if (value == 1)
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack + 1, scoreWhite - 1);
                            else
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack - 1, scoreWhite + 1);
                            NavigateCommand.Execute(null, button[j.ToString() + col.ToString()]);
                            res = true;
                            RemovePaint();
                        }
                        break;
                    }                    
                }
            }
            
            //left top
            if (data[row - 1, col - 1] != value && data[row - 1, col - 1] != 8)
            {
                int j = col - 2;
                for (int i = row - 2; i >= 1; i--)
                {
                    if (data[i, j] == 8)
                        break;
                    if (data[i, j] == value)
                    {
                        int n = j + 1;
                        for (int m = i + 1; m <= row - 1; m++)
                        {
                            data[m, n] = value;
                            if (value == 1)
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack + 1, scoreWhite - 1);
                            else
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack - 1, scoreWhite + 1);
                            NavigateCommand.Execute(null, button[m.ToString() + n.ToString()]);
                            res = true;
                            n++;
                            RemovePaint();
                        }
                        break;
                    }
                    j--;
                    if (j < 1)
                        break;
                }
            }

            // right top
            if (data[row - 1, col + 1] != value && data[row - 1, col + 1] != 8)
            {
                int j = col + 2;
                for (int i = row - 2; i >= 1; i--)
                {
                    if (data[i, j] == 8)
                        break;
                    if (data[i, j] == value)
                    {
                        int n = j - 1;
                        for (int m = i + 1; m <= row - 1; m++)
                        {
                            data[m, n] = value;
                            if (value == 1)
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack + 1, scoreWhite - 1);
                            else
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack - 1, scoreWhite + 1);
                            NavigateCommand.Execute(null, button[m.ToString() + n.ToString()]);
                            res = true;
                            n--;
                            RemovePaint();
                        }
                        break;
                    }
                    j++;
                    if (j > 8)
                        break;
                }
            }

            //right bottom
            if (data[row + 1, col + 1] != value && data[row + 1, col + 1] != 8)
            {
                int j = col + 2;
                for (int i = row + 2; i <= 8; i++)
                {
                    if (data[i, j] == 8)
                        break;
                    if (data[i, j] == value)
                    {
                        int n = j - 1;
                        for (int m = i - 1; m >= row + 1; m--)
                        {
                            data[m, n] = value;
                            if (value == 1)
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack + 1, scoreWhite - 1);
                            else
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack - 1, scoreWhite + 1);
                            NavigateCommand.Execute(null, button[m.ToString() + n.ToString()]);
                            res = true;
                            n--;
                            RemovePaint();
                        }
                        break;
                    }                  
                    j++;
                    if (j > 8)
                        break;
                }
            }

            //left bottom
            if (data[row + 1, col - 1] != value && data[row + 1, col - 1] != 8)
            {
                int j = col - 2;
                for (int i = row + 2; i <= 8; i++)
                {
                    if (data[i, j] == 8)
                        break;
                    if (data[i, j] == value)
                    {
                        int n = j + 1;
                        for (int m = i - 1; m >= row + 1; m--)
                        {
                            data[m, n] = value;
                            if (value == 1)
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack + 1, scoreWhite - 1);
                            else
                                UpdateScore(scoreBlack, scoreWhite, scoreBlack - 1, scoreWhite + 1);
                            NavigateCommand.Execute(null, button[m.ToString() + n.ToString()]);
                            res = true;
                            n++;
                            RemovePaint();
                        }
                        break;
                    }
                    j--;
                    if (j < 1)
                        break;
                }
            }
            return res;
        }

        void UpdateScore(int temp_scoreBlack, int temp_scoreWhite, int new_scoreBlack, int new_scoreWhite)
        {
            /*int temp_scoreBlack = 0;
            int temp_scoreWhite = 0;
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (data[i, j] == 1)
                        temp_scoreBlack += 1;
                    if (data[i, j] == 0)
                        temp_scoreWhite += 1;
                }
            }*/

            scoreBlack_text.Text = new_scoreBlack.ToString();
            Delay(temp_scoreBlack, new_scoreBlack, progressBlack);
            scoreWhite_text.Text = new_scoreWhite.ToString();
            Delay(temp_scoreWhite, new_scoreWhite, progressWhite);
            scoreBlack = new_scoreBlack;
            scoreWhite = new_scoreWhite;

        }

        void RemovePaint()
        {
            for (int i = 0; i < gridTemp.Count; i++)
            {
                gridTemp[i].Children.RemoveAt(1);
            }
            gridTemp.Clear();
        }

        void Paint()
        {
            for (int a = 1; a <= 8; a++)
            {
                for (int b = 1; b <= 8; b++)
                {
                    if (data[a, b] == 8)
                    {
                        if (data[a, b - 1] != value && data[a, b - 1] != 8)
                        {
                            for (int i = b - 2; i >= 1; i--)
                            {
                                if (data[a, i] == 8)
                                    break;
                                if (data[a, i] == value && !gridTemp.Contains(gridPaint[a.ToString() + b.ToString()]))
                                {
                                    Rectangle rec = new Rectangle();
                                    if(value==1)
                                        rec.Style = (Style)this.FindResource("RectBlack");
                                    else
                                        rec.Style = (Style)this.FindResource("RectLight");
                                    gridPaint[a.ToString()+b.ToString()].Children.Add(rec);
                                    gridTemp.Add(gridPaint[a.ToString() + b.ToString()]);
                                    count += 1;
                                    break;
                                }
                            }
                        }

                        if (data[a, b + 1] != value && data[a, b + 1] != 8)
                        {
                            for (int i = b + 2; i <= 8; i++)
                            {
                                if (data[a, i] == 8)
                                    break;
                                if (data[a, i] == value && !gridTemp.Contains(gridPaint[a.ToString() + b.ToString()]))
                                {
                                    Rectangle rec = new Rectangle();
                                    if (value == 1)
                                        rec.Style = (Style)this.FindResource("RectBlack");
                                    else
                                        rec.Style = (Style)this.FindResource("RectLight");
                                    gridPaint[a.ToString() + b.ToString()].Children.Add(rec);
                                    gridTemp.Add(gridPaint[a.ToString() + b.ToString()]);
                                    count += 1;
                                    break;
                                }
                            }
                        }

                        if (data[a - 1, b] != value && data[a - 1, b] != 8)
                        {
                            for (int i = a - 2; i >= 1; i--)
                            {
                                if (data[i, b] == 8)
                                    break;
                                if (data[i, b] == value && !gridTemp.Contains(gridPaint[a.ToString() + b.ToString()]))
                                {
                                    Rectangle rec = new Rectangle();
                                    if (value == 1)
                                        rec.Style = (Style)this.FindResource("RectBlack");
                                    else
                                        rec.Style = (Style)this.FindResource("RectLight");
                                    gridPaint[a.ToString() + b.ToString()].Children.Add(rec);
                                    gridTemp.Add(gridPaint[a.ToString() + b.ToString()]);
                                    count += 1;
                                    break;
                                }
                            }
                        }

                        if (data[a + 1, b] != value && data[a + 1, b] != 8)
                        {
                            for (int i = a + 2; i <= 8; i++)
                            {
                                if (data[i, b] == 8)
                                    break;
                                if (data[i, b] == value && !gridTemp.Contains(gridPaint[a.ToString() + b.ToString()]))
                                {
                                    Rectangle rec = new Rectangle();
                                    if (value == 1)
                                        rec.Style = (Style)this.FindResource("RectBlack");
                                    else
                                        rec.Style = (Style)this.FindResource("RectLight");
                                    gridPaint[a.ToString() + b.ToString()].Children.Add(rec);
                                    gridTemp.Add(gridPaint[a.ToString() + b.ToString()]);
                                    count += 1;
                                    break;
                                }
                            }
                        }

                        //left top
                        if (data[a - 1, b - 1] != value && data[a - 1, b - 1] != 8)
                        {
                            int j = b - 2;
                            for (int i = a - 2; i >= 1; i--)
                            {
                                if (data[i, j] == 8)
                                    break;
                                if (data[i, j] == value && !gridTemp.Contains(gridPaint[a.ToString() + b.ToString()]))
                                {
                                    Rectangle rec = new Rectangle();
                                    if (value == 1)
                                        rec.Style = (Style)this.FindResource("RectBlack");
                                    else
                                        rec.Style = (Style)this.FindResource("RectLight");
                                    gridPaint[a.ToString() + b.ToString()].Children.Add(rec);
                                    gridTemp.Add(gridPaint[a.ToString() + b.ToString()]);
                                    count += 1;
                                    break;
                                }
                                j--;
                                if (j < 1)
                                    break;
                            }
                        }

                        // right top
                        if (data[a - 1, b + 1] != value && data[a - 1, b + 1] != 8)
                        {
                            int j = b + 2;
                            for (int i = a - 2; i >= 1; i--)
                            {
                                if (data[i, j] == 8)
                                    break;
                                if (data[i, j] == value && !gridTemp.Contains(gridPaint[a.ToString() + b.ToString()]))
                                {
                                    Rectangle rec = new Rectangle();
                                    if (value == 1)
                                        rec.Style = (Style)this.FindResource("RectBlack");
                                    else
                                        rec.Style = (Style)this.FindResource("RectLight");
                                    gridPaint[a.ToString() + b.ToString()].Children.Add(rec);
                                    gridTemp.Add(gridPaint[a.ToString() + b.ToString()]);
                                    count += 1;
                                    break;
                                }
                                j++;
                                if (j > 8)
                                    break;
                            }
                        }

                        //right bottom
                        if (data[a + 1, b + 1] != value && data[a + 1, b + 1] != 8)
                        {
                            int j = b + 2;
                            for (int i = a + 2; i <= 8; i++)
                            {
                                if (data[i, j] == 8)
                                    break;
                                if (data[i, j] == value && !gridTemp.Contains(gridPaint[a.ToString() + b.ToString()]))
                                {
                                    Rectangle rec = new Rectangle();
                                    if (value == 1)
                                        rec.Style = (Style)this.FindResource("RectBlack");
                                    else
                                        rec.Style = (Style)this.FindResource("RectLight");
                                    gridPaint[a.ToString() + b.ToString()].Children.Add(rec);
                                    gridTemp.Add(gridPaint[a.ToString() + b.ToString()]);
                                    count += 1;
                                    break;
                                }
                                j++;
                                if (j > 8)
                                    break;
                            }
                        }

                        //left bottom
                        if (data[a + 1, b - 1] != value && data[a + 1, b - 1] != 8)
                        {
                            int j = b - 2;
                            for (int i = a + 2; i <= 8; i++)
                            {
                                if (data[i, j] == 8)
                                    break;
                                if (data[i, j] == value && !gridTemp.Contains(gridPaint[a.ToString() + b.ToString()]))
                                {
                                    Rectangle rec = new Rectangle();
                                    if (value == 1)
                                        rec.Style = (Style)this.FindResource("RectBlack");
                                    else
                                        rec.Style = (Style)this.FindResource("RectLight");
                                    gridPaint[a.ToString() + b.ToString()].Children.Add(rec);
                                    gridTemp.Add(gridPaint[a.ToString() + b.ToString()]);
                                    count += 1;
                                    break;
                                }
                                j--;
                                if (j < 1)
                                    break;
                            }
                        }
                    }
                }
            }
        }

        void Delay(int before, int after, ProgressBar temp)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = before;
            myDoubleAnimation.To = after;
            myDoubleAnimation.Duration =  new Duration(TimeSpan.FromSeconds(0.5));

            // Apply the animation to the button's Width property.
            temp.BeginAnimation(ProgressBar.ValueProperty, myDoubleAnimation);

        }
        private void WhiteTurn()
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 100;
            myDoubleAnimation.To = 200;
            //myDoubleAnimation.EasingFunction = new SineEase();
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            BWhite.BeginAnimation(Thriple.Controls.ContentControl3D.WidthProperty, myDoubleAnimation);
            BWhite.BeginAnimation(Thriple.Controls.ContentControl3D.HeightProperty, myDoubleAnimation);

            DoubleAnimation myDoubleAnimation2 = new DoubleAnimation();
            myDoubleAnimation2.From = 200;
            myDoubleAnimation2.To = 100;
            //myDoubleAnimation2.EasingFunction = new SineEase();
            myDoubleAnimation2.Duration = new Duration(TimeSpan.FromSeconds(1));
            BBlack.BeginAnimation(Thriple.Controls.ContentControl3D.WidthProperty, myDoubleAnimation2);
            BBlack.BeginAnimation(Thriple.Controls.ContentControl3D.HeightProperty, myDoubleAnimation2);
        }

        private void BlackTurn()
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 100;
            myDoubleAnimation.To = 200;
            //myDoubleAnimation.EasingFunction = new SineEase();
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            BBlack.BeginAnimation(Thriple.Controls.ContentControl3D.WidthProperty, myDoubleAnimation);
            BBlack.BeginAnimation(Thriple.Controls.ContentControl3D.HeightProperty, myDoubleAnimation);

            DoubleAnimation myDoubleAnimation2 = new DoubleAnimation();
            myDoubleAnimation2.From = 200;
            myDoubleAnimation2.To = 100;
            //myDoubleAnimation2.EasingFunction = new SineEase();
            myDoubleAnimation2.Duration = new Duration(TimeSpan.FromSeconds(1));
            BWhite.BeginAnimation(Thriple.Controls.ContentControl3D.WidthProperty, myDoubleAnimation2);
            BWhite.BeginAnimation(Thriple.Controls.ContentControl3D.HeightProperty, myDoubleAnimation2);
        }

        private void table_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (count > 0)
            {
                int row = int.Parse(((Grid)sender).Name.Substring(5, 1));
                int col = int.Parse(((Grid)sender).Name.Substring(6, 1));
                bool res = Update(row, col);

                debug();
                if (res == true)
                {                 
                    if (value == 1)
                        UpdateScore(scoreBlack, scoreWhite, scoreBlack + 1, scoreWhite);
                    else
                        UpdateScore(scoreBlack, scoreWhite, scoreBlack, scoreWhite + 1);
                    if(gridFlag.Count>0)
                        gridFlag[gridFlag.Count - 1].Children.RemoveAt(gridFlag[gridFlag.Count - 1].Children.Count - 1);
                
                    ((Grid)sender).Children.Add(GenerateButton(row.ToString() + col.ToString(), row, col));
                    ((Grid)sender).IsEnabled = false;                 
                    player.Play();
                    Rectangle rec = new Rectangle();
                    rec.Height = 10;
                    rec.Width = 10;
                    rec.Fill = new SolidColorBrush(Colors.Red);
                    ((Grid)sender).Children.Add(rec);
                    gridFlag.Add((Grid)sender);
                    WhiteTurn();
                    count = 0;
                    Paint();
                    Debug.WriteLine("White Count = " + count);
                    if (count > 0)
                    {
                        grid_table.IsEnabled = false;
                        BWhite.Width = 200;
                        BWhite.Height = 200;
                        BackgroundWorker bg = new BackgroundWorker();
                        bg.DoWork += new DoWorkEventHandler(bg_DoWork);
                        bg.RunWorkerAsync();
                    }
                    else
                    {
                        value += 1;
                        if (value == 2)
                            value = 0;
                        Paint();
                        BlackTurn();
                        Debug.WriteLine("Black Count = " + count);
                        if(count==0)
                            MessageBox.Show("Finished Game!!");
                    }
                }
                
            }
            
            
        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            //throw new NotImplementedException();
            string temp = AIothello.AI.Search(data);
            int row = int.Parse(temp.Substring(0, 1));
            int col = int.Parse(temp.Substring(2, 1));
            Thread.Sleep(1000);
            Dispatcher.Invoke(new delUpdate(Update), new object[] { row, col });
            
            if (value == 1)
                Dispatcher.Invoke(new delUpdateScore(UpdateScore), new object[] { scoreBlack, scoreWhite, scoreBlack + 1, scoreWhite });
            else
                Dispatcher.Invoke(new delUpdateScore(UpdateScore), new object[] { scoreBlack, scoreWhite, scoreBlack, scoreWhite + 1 });
            
            Dispatcher.Invoke(new delGen(Gen), new object[] { row, col });
            count = 0;
            Dispatcher.Invoke(new delPaint(Paint));           
            debug();
            Debug.WriteLine("Black Count = " + count);
            if (count == 0)
            {
                value += 1;
                if (value == 2)
                    value = 0;
                Dispatcher.Invoke(new delPaint(Paint)); 
                if (count == 0)
                    MessageBox.Show("Finised Game!!");
                else
                    Dispatcher.Invoke(new delPaint(again));
            }
            
        }
        void again()
        {
            WhiteTurn();
            grid_table.IsEnabled = false;
            BWhite.Width = 200;
            BWhite.Height = 200;
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.RunWorkerAsync();

        }
        void Gen(int row, int col)
        {
            gridFlag[gridFlag.Count - 1].Children.RemoveAt(gridFlag[gridFlag.Count - 1].Children.Count - 1);
            gridPaint[row.ToString() + col.ToString()].Children.Add(GenerateButton(row.ToString() + col.ToString(), row, col));
            player.Play();
            Rectangle rec = new Rectangle();
            rec.Height = 10;
            rec.Width = 10;
            rec.Fill = new SolidColorBrush(Colors.Red);
            gridPaint[row.ToString() + col.ToString()].Children.Add(rec);
            gridFlag.Add(gridPaint[row.ToString() + col.ToString()]);
            gridPaint[row.ToString() + col.ToString()].IsEnabled = false;
            grid_table.IsEnabled = true;
            BlackTurn();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to start new game ?","New Game", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            
        }

    }
}
