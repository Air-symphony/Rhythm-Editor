using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using CinderellaEditor;

namespace DirectXSample
{
    public partial class Form1 : Form
    {
        //DirectXWindow child;
        DXWindow child;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Form1()
        {
            // デザイナー設定
            InitializeComponent();
            child = new DXWindow(this);

            textBox1.Width = this.ClientSize.Width / 2;
            textBox1.Height = this.ClientSize.Height;

            // DirectXウィンドウの追加
            //child = new DirectXWindow(this);
        }

        /// <summary>
        /// 表示開始
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            // DirectX子ウィンドウの表示を開始
            /*if (child != null && !child.IsDisposed)
            {
                child.Show();
                child.Idle();
            }*/
            base.OnShown(e);
        }

        /// <summary>
        /// リサイズ
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            // 子ウィンドウのリサイズ
            /*if (child != null && child.Visible)
            {
                // 親の描画領域ぴったりに合わせる
                child.Width = this.ClientSize.Width - 100;
                child.Height = this.ClientSize.Height - 100;
            }*/
            textBox1.Width = this.ClientSize.Width / 2;
            textBox1.Height = this.ClientSize.Height;
            if (child != null)
            {
                child.Resize(this.ClientSize.Width / 2, this.ClientSize.Height);
            }
            base.OnResize(e);
        }

        /// <summary>
        /// 更新メソッド
        /// </summary>
        public void MainLoop()
        {
            // DirectXウィンドウ更新
            /*if (child != null && child.Visible)
            {
                child.Idle();
            }*/
            child.DrawMainView();
        }
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            //dialog.Title = "ファイルを保存する";
            if (dialog.ShowDialog() == DialogResult.OK)
                File.WriteAllText(dialog.FileName, textBox1.Text);
        }

        private void 読み込みToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            //dialog.Title = "ファイルを読み込む";
            if (dialog.ShowDialog() == DialogResult.OK)
                textBox1.Text = File.ReadAllText(dialog.FileName);
        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 元に戻すToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Undo();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Multiline = true;
            textBox1.WordWrap = false;
            textBox1.ScrollBars = ScrollBars.Both;
            //child.DrawText(0, 0, textBox1.Text);
            child.DrawMainView();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
