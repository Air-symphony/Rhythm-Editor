using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using DxLibDLL;
using CinderellaEditer;

namespace CinderellaEditor
{
    class DXWindow
    {
        int Width = 0;
        int Height = 0;
        MusicGraphics graph;
        public DXWindow(Form parent)
        {
            DX.SetUserWindow(parent.Handle); //DxLibの親ウインドウをこのフォームウインドウにセット
            DX.DxLib_Init();

            // ウィンドウモードに切り替え
            DX.ChangeWindowMode(DX.TRUE);

            // ウィンドウのサイズ指定
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);

            // DXライブラリの初期化
            if (DX.DxLib_Init() == -1)
            {
                // 初期化に失敗した場合は終了
                return;
            }
            Width = parent.Width / 2;
            Height = parent.Height;
            graph = new MusicGraphics(Width, Height);
            DrawText(0, 100, "設定完了");
        }

        public void DrawText(int x, int y, String text, bool Clear = false)
        {
            if (Clear) DX.ClearDrawScreen();
            DX.DrawString(x, y, text, DX.GetColor(255, 255, 255));
            if (Clear) DX.ScreenFlip();
        }

        public void DrawMainView()
        {
            DrawWindowLine();
            graph.DrawNoteLines();
            graph.DrawRhythmLine();
        }

        /*index,index - 1,index + 1*/
        public void DrawNote(string[] send)
        {
            DX.ClearDrawScreen();
            DrawMainView();
            graph.ReSetMainNote();

            /*各行について*/
            int main_bar_number = -1;
            int mainChannel;
            for (int line = 0; line < send.Length; line++)
            {
                string text = send[line];
                string channel_str = text.Split(',')[0];
                int channel = -1;
                bool writeNote = false;
                for (int i = 0; i < 4; i++)
                {
                    if (channel_str.Equals("#" + i))
                    {
                        channel = int.Parse(channel_str[1].ToString());
                        if (line == 0)
                        {
                            mainChannel = channel;
                        }
                        writeNote = true;
                        break;
                    }
                }
                if (writeNote == false)
                {
                    continue;
                }

                string[] NoteData = (text.Split(',')[1]).Split(':');
                /*何小節目か取得*/
                string bar_number_str = NoteData[0];
                if (bar_number_str.Length != 3) continue;

                int bar_number = 0;
                for (int i = 0; i < bar_number_str.Length; i++)
                {
                    int c = int.Parse(bar_number_str[i].ToString());
                    for (int k = i + 1; k < bar_number_str.Length; k++)
                    {
                        c *= 10;
                    }
                    bar_number += c;
                }
                /*選択中の行であれば*/
                if (line == 0)
                {
                    main_bar_number = bar_number;
                }
                /*その他上下の行で、小節番号が異なる場合*/
                else if (main_bar_number != bar_number)
                {
                    continue;
                }
                if (NoteData.Length <= 1) continue;
                /*各小節内のノーツの特徴*/
                string noteRythem = NoteData[1];
                int Rythem = noteRythem.Length;

                string noteFirstPos = "";
                string noteEndPos = "";
                if (NoteData.Length >= 3)
                {
                    noteFirstPos = noteEndPos = NoteData[2];
                    if (NoteData.Length >= 4)
                    {
                        noteEndPos = NoteData[3];
                    }
                }

                for (int i = noteFirstPos.Length; i < Rythem; i++)
                {
                    noteFirstPos += "3";
                }
                for (int i = noteEndPos.Length; i < Rythem; i++)
                {
                    noteEndPos += "3";
                }

                Note[] note = new Note[Rythem];
                DrawText(10, 40, bar_number + "小節目");
                if (line != 0)
                {
                    DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 127);
                }
                int count = 0;
                for (int i = 0; i < Rythem; i++)
                {
                    note[i] = new Note();
                    int type = int.Parse(noteRythem[i].ToString());
                    if (0 < type && type < 5)
                    {
                        int first = int.Parse(noteFirstPos[count].ToString());
                        int end = int.Parse(noteEndPos[count].ToString());
                        count++;
                        note[i] = new Note(channel, bar_number, type, Rythem, i, first, end);
                        //graph.DrawNote(end, Rythem, (int)Rythem - i, type);
                        //graph.DrawNote(note[i]);
                    }
                }
                if (line == 0)
                {
                    graph.SetMainNote(note);
                }
                graph.DrawNote(note);
                
            }
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 255);
            DX.ScreenFlip();
        }

        public void DrawWindowLine()
        {
            uint color = DX.GetColor(0, 0, 255);
            DX.DrawLine(0, 0, Width - 1, 0, color);
            DX.DrawLine(Width - 1, 0, Width - 1, Height - 1, color);
            DX.DrawLine(Width - 1, Height - 1, 0, Height - 1, color);
            DX.DrawLine(0, Height - 1, 0, 0, color);
        }

        public void DrawView()
        {
            DX.ClearDrawScreen();
            DrawMainView();
            DX.ScreenFlip();
        }
    }
}
