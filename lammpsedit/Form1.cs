using System;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;

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
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////zinit
            if (comboBox1.SelectedItem == "zinit")
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
                    for (int i = 0; i < readlines.Length; i++)
                    {
                        if (readlines[i] == "Atoms  # atomic")
                        {
                            atomdataline = i + 2;
                            break;
                        }
                    }
                    //一度抜ける

                    double[,] atomdata = new double[n, 5];//原子座標データを格納する二次元配列
                    // r0:Atom_ID r1:Atom_Type r2-4:position x-z

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
                    label9.Text = Convert.ToString(atomdatax[n - 1]);
                    label10.Text = Convert.ToString(atomdatay[n - 1]);
                    label11.Text = Convert.ToString(atomdataz[n - 1]);

                    //その他の計算編集など
                    for (int i = 0; i < n; i++)
                    {
                        if (atomdata[i, 2] > 19.8 && atomdata[i, 1] == 2)
                        {
                            atomdata[i, 1] = 6;
                        }
                    }

                    //progress barの設定
                    progressBar1.Maximum = n;

                    //box2に書き込み
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            sb.Append(Convert.ToString(atomdata[i, j]) + " ");
                        }
                        sb.Append(Environment.NewLine);
                        progressBar1.PerformStep();
                    }
                    textBox2.Text = sb.ToString();//textboxへの文字列追加はファイル読み込みより時間がかかる。一度に行えば造作もない
                }
            }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////lammpstrj
            else if(comboBox1.SelectedItem == "lammpstrj") 
            {
                int framenumber = 0;
                int datatype = 8;

                DialogResult dr = openFileDialog1.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    textBox2.Text = ""; //textboxをリセット

                    filename = openFileDialog1.FileName;
                    string[] readlines = System.IO.File.ReadAllLines(@filename);//ファイル内容を取得

                    //ITEM: TIMESTEPコメントを探索 → framenumberをカウント & maxatomsを読み取る
                    int atoms = 0;
                    int maxatoms = 0;
                    for (int i = 0; i < readlines.Length; i++)
                    {
                        if (readlines[i] == "ITEM: TIMESTEP")
                        {
                            framenumber++;
                            atoms = Convert.ToInt32(readlines[i+3]);
                            if (atoms > maxatoms) maxatoms = atoms;
                        }
                    }
                    //一度抜ける

                    //ITEM: NUMBER OF ATOMSコメントの行インデックスを配列に格納 & number of atomsをnatoms配列に格納
                    int[] natomsindex = new int[framenumber];
                    int[] natoms = new int[framenumber];
                    int nn = 0;
                    for (int i = 0; i < readlines.Length; i++)
                    {
                        if (readlines[i] == "ITEM: NUMBER OF ATOMS")
                        {
                            natomsindex[nn] = i;
                            natoms[nn] = Convert.ToInt32(readlines[i+1]);
                            nn++;
                        }
                    }

                    double[,,] satomdata = new double[framenumber, maxatoms, datatype];//原子座標データを格納する三次元配列
                     //r0:Atom_ID   r1:Atom_Type   r2-4:position_x-z   r5-7:compute

                    for (int i=0; i<framenumber; i++)
                    {
                        for(int j=0; j<natoms[i]; j++)
                        {
                            string[] wordss = readlines[natomsindex[i]+7+j].Split(' ');
                            for (int k = 0; k < datatype; k++)
                            {
                                satomdata[i,j,k] = Convert.ToDouble(wordss[k]);
                            }
                        }
                    }


                    //その他の計算編集など
                    for (int i = 0; i < framenumber; i++)
                    {
                        for (int j = 0; j < natoms[i]; j++)
                        {
                            if (satomdata[i, j, 2] > 0.30200547 && satomdata[i, j, 1] == 2)
                            {
                                satomdata[i, j, 1] = 6;
                            }
                        }
                    }


                    //progress barの設定
                    progressBar1.Maximum = framenumber;
                    progressBar1.Value = 0;
                    StringBuilder sb = new StringBuilder();
                    //フレームごとにboxに書き込み
                    for (int i = 0; i < framenumber; i++)
                    {
                        for(int l = natomsindex[i] - 2; l< natomsindex[i] + 7; l++)
                        {
                                sb.Append(readlines[l] + Environment.NewLine);
                        }
                        for (int j = 0; j < natoms[i]; j++)
                        {
                            for (int k = 0; k < datatype; k++)
                            {
                                sb.Append(Convert.ToString(satomdata[i, j, k]) + " ");
                            }
                            sb.Append(Environment.NewLine);
                        }
                        progressBar1.PerformStep();
                    }
                    textBox2.Text = sb.ToString();//textboxへの文字列追加はファイル読み込みより時間がかかる。一度に行えば造作もない
                    MessageBox.Show("読み込み完了", "完了", MessageBoxButtons.OK);//メッセージボックスで完了通知
                    progressBar1.Value = 0;
                }
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem  == "zinit")
            {
                saveFileDialog1.Filter = "結晶構造データ|*.zinit|すべてのファイル|*.*";
                openFileDialog1.Filter = "結晶構造データ|*.zinit|すべてのファイル|*.*";
            }
            else if(comboBox1.SelectedItem == "lammpstrj")
            {
                saveFileDialog1.Filter = "結晶構造アニメーション|*.lammpstrj|すべてのファイル|*.*";
                openFileDialog1.Filter = "結晶構造アニメーション|*.lammpstrj|すべてのファイル|*.*";
            }
        }
    }
}