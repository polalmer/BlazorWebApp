using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;

namespace BlazorWebApp.Client.Pages;

public partial class Minesweeper
{
    private (int x, int y) _cursor;
    private bool _playing = false;
    private int _mineAmount = 10;
    private int _flagCount;
    private int _bombsflagged = 0;
    private bool _won = false;
    private int _gridsize = 8;
    private const int FieldSize = 40;
    private int _mapSize;
#pragma warning disable CS8618 
    private MinesweeperField[,] _fields;
    private Stopwatch _stopWatch;
#pragma warning restore CS8618 
    private string _elapsedTime = "";

    private void StartNewGame()
    {
        _fields = new MinesweeperField[_gridsize, _gridsize];
        if (_mineAmount >= _fields.Length)
        {
            _mineAmount = 1;
        }
        else
        {
            _mapSize = FieldSize * _gridsize;
            for (int y = 0; y < _fields.GetLength(1); y++)
            {
                for (int x = 0; x < _fields.GetLength(0); x++)
                {
                    _fields[x, y] = new();
                }
            }
            _bombsflagged = 0;
            _flagCount = _mineAmount;
            _playing = true;
            _won = false;
            PlaceMines();
        }
        _elapsedTime = "";
        _stopWatch = new();
        _stopWatch.Start();
    }
    private void PlaceMines()
    {
        int minesplaced = 0;
        int minesLeft = _mineAmount;
        while (minesplaced < minesLeft)
        {
            Random rnd = new();
            int x = rnd.Next(_fields.GetLength(0));
            int y = rnd.Next(_fields.GetLength(1));
            if (_fields[x, y].Value < 9) //the random field is not a bomb
            {
                _fields[x, y].Value = 9; //hiddenfield[x,y] >= 9 --> this field is a mine
                minesplaced++;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        //is x+i and y+j part of the field? if yes increment, else do nothing
                        if (x + i >= 0 && x + i < _fields.GetLength(0) && y + j >= 0 && y + j < _fields.GetLength(1))
                        {
                            _fields[x + i, y + j].Value++;
                        }
                    }
                }
            }//else the selected field was a mine, the minesplaced counter does not go up and the mine gets placed elsewhere
        }
    }
    private void SetCursorXY(MouseEventArgs e)
    {
        _cursor.x = Convert.ToInt32(e.OffsetX);
        _cursor.y = Convert.ToInt32(e.OffsetY);
        _cursor.x /= FieldSize;
        _cursor.y /= FieldSize;
    }
    private void UncoverField(MouseEventArgs e)
    {
        SetCursorXY(e);
        if (_fields[_cursor.x, _cursor.y].Value == 0)
        {
            //uncover all open fields next to the current field
            FieldIsEmpty(_cursor.x, _cursor.y);
        }
        else
        {
            //if the field is a number, only uncover this field
            if (_fields[_cursor.x, _cursor.y].Value < 9)
            {
                _fields[_cursor.x, _cursor.y].Visible = true;
            }
            else //uncover all fields
            {
                ShowAllFields();
            }
        }
    }
    private void FieldIsEmpty(int x, int y)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                // check if x+i or y+j outside of boundaries
                if (x + i >= 0 && x + i < _fields.GetLength(0) && y + j >= 0 && y + j < _fields.GetLength(1))
                {
                    if (_fields[i + x, j + y].Visible)
                    { //if the nextfield is already found, it can be skipped, because all its neighbours been already checked
                    }
                    else
                    {
                        if (_fields[i + x, j + y].Value == 0)
                        {   //if the next field is empty, its set as found and all the neighbours of the next field are getting checked
                            _fields[i + x, j + y].Visible = true;
                            FieldIsEmpty(x + i, y + j);
                        }
                        else
                        {   //if the nextfield is a number, it gets uncovered
                            _fields[x + i, y + j].Visible = true;
                        }
                    }
                }
            }
        }
    }
    private void FlagField(MouseEventArgs e)
    {
        SetCursorXY(e);
        if (_fields[_cursor.x, _cursor.y].Flagged)
        {
            _fields[_cursor.x, _cursor.y].Flagged = false;
            _flagCount++;
            if (_fields[_cursor.x, _cursor.y].Value >= 9)
            {
                _bombsflagged--;
            }
            else
            {
                _bombsflagged++;
            }
        }
        else
        {
            _fields[_cursor.x, _cursor.y].Flagged = true;
            _flagCount--;
            //de/increase the win counter, the game is won if all bombs are flagged with 
            if (_fields[_cursor.x, _cursor.y].Value >= 9)
            {
                _bombsflagged++;
            }
            else
            {
                _bombsflagged--;
            }
        }

        if (_bombsflagged == _mineAmount)
        {
            GameWon();
        }
    }
    private void GameWon()
    {
        _won = true;
        ShowAllFields();
    }
    private void ShowAllFields()
    {
        TimeSpan ts = _stopWatch.Elapsed;
        _elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        for (int y = 0; y < _fields.GetLength(1); y++)
        {
            for (int x = 0; x < _fields.GetLength(0); x++)
            {
                _fields[x, y].Visible = true;
                _fields[x, y].Flagged = false;
            }
        }
    }
}

public class MinesweeperField
{
    public bool Visible;
    public bool Flagged;
    public int Value;

    public MinesweeperField()
    {
        Value = 0;
        Visible = false;
        Flagged = false;
    }
}
