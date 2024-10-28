using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProjektY
{
    public partial class Form2 : Form
    {

    public Form2()
        {
            InitializeComponent();
        }

        // Veřejná hesla
        private string[] Project_Pass = { "Heslo", "Heslo2", "Heslo3" };

        // Seznam uživatelů - Veřejný
        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                string connectionString = Properties.Settings.Default.cn;
                using (SqlConnection db_connect = new SqlConnection(connectionString))
                {
                    db_connect.Open();

                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {

                        queryString.CommandText = "SELECT Name, Pass, Level FROM Users";
                        using (SqlDataReader dataReader = queryString.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                comboBox1.Items.Add(dataReader["Name"].ToString());
                            }
                        }

                    }

                    db_connect.Close();

                }
            }
            catch
            {

                MessageBox.Show("Chyba: Login_0", "ProjektY - DB obchodu - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string User_Pass = "Null";
                int User_Lvl = 0;

                string connectionString = Properties.Settings.Default.cn;
                using (SqlConnection db_connect = new SqlConnection(connectionString))
                {
                    db_connect.Open();

                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {

                        queryString.CommandText = "SELECT Pass, Level FROM Users WHERE Name = @name_value";
                        queryString.Parameters.AddWithValue("@name_value", comboBox1.Text);

                        using (SqlDataReader dataReader = queryString.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                User_Pass = dataReader["Pass"].ToString();
                                User_Lvl = Int32.Parse(dataReader["Level"].ToString());
                            }
                        }

                    }

                    db_connect.Close();

                }

                if (User_Pass == Get256Hash(textBox1.Text))
                {
                    MessageBox.Show("Úspěšně přihlášen", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Form1 f1 = new Form1();
                    f1.Show();
                    f1.Project_SetLvl(comboBox1.Text, User_Lvl);
                    this.Hide();

                }
                else
                {

                    MessageBox.Show("Špatné heslo", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch
            {

                MessageBox.Show("Chyba: Login_1", "ProjektY - DB obchodu - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        
        // Veřejné heslo - Automatické doplnění hesla
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex != -1)
            {

                textBox1.Text = Project_Pass[comboBox1.SelectedIndex].ToString();

            }

        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {

            textBox1.Text = "";

        }

        // Hash 256
        static string Get256Hash(string rawData)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Close project
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
