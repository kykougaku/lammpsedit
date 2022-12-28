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
            int atomdataline = 20;

            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = ""; //textboxをリセット

                filename = openFileDialog1.FileName;
                string[] readlines = System.IO.File.ReadAllLines(@filename);//ファイル内容を取得

                //2行目で原子数を読み取り
                string[] words = readlines[1].Split(' ');
                n = Convert.ToInt32(words[0]);


                //"Atoms  # atomic")コメントを探索 → indexから原子座標データの行番号を取得
                for (int i = 0; i<readlines.Length; i++)
                {
                    if (readlines[i] == "Atoms  # atomic")
                    {
                        atomdataline = i + 2;
                        break;
                    }
                }
                //一度抜ける

                double[,] atomdata = new double[n, 5];//原子座標データを格納する二次元配列
                // l0:Atom_ID l1:Atom_Type l2-4:position x-z

                for (int i = 0; i < readlines.Length; i++)
                {
                    if (atomdataline <= i)//コメント「toms  # atomic」から2行後ろの原子座標データに到達したら
                    {
                        string[] wordss = readlines[i].Split(' ');
                        for (int j = 0; j < 5; j++)
                        {
                            atomdata[i - atomdataline, j] = Convert.ToDouble(wordss[j]);
                        }
                    }
                    else//原子座標データ前ならbox2に書き込み
                    {
                        textBox2.Text += readlines[i] + Environment.NewLine;
                    }

                }


                //ここで配列の計算編集処理

                //座標の最大最小値を表示
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

                label6.Text = Convert.ToString(atomdatax[0]);
                label7.Text = Convert.ToString(atomdatay[0]);
                label8.Text = Convert.ToString(atomdataz[0]);
                label9.Text = Convert.ToString(atomdatax[n-1]);
                label10.Text = Convert.ToString(atomdatay[n-1]);
                label11.Text = Convert.ToString(atomdataz[n-1]);

                //その他の計算編集など
                for (int i = 0; i < n; i++)
                {
                    if (atomdata[i,2] > 11.5)
                    {
                        //atomdata[i, 1] += 2;
                    }
                }

                //progress barの設定
                progressBar1.Maximum = n;

                //box2に書き込み
                string boxtext = "";//宣言のみだとnullになる。このようにすることで文字列を追加できる
                for(int i = 0; i < n; i++)
                {
                    for(int j = 0; j < 5; j++)
                    {
                        boxtext += Convert.ToString(atomdata[i,j]) + " ";
                    }
                    boxtext += Environment.NewLine;
                    progressBar1.PerformStep();
                }
                textBox2.AppendText(boxtext);//textboxへの文字列追加はファイル読み込みより時間がかかる。一度に行えば造作もない
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

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}