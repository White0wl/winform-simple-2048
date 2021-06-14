using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        enum StateCell { CanSum, CantMove, CanMove };
        //--------------------------------------//
        private bool SetEmptyCell()
        {
            int[] indexes = FoundEmptyCell();
            int randIdx = -1;

            if (indexes.Length > 0)
                randIdx = indexes[rand.Next(0, indexes.Length)];

            if (randIdx != -1)
            {
                cells[randIdx].Text = "2";
            }
            else if (randIdx == -1)
                return false;

            return true;
        }
        private int[] FoundIndicesValuesCell()
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                if (cells[i].Text != " ")
                {
                    indexes.Add(i);
                }
            }
            return indexes.ToArray();
        }
        private int[] ReverseFoundIndicesValuesCell()
        {
            int[] indicesValueCells = FoundIndicesValuesCell();
            int[] tmp = indicesValueCells.ToArray();
            //Revers
            for (int i = tmp.Length - 1, j = 0; i >= 0; i--, j++)
            {
                indicesValueCells[j] = tmp[i];
            }
            return indicesValueCells;
        }

        private int[] FoundEmptyCell()
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                if (cells[i].Text == " ")
                {
                    indexes.Add(i);
                }
            }
            return indexes.ToArray();
        }

        private void InitializeArray()
        {
            cells.Add(label0);
            cells.Add(label1);
            cells.Add(label2);
            cells.Add(label3);
            cells.Add(label4);
            cells.Add(label5);
            cells.Add(label6);
            cells.Add(label7);
            cells.Add(label8);
            cells.Add(label9);
            cells.Add(label10);
            cells.Add(label11);
            cells.Add(label12);
            cells.Add(label13);
            cells.Add(label14);
            cells.Add(label15);
        }
        //--------------------------------------//
        List<Label> cells;
        Random rand;
        bool someChange;
        DateTime start;
        int maxValue;
        public Form1()
        {
            cells = new List<Label>(16);
            start = DateTime.Now;
            maxValue = 0;
            InitializeComponent();
            InitializeArray();
            someChange = false;
            rand = new Random();

            for (int i = 0; i < 2; i++)
                SetEmptyCell();


        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    MoveLeftCells();
                    break;
                case Keys.Up:
                    MoveUpCells();
                    break;
                case Keys.Right:
                    MoveRightCells();
                    break;
                case Keys.Down:
                    MoveDownCells();
                    break;
                default:
                    break;
            }

            if(someChange)
                SetEmptyCell();
            someChange = false;

            if (FoundEmptyCell().Length == 0)
            {
                MessageBox.Show("Не осталось ячеек за " + (Math.Round((DateTime.Now - start).TotalSeconds,1))+"сек. , максимальное значение - "+maxValue);
                Close();
            }

        }
        private void MoveCell(int idx, string vector = "down")
        {
            int idxNow = idx;
            int idxNext;
            int vectorIdx = 0;


            int minIdx = 0;
            int maxIdx = 15;
            
           

            switch (vector)
            {
                case "up":
                    vectorIdx = -4;
                    break;
                case "left":
                    vectorIdx = -1;
                    if (idx < 4)
                    {
                        minIdx = 0;
                        maxIdx = 3;
                    }
                    else if (idx >= 4 && idx < 8)
                    {
                        minIdx = 4;
                        maxIdx = 7;
                    }
                    else if (idx >= 8 && idx < 12)
                    {
                        minIdx = 8;
                        maxIdx = 11;
                    }
                    else if (idx >= 12 && idx < 16)
                    {
                        minIdx = 12;
                        maxIdx = 15;
                    }
                    break;
                case "right":
                    vectorIdx = 1;
                    if (idx < 4)
                    {
                        minIdx = 0;
                        maxIdx = 3;
                    }
                    else if (idx >= 4 && idx < 8)
                    {
                        minIdx = 4;
                        maxIdx = 7;
                    }
                    else if (idx >= 8 && idx < 12)
                    {
                        minIdx = 8;
                        maxIdx = 11;
                    }
                    else if (idx >= 12 && idx < 16)
                    {
                        minIdx = 12;
                        maxIdx = 15;
                    }
                    break;
                case "down":
                    vectorIdx = 4;
                    break;
                default:
                    vectorIdx = 0;
                    break;
            }

            idxNext = idxNow + vectorIdx;
            StateCell state;
            if (idxNext >= minIdx && idxNext <= maxIdx)
                state = GetStateCell(idxNow, idxNext);
            else
                state = StateCell.CantMove;

            while (state != StateCell.CantMove)
            {
                if (state == StateCell.CanSum)
                {
                    int sum = Int32.Parse(cells[idxNow].Text) + Int32.Parse(cells[idxNext].Text);
                    if (sum > maxValue)
                        maxValue = sum;
                    cells[idxNext].Text = "" + sum;
                    cells[idxNow].Text = " ";
                }
                else if (state == StateCell.CanMove)
                {
                    cells[idxNext].Text = cells[idxNow].Text;
                    cells[idxNow].Text = " ";
                }
                idxNow = idxNext;
                idxNext = idxNow + vectorIdx;

                if (idxNext >= minIdx && idxNext <= maxIdx)
                    state = GetStateCell(idxNow, idxNext);
                else
                    state = StateCell.CantMove;
                someChange = true;
            }
        }

        private StateCell GetStateCell(int idx, int idxNext)
        {
            try
            {
                if (cells[idx].Text == cells[idxNext].Text)
                    return StateCell.CanSum;
                else if (cells[idxNext].Text == " ")
                    return StateCell.CanMove;
                else
                    return StateCell.CantMove;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return StateCell.CantMove;
            }
        }

        private void MoveDownCells()
        {
            int[] indicesValueCells = ReverseFoundIndicesValuesCell();
            for (int i = 0; i < indicesValueCells.Length; i++)
            {
                MoveCell(indicesValueCells[i], "down");
            }
        }


        private void MoveRightCells()
        {
            int[] indicesValueCells = ReverseFoundIndicesValuesCell();
            for (int i = 0; i < indicesValueCells.Length; i++)
            {
                MoveCell(indicesValueCells[i], "right");
            }
        }

        private void MoveUpCells()
        {
            int[] indicesValueCells = FoundIndicesValuesCell();
            for (int i = 0; i < indicesValueCells.Length; i++)
            {
                MoveCell(indicesValueCells[i], "up");
            }
        }

        private void MoveLeftCells()
        {
            int[] indicesValueCells = FoundIndicesValuesCell();
            for (int i = 0; i < indicesValueCells.Length; i++)
            {
                MoveCell(indicesValueCells[i], "left");
            }
        }
    }
}
