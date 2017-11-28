﻿using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using CinderellaEditor;
using SlimDX.DirectInput;
using System.Runtime.InteropServices;
using System.Text;

namespace DirectXSample
{
    public partial class Form1 : Form
    {
        const int EM_LINEINDEX = 0xBB;
        const int EM_LINEFROMCHAR = 0xC9;

        [DllImport("User32.Dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        
        DXWindow child;
        int index = 1;
        string FileName = "";
        string WindowName = "Cinderella Editor";
        string[] cmds;
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
            上書き保存ToolStripMenuItem.Enabled = false;
            cmds = Environment.GetCommandLineArgs();
            if (cmds.Length > 1)
            {
                FileName = cmds[0];
                FileName = cmds[1];
                textBox1.Text = File.ReadAllText(FileName);
            }
            this.Text = WindowName + "  " + cmds[0];
        }

        /// <summary>
        /// 表示開始
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {

            if (child != null)
            {
                child.DrawView();
            }
            base.OnShown(e);
        }

        /// <summary>
        /// リサイズ
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            textBox1.Width = this.ClientSize.Width / 2;
            textBox1.Height = this.ClientSize.Height;
            if (child != null)
            {
                child.DrawView();
            }
            base.OnResize(e);
        }

        /// <summary>
        /// 更新メソッド
        /// </summary>
        public void MainLoop()
        {

        }
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            //dialog.Title = "ファイルを保存する";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, textBox1.Text);
                FileName = dialog.FileName;
            }
            this.Text = WindowName + "  " + this.FileName;
        }

        private void 読み込みToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            //dialog.Title = "ファイルを読み込む";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = File.ReadAllText(dialog.FileName);
                FileName = dialog.FileName;
            }
            this.Text = WindowName + "  " + this.FileName;
        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 元に戻すToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Undo();
        }

        private void MenuItemFileSave_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(this.FileName, textBox1.Text,
                    Encoding.GetEncoding("UTF-8"));
            // タイトルバーにファイル名を表示する
            this.Text = WindowName + "  " + this.FileName;
            // ファイル名が判明したため「上書き保存」を有効にする
            上書き保存ToolStripMenuItem.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.WordWrap = false;
            textBox1.ScrollBars = ScrollBars.Both;
            int index = SendMessage(textBox1.Handle, EM_LINEFROMCHAR, -1, 0);
            string[] text = SearchLine(index);
            if (text != null)
            {
                child.DrawNote(text);
            }
            this.Text = WindowName + "* " + this.FileName;
            上書き保存ToolStripMenuItem.Enabled = true;
        }

        private void textBox1_OnMouseClickHandler(object sender, MouseEventArgs e)
        {
            int index = SendMessage(textBox1.Handle, EM_LINEFROMCHAR, -1, 0);
            string[] text = SearchLine(index);
            if (text != null)
            {
                child.DrawNote(text);
            }
        }

        private void textBox1_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            int maxIndex = (textBox1.Lines.Length - 1 >= 0) ? textBox1.Lines.Length - 1 : 0;
            //child.DrawText(10, 100, e.KeyCode.ToString(), true);
            if (e.KeyCode.ToString().Equals("Up") && index > 0)
            {
                index = SendMessage(textBox1.Handle, EM_LINEFROMCHAR, -1, 0) - 1;
            }
            else if (e.KeyCode.ToString().Equals("Down") && index < maxIndex)
            {
                index = SendMessage(textBox1.Handle, EM_LINEFROMCHAR, -1, 0) + 1;
            }
            else
            {
                index = SendMessage(textBox1.Handle, EM_LINEFROMCHAR, -1, 0);
            }

            string[] text = SearchLine(index);
            if (text != null)
            {
                child.DrawNote(text);
            }
        }

        private string[] SearchLine(int index)
        {
            string[] text = null;
            try
            {
                int maxIndex = textBox1.Lines.Length;

                int size = 1;
                int verticalSize = 2;
                for (int i = 0; i < verticalSize; i++)
                {
                    if (index > i)
                    {
                        size++;
                    }
                    if (index < maxIndex - (i + 1))
                    {
                        size++;
                    }
                }
                text = new string[size];
                text[0] = textBox1.Lines[index];
                int id = 1;

                for (int i = 0; i < verticalSize; i++)
                {
                    if (index > i)
                    {
                        text[id] = textBox1.Lines[index - (i + 1)];
                        id++;
                    }
                    if (index < maxIndex - (i + 1))
                    {
                        text[id] = textBox1.Lines[index + (i + 1)];
                        id++;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.Write(ex.Message + "\n");
                text = null;
            }
            return text;
        }
    }
}
