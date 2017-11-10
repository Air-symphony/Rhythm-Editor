
#define USE_DIRECTX // DirectXが使用されている

using System;
using System.Windows.Forms;

#if USE_DIRECTX
using SlimDX.Windows;
#endif

namespace DirectXSample
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Form1 mainForm = null;
            Application.EnableVisualStyles();            
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                // Mainウィンドウ
                mainForm = new Form1();

#if USE_DIRECTX // DirectXを使う場合
                MessagePump.Run(mainForm, mainForm.MainLoop);

#else // アプリケーションクラスを使う場合
                Application.Run(mainForm);

#endif
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                MessageBox.Show(
                    ex.Message,
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                // Formの開放
                if (mainForm != null)
                {
                    if (!mainForm.IsDisposed)
                        mainForm.Dispose();

                    mainForm = null;
                }
            }
        }
    }
}
