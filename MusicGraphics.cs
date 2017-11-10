using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace CinderellaEditor
{
    class MusicGraphics
    {
        private int Width;
        private int Height;
        private int distance = 50;
        private int x = 0;
        private int y = 10;
        public MusicGraphics(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
            distance = (int)(Width / 6.0);
        }

        public void DrawNoteLines()
        {
            int count = 5;
            uint color = DX.GetColor(200, 200, 200);
            for (int i = 1; i <= count; i++)
            {
                DX.DrawLine(x + distance * i, y, x + distance * i, Height - y, color);
            }
        }

        public void DrawRhythmLine()
        {
            int count = 5, dis_y = Height / (count + 2);

            uint color = DX.GetColor(100, 100, 100);
            for (int i = 1; i <= count; i++)
            {
                DX.DrawLine(x + distance * 1 - 20, dis_y * i, x + distance * 5 + 20, dis_y * i, color);
            }
        }
    }
}
