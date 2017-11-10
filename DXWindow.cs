using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using DxLibDLL;

namespace CinderellaEditor
{
    class DXWindow
    {
        int Width = 400;
        int Height = 700;
        MusicGraphics graph;
        public DXWindow(Form parent)
        {
            DX.SetUserWindow(parent.Handle); //DxLibの親ウインドウをこのフォームウインドウにセット
            DX.DxLib_Init();

            // ウィンドウモードに切り替え
            DX.ChangeWindowMode(DX.TRUE);

            // ウィンドウのサイズ指定
            //DX.SetGraphMode(SIZE_X, SIZE_Y, 32);
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);

            // DXライブラリの初期化
            if (DX.DxLib_Init() == -1)
            {
                // 初期化に失敗した場合は終了
                return;
            }
            graph = new MusicGraphics(Width, Height);
            DrawText(0, 100, "設定完了");
        }

        public void DrawText(int x, int y, String text)
        {
            DX.DrawString(x, y, text, DX.GetColor(255, 255, 255));
        }

        public void DrawMainView()
        {
            DX.ClearDrawScreen();
            DrawWindowLine();
            graph.DrawNoteLines();
            graph.DrawRhythmLine();
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
    }
}
