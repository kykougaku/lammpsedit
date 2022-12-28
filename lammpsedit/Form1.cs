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
                    if (linenum == 2)//2�s�ڂŌ��q����ǂݎ��
                    {
                        string[] words = line.Split(' ');
                        n = Convert.ToInt32(words[0]);
                        break;
                    }

                    linenum++;
                }
            }

            //���������čŏ�����ǂݒ���(n���������Ĕz��錾�����邽�߂�foreach�𔲂��Ȃ���΂����Ȃ�)
            linenum = 1;
            double[,] atomdata = new double[n, 5];//���q���W�f�[�^���i�[����񎟌��z��
            // l0:Atom_ID l1:Atom_Type l2-4:position x-z

            string[] atomdatas = new string[n+20];

            foreach (string line in System.IO.File.ReadLines(@filename))
            {
                if (line == "Atoms  # atomic") atomdataline = linenum + 2;//�R�����g�utoms  # atomic�v����������t���O�𗧂Ă�


                if ( linenum >= atomdataline)//�R�����g�utoms  # atomic�v����2�s���̌��q���W�f�[�^�ɓ��B������
                {
                    string[] words = line.Split(' ');
                    for(int j = 0; j<5; j++)
                    {
                        atomdata[linenum - atomdataline, j] = Convert.ToDouble(words[j]);
                    }
                }
                else//���q���W�f�[�^�O�Ȃ�box2�ɏ�������
                {
                    textBox2.Text += line + Environment.NewLine;
                }

                linenum++;
            }

            //�����Ŕz��̌v�Z�ҏW����
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

            //box2�ɏ�������
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
    }
}