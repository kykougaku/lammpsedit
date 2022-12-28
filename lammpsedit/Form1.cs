using System;
using System.IO;


namespace lammpsedit
{
    public partial class Form1 : Form
    {
        int n = 0;
        string filename;
        string savename;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int linenum = 1;
            int atomdataline = 20;

            textBox2.Text = "";

            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                filename = openFileDialog1.FileName;

                foreach (string line in System.IO.File.ReadLines(@filename))
                {
                    if (linenum == 2)//2行目で原子数を読み取り
                    {
                        string[] words = line.Split(' ');
                        n = Convert.ToInt32(words[0]);
                        break;
                    }

                    linenum++;
                }
            }

            //初期化して最初から読み直し(nをもちいて配列宣言をするためにforeachを抜けなければいけない)
            linenum = 1;
            double[,] atomdata = new double[n, 5];//原子座標データを格納する二次元配列
            // l0:Atom_ID l1:Atom_Type l2-4:position x-z

            string[] atomdatas = new string[n+20];

            foreach (string line in System.IO.File.ReadLines(@filename))
            {
                if (line == "Atoms  # atomic") atomdataline = linenum + 2;//コメント「toms  # atomic」を見つけたらフラグを立てる


                if ( linenum >= atomdataline)//コメント「toms  # atomic」から2行後ろの原子座標データに到達したら
                {
                    string[] words = line.Split(' ');
                    for(int j = 0; j<5; j++)
                    {
                        atomdata[linenum - atomdataline, j] = Convert.ToDouble(words[j]);
                    }
                }
                else//原子座標データ前ならbox2に書き込み
                {
                    textBox2.Text += line + Environment.NewLine;
                }

                linenum++;
            }

            //ここで配列の計算編集処理
            double[] atomdatax = new double[n];
            double[] atomdatay = new double[n];
            double[] atomdataz = new double[n];
            for (int i = 0; i < n; i++)
            {
                atomdatax[i] = atomdata[i, 2];
                atomdatay[i] = atomdata[i, 3];
                atomdataz[i] = atomdata[i, 4];
            }
            Array.Sort(atomdatax);
            Array.Sort(atomdatay);
            Array.Sort(atomdataz);

            label9.Text = Convert.ToString(atomdatax[0]);
            label10.Text = Convert.ToString(atomdatay[0]);
            label11.Text = Convert.ToString(atomdataz[0]);
            for (int i = 0; i < n; i++)
            {
                if (atomdata[i,2] > 11.5)
                {
                    //atomdata[i, 1] += 2;
                }
            }

            //box2に書き込み
            for(int i = 0; i < n; i++)
            {
                string linew = "";
                for(int j = 0; j < 5; j++)
                {
                    linew += Convert.ToString(atomdata[i,j]) + " ";
                }
                textBox2.Text += linew + Environment.NewLine;
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //box2からの保存処理
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                savename = saveFileDialog1.FileName;
                StreamWriter sw = new System.IO.StreamWriter(savename, false);
                sw.Write(textBox2.Text);
                sw.Close();
                MessageBox.Show("保存できました", "完了", MessageBoxButtons.OK);//メッセージボックスで完了通知
            }
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}