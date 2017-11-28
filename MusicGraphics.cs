using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using CinderellaEditer;

namespace CinderellaEditor
{
    class MusicGraphics
    {
        Note[] mainNote;
        Note[] subNote;
        int count = 5, noteLine = 5;
        int[] graphHandle = new int[4];
        private int Width, Height;
        private int dis_x = 50, dis_y = 10;

        int index = 0;

        /*上下左右の余白*/
        private int x = 0, y = 20;

        public MusicGraphics(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
            dis_x = (int)(Width / 6.0);
            dis_y = Height / (count + 1);
            SetGraph();
        }

        public void SetGraph()
        {
            for (int i = 0; i < graphHandle.Length; i++)
            {
                graphHandle[i] = DX.LoadGraph("Material//Note" + (i + 1) + ".png");
            }
        }

        public void DrawNoteLines()
        {
            uint color = DX.GetColor(200, 200, 200);
            for (int i = 1; i <= noteLine; i++)
            {
                DX.DrawLine(x + dis_x * i, y, 
                            x + dis_x * i, Height - y, color);
            }
        }

        public void DrawRhythmLine()
        {
            uint color = DX.GetColor(100, 100, 100);
            for (int i = 1; i <= count; i++)
            {
                DX.DrawLine(x + dis_x * 1 - 20, dis_y * i, 
                            x + dis_x * count + 20, dis_y * i, color);
            }
        }

        public void DrawNoteGraph(int id, Note note) {
            /*同時押し線*/
            SynchronousLine(id, note);

            /*ロングノーツ実装*/
            LongLine(id, note);

            /*フリック連結線*/
            FlicLine(id, note);

            int center_x = x + dis_x * note.end;
            int center_y = dis_y + (int)(((double)(Height - dis_y * 2) / note.rythem) * (note.rythem - note.timing));
            DrawGraph(center_x, center_y, note.type);
        }

        public void SynchronousLine(int id, Note note)
        {
            int center_x = x + dis_x * note.end;
            int center_y = dis_y + (int)(((double)(Height - dis_y * 2) / note.rythem) * (note.rythem - note.timing));
            uint color = DX.GetColor(255, 255, 255);
            
            for (int i = 0; i < mainNote.Length; i++)
            {
                if (mainNote[i].channel == note.channel) break;
                
                if (Math.Abs(mainNote[i].GetTiming() - note.GetTiming()) < 0.01d)
                {
                    int main_center_x = x + dis_x * mainNote[i].end;
                    int main_center_y = dis_y +
                        (int)(((double)(Height - dis_y * 2) / mainNote[i].rythem)
                         * (mainNote[i].rythem - mainNote[i].timing));
                    DX.DrawFillBox(center_x, center_y - 4,
                            main_center_x, main_center_y + 4, color);
                    DrawGraph(main_center_x, main_center_y, mainNote[i].type);
                    break;
                }
            }
        }

        public void LongLine(int id, Note note)
        {
            int center_x = x + dis_x * note.end;
            int center_y = dis_y + (int)(((double)(Height - dis_y * 2) / note.rythem) * (note.rythem - note.timing));
            uint color = DX.GetColor(255, 255, 255);

            for (int i = 0; i < mainNote.Length; i++)
            {
                if (mainNote[i].channel != note.channel ||
                    mainNote[i].GetTiming() >= note.GetTiming()) break;

                if (mainNote[i].type == 4)
                {
                    if (mainNote[i].end == note.end)
                    {
                        int main_center_x = x + dis_x * mainNote[i].end;
                        int main_center_y = dis_y +
                            (int)(((double)(Height - dis_y * 2) / mainNote[i].rythem)
                             * (mainNote[i].rythem - mainNote[i].timing));
                        DX.DrawLineBox(center_x - 1, center_y,
                                main_center_x + 1, main_center_y, color);
                        break;
                    }
                }
            }
        }

        public void FlicLine(int id, Note note)
        {
            int center_x = x + dis_x * note.end;
            int center_y = dis_y + (int)(((double)(Height - dis_y * 2) / note.rythem) * (note.rythem - note.timing));
            uint color = DX.GetColor(255, 255, 255);
            for (int i = id + 1; i < subNote.Length; i++)
            {
                if (subNote[i].channel < 0) continue;

                int sub_center_x = x + dis_x * subNote[i].end;
                int sub_center_y = dis_y +
                    (int)(((double)(Height - dis_y * 2) / subNote[i].rythem)
                     * (subNote[i].rythem - subNote[i].timing));
                /*同じ方向*/
                if (subNote[i].channel == 0 || subNote[i].channel == 1)
                {
                    if (note.type == subNote[i].type)
                    {
                        if (note.type == 1 && (note.end - 1) == subNote[i].end)
                        {
                            DX.DrawLine(center_x, center_y,
                                sub_center_x, sub_center_y, color);
                            break;
                        }
                        else if (note.type == 3 && (note.end + 1) == subNote[i].end)
                        {
                            DX.DrawLine(center_x, center_y,
                                sub_center_x, sub_center_y, color);
                            break;
                        }
                    }
                }
                /*ジグザグ*/
                else if (subNote[i].channel == 2 || subNote[i].channel == 3)
                {
                    if (note.type == 1 && (note.end - 1) == subNote[i].end)
                    {
                        DX.DrawLine(center_x, center_y,
                            sub_center_x, sub_center_y, color);
                        break;
                    }
                    else if (note.type == 3 && (note.end + 1) == subNote[i].end)
                    {
                        DX.DrawLine(center_x, center_y,
                            sub_center_x, sub_center_y, color);
                        break;
                    }
                }
            }
        }

        public void DrawNote(Note[] note)
        {
            subNote = new Note[note.Length];
            index++;
            for (int i = 0; i < note.Length; i++)
            {
                subNote[i] = new Note(note[i]);
                /*if (subNote[i].channel >= 0)
                {
                    DX.DrawString(30, 80 + index * 20,
                        subNote[i].timing + "/" + subNote[i].rythem + " = " + subNote[i].GetTiming(), DX.GetColor(255, 255, 255));
                    index++;
                }*/

            }
            for (int i = 0; i < subNote.Length; i++)
            {
                if (subNote[i].channel >= 0)
                {
                    DrawNoteGraph(i, subNote[i]);
                }
            }
        }

        public void SetMainNote(Note[] note)
        {
            index = note.Length;
            mainNote = new Note[note.Length];
            for (int i = 0; i < note.Length; i++)
            {
                mainNote[i] = new Note(note[i]);
                //DX.DrawString(30, 80 + i * 20, mainNote[i].timing + "/" + mainNote[i].rythem + " = " + mainNote[i].GetTiming(), DX.GetColor(255, 255, 255));

            }
        }

        public void ReSetMainNote()
        {
            mainNote = new Note[0];
        }

        public void DrawGraph(int x, int y, int type)
        {
            int SizeX = 40, SizeY = 40;
            //DX.GetGraphSize(graphHandle[type - 1], out SizeX, out SizeY);
            //DX.DrawString(40, 110, graphHandle[type - 1] + " = " + SizeX + ":" + SizeY, DX.GetColor(255,255,255));
            //DX.DrawGraph(x - SizeX / 2, y - SizeY / 2, graphHandle[type - 1], 1);
            DX.DrawExtendGraph(x - SizeX / 2, y - SizeY / 2, x + SizeX / 2, y + SizeY / 2, graphHandle[type - 1], 1);
        }
    }
}
