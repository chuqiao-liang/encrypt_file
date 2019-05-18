using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Lab7
{
    public partial class Form1 : Form
    {
        string errormissingfile = "Could not open source or destination file.";
        string errormissingkey = "Please enter a key.";
        string errornotenc = "Not a .enc file.";
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button_browse_Click(object sender, EventArgs e)
        {
            if (openDlg.ShowDialog(this) == DialogResult.OK)
                fileBox.Text = openDlg.FileName;
        }

        private void button_encrypt_Click(object sender, EventArgs e)
        {
            // first check for valid file and key.
            bool fileexist = File.Exists(fileBox.Text);
            if (keyBox.Text == "")
            {
                MessageBox.Show(errormissingkey, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else if (!fileexist)
            {
                MessageBox.Show(errormissingfile, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int rbyte;
            int pos = 0;    //position in key string
            int length = keyBox.Text.Length; //length of key
            byte kbyte, ebyte; //encrypted byte

            string message = "Operation completed successfully.";
            MessageBoxButtons buttonOK = MessageBoxButtons.OK;
            FileStream fin = new FileStream(fileBox.Text, FileMode.Open); // reading form this file
            FileStream fout = new FileStream(fileBox.Text + ".enc", FileMode.Create);

            while ((rbyte = fin.ReadByte()) != -1)
            {
                kbyte = (byte)keyBox.Text[pos];
                ebyte = (byte)(rbyte ^ kbyte);
                fout.WriteByte(ebyte);
                ++pos;
                if (pos == length)
                    pos = 0;
            }

            DialogResult result;
            result = MessageBox.Show(message, "", buttonOK);
            fin.Close();
            fout.Close();
        }

        private void button_decrypt_Click(object sender, EventArgs e)
        {
            // first check for valid file and key.
            bool fileexist = File.Exists(fileBox.Text);

            if (keyBox.Text == "")
            {
                MessageBox.Show(errormissingkey, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!fileexist)
            {
                MessageBox.Show(errormissingfile, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string substr = fileBox.Text.Substring(fileBox.Text.Length - 4);
            if (substr != ".enc")
            {
                MessageBox.Show(errornotenc, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int rbyte;
            int pos = 0;    //position in key string
            int length = keyBox.Text.Length; //length of key
            byte kbyte, ebyte; //encrypted byte
           
            string undoenc = fileBox.Text.Replace(".enc", "");
            string overwrite = "Output file exists. Overwrite?";
            MessageBoxButtons buttonYesNo = MessageBoxButtons.YesNo;
            bool fileexistenc = File.Exists(undoenc);
            DialogResult dialogresult = DialogResult.No;

            if (fileexistenc)
            {
                dialogresult = MessageBox.Show(overwrite, "", buttonYesNo, MessageBoxIcon.Question);
            }

            if (dialogresult == DialogResult.Yes || !fileexistenc)
            {
                FileStream fin = new FileStream(fileBox.Text, FileMode.Open); // reading form this file
                FileStream fout = new FileStream(undoenc, FileMode.Create);

                while ((rbyte = fin.ReadByte()) != -1)
                {
                    kbyte = (byte)keyBox.Text[pos];
                    ebyte = (byte)(rbyte ^ kbyte);
                    fout.WriteByte(ebyte);
                    ++pos;
                    if (pos == length)
                        pos = 0;
                }
                DialogResult result;
                result = MessageBox.Show("Operation completed successfully.", "", MessageBoxButtons.OK);

                fin.Close();
                fout.Close();
            }
            else if (dialogresult == DialogResult.No) return;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
