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
                textBox2.Text = ""; //textbox�����Z�b�g

                filename = openFileDialog1.FileName;
                string[] readlines = System.IO.File.ReadAllLines(@filename);//�t�@�C�����e���擾

                //2�s�ڂŌ��q����ǂݎ��
                string[] words = readlines[1].Split(' ');
                n = Convert.ToInt32(words[0]);


                //"Atoms  # atomic")�R�����g��T�� �� index���猴�q���W�f�[�^�̍s�ԍ����擾
                for (int i = 0; i<readlines.Length; i++)
                {
                    if (readlines[i] == "Atoms  # atomic")
                    {
                        atomdataline = i + 2;
                        break;
                    }
                }
                //��x������

                double[,] atomdata = new double[n, 5];//���q���W�f�[�^���i�[����񎟌��z��
                // l0:Atom_ID l1:Atom_Type l2-4:position x-z

                for (int i = 0; i < readlines.Length; i++)
                {
                    if (atomdataline <= i)//�R�����g�utoms  # atomic�v����2�s���̌��q���W�f�[�^�ɓ��B������
                    {
                        string[] wordss = readlines[i].Split(' ');
                        for (int j = 0; j < 5; j++)
                        {
                            atomdata[i - atomdataline, j] = Convert.ToDouble(wordss[j]);
                        }
                    }
                    else//���q���W�f�[�^�O�Ȃ�box2�ɏ�������
                    {
                        textBox2.Text += readlines[i] + Environment.NewLine;
                    }

                }


                //�����Ŕz��̌v�Z�ҏW����

                //���W�̍ő�ŏ��l��\��
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

                //���̑��̌v�Z�ҏW�Ȃ�
                for (int i = 0; i < n; i++)
                {
                    if (atomdata[i,2] > 11.5)
                    {
                        //atomdata[i, 1] += 2;
                    }
                }

                //progress bar�̐ݒ�
                progressBar1.Maximum = n;

                //box2�ɏ�������
                string boxtext = "";//�錾�݂̂���null�ɂȂ�B���̂悤�ɂ��邱�Ƃŕ������ǉ��ł���
                for(int i = 0; i < n; i++)
                {
                    for(int j = 0; j < 5; j++)
                    {
                        boxtext += Convert.ToString(atomdata[i,j]) + " ";
                    }
                    boxtext += Environment.NewLine;
                    progressBar1.PerformStep();
                }
                textBox2.AppendText(boxtext);//textbox�ւ̕�����ǉ��̓t�@�C���ǂݍ��݂�莞�Ԃ�������B��x�ɍs���Α�����Ȃ�
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
            //box2����̕ۑ�����
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                savename = saveFileDialog1.FileName;
                StreamWriter sw = new System.IO.StreamWriter(savename, false);
                sw.Write(textBox2.Text);
                sw.Close();
                MessageBox.Show("�ۑ��ł��܂���", "����", MessageBoxButtons.OK);//���b�Z�[�W�{�b�N�X�Ŋ����ʒm
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