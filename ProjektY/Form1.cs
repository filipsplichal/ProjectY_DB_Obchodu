using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProjektY
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Project_Reload_All();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        //  Buttons - Přidat
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (Int32.Parse(label34.Text) < 1)
                {
                    MessageBox.Show("Nemáš dostatečná oprávnění!", "ProjektY - DB obchodu - User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (double.TryParse(textBox2.Text, out double value_double) == false)
                {
                    MessageBox.Show("Hodnota [ Cena s DPH ] není správně!", "ProjektY - DB obchodu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (value_double < 0)
                {
                    MessageBox.Show("Hodnota [ Cena s DPH ] není správně!", "ProjektY - DB obchodu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (int.TryParse(textBox8.Text, out int value_int) == false)
                {
                    MessageBox.Show("Hodnota [ Kusy ] není číslo!", "ProjektY - DB obchodu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (value_int < 0)
                {
                    MessageBox.Show("Hodnota [ Cena s DPH ] není správně!", "ProjektY - DB obchodu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string connectionString = Properties.Settings.Default.cn;
                using (SqlConnection db_connect = new SqlConnection(connectionString))
                {
                    db_connect.Open();

                    int CategoryID_Value = 0;

                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {
                        queryString.CommandText = "SELECT ID FROM Kategorie WHERE Name = @id_value";
                        queryString.Parameters.AddWithValue("@id_value", comboBox1.Text);
                        CategoryID_Value = (int)queryString.ExecuteScalar();
                    }

                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {
                        queryString.CommandText = "INSERT INTO Zbozi (Name, Prince, CategoryID, Number) VALUES (@name_value, @prince_value, @categoryID_value, @number_value)";

                        queryString.Parameters.AddWithValue("@name_value", textBox1.Text);
                        queryString.Parameters.AddWithValue("@prince_value", double.Parse(textBox2.Text));
                        queryString.Parameters.AddWithValue("@categoryID_value", CategoryID_Value.ToString());
                        queryString.Parameters.AddWithValue("@number_value", int.Parse(textBox8.Text));

                        queryString.ExecuteNonQuery();
                    }

                    db_connect.Close();
                }

                Project_Reload_All();

            }
            catch
            {

                MessageBox.Show("Chyba: P_BT0", "ProjektY - DB obchodu - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void button4_Click(object sender, EventArgs e)
        {

            try
            {

                if (Int32.Parse(label34.Text) < 1)
                {
                    MessageBox.Show("Nemáš dostatečná oprávnění!", "ProjektY - DB obchodu - User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (int.TryParse(textBox3.Text, out int value_int) == false)
                {
                    MessageBox.Show("Hodnota [ Počet kusů ] není číslo!", "ProjektY - DB obchodu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (value_int < 0)
                {
                    MessageBox.Show("Hodnota [ Počet kusů ] není správně!", "ProjektY - DB obchodu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (double.TryParse(textBox4.Text, out double value_double) == false)
                {
                    MessageBox.Show("Hodnota [ Cena s DPH ] není správně!", "ProjektY - DB obchodu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (value_double < 0)
                {
                    MessageBox.Show("Hodnota [ Cena s DPH ] není správně!", "ProjektY - DB obchodu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                if (textBox6.Text != "0")
                {
                    string connectionString = Properties.Settings.Default.cn;
                    using (SqlConnection db_connect = new SqlConnection(connectionString))
                    {
                        db_connect.Open();

                        int CategoryID_Value = 0;

                        using (SqlCommand queryString = db_connect.CreateCommand())
                        {
                            queryString.CommandText = "SELECT ID FROM Kategorie WHERE Name = @id_value";
                            queryString.Parameters.AddWithValue("@id_value", comboBox2.Text);
                            CategoryID_Value = (int)queryString.ExecuteScalar();
                        }

                        using (SqlCommand queryString = db_connect.CreateCommand())
                        {
                            queryString.CommandText = "UPDATE Zbozi SET Name = @name_value, Prince = @prince_value, CategoryID = @categoryID_value, Number = @number_value WHERE ID = @id_value";

                            queryString.Parameters.AddWithValue("@id_value", textBox6.Text);
                            queryString.Parameters.AddWithValue("@name_value", textBox7.Text);
                            queryString.Parameters.AddWithValue("@prince_value", double.Parse(textBox4.Text));
                            queryString.Parameters.AddWithValue("@categoryID_value", CategoryID_Value.ToString());
                            queryString.Parameters.AddWithValue("@number_value", textBox3.Text);

                            queryString.ExecuteNonQuery();
                        }

                        db_connect.Close();
                    }

                    Project_Reload_All();

                }
            }
            catch
            {

                MessageBox.Show("Chyba: P_BT4", "ProjektY - DB obchodu - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void button5_Click(object sender, EventArgs e)
        {
            Project_Reload_Add();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "0")
            {
                textBox3.Text = (Int32.Parse(textBox3.Text) + 1).ToString();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Int32.Parse(textBox3.Text) > 0)
            {
                textBox3.Text = (Int32.Parse(textBox3.Text) - 1).ToString();
            }
        }

        // Buttons - Sklad page
        private void button2_Click(object sender, EventArgs e)
        {

            Project_Reload_Inventory();

        }

        // Buttons - Kategorie page
        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                if (Int32.Parse(label34.Text) < 1)
                {
                    MessageBox.Show("Nemáš dostatečná oprávnění!", "ProjektY - DB obchodu - User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string connectionString = Properties.Settings.Default.cn;
                using (SqlConnection db_connect = new SqlConnection(connectionString))
                {
                    db_connect.Open();

                    int Check_Exist = 0;

                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {
                        queryString.CommandText = "SELECT COUNT(*) FROM Kategorie WHERE Name = @name_value";

                        queryString.Parameters.AddWithValue("@name_value", textBox5.Text);

                        Check_Exist = int.Parse(queryString.ExecuteScalar().ToString());
                    }

                    if (Check_Exist < 1)
                    {
                        using (SqlCommand queryString = db_connect.CreateCommand())
                        {
                            queryString.CommandText = "INSERT INTO Kategorie (Name) VALUES (@name_value)";

                            queryString.Parameters.AddWithValue("@name_value", textBox5.Text);

                            queryString.ExecuteNonQuery();
                        }
                    }
                    else
                    {

                        MessageBox.Show("Kategorie již existuje", "ProjektY - DB obchodu - Kategorie", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }

                    db_connect.Close();
                }

                Project_Reload_All();
            }
            catch
            {

                MessageBox.Show("Chyba: P_BT3", "ProjektY - DB obchodu - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Project_Reload_Category();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (Int32.Parse(label34.Text) < 1)
                {
                    MessageBox.Show("Nemáš dostatečná oprávnění!", "ProjektY - DB obchodu - User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (textBox10.Text != "0")
                {

                    string connectionString = Properties.Settings.Default.cn;
                    using (SqlConnection db_connect = new SqlConnection(connectionString))
                    {
                        db_connect.Open();

                        using (SqlCommand queryString = db_connect.CreateCommand())
                        {
                            queryString.CommandText = "UPDATE Kategorie SET Name = @name_value WHERE ID = @id_value";

                            queryString.Parameters.AddWithValue("@id_value", textBox10.Text);
                            queryString.Parameters.AddWithValue("@name_value", textBox9.Text);

                            queryString.ExecuteNonQuery();
                        }

                        db_connect.Close();
                    }

                    Project_Reload_All();

                }
            }
            catch
            {

                MessageBox.Show("Chyba: P_BT9", "ProjektY - DB obchodu - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        // Buttons - Admin

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (Int32.Parse(label34.Text) < 2)
                {

                    MessageBox.Show("Nemáš dostatečná oprávnění!", "ProjektY - DB obchodu - User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (textBox13.Text != "0")
                {

                    if (textBox13.Text != "1")
                    {

                        if (textBox15.Text != comboBox3.Text)
                        {

                            string connectionString = Properties.Settings.Default.cn;
                            using (SqlConnection db_connect = new SqlConnection(connectionString))
                            {
                                db_connect.Open();

                                using (SqlCommand queryString = db_connect.CreateCommand())
                                {
                                    queryString.CommandText = "UPDATE Zbozi SET CategoryID = ( SELECT ID FROM Kategorie WHERE Name = @name_value )  WHERE CategoryID = @id_value";

                                    queryString.Parameters.AddWithValue("@name_value", comboBox3.Text);
                                    queryString.Parameters.AddWithValue("@id_value", textBox13.Text);

                                    queryString.ExecuteNonQuery();

                                }

                                using (SqlCommand queryString = db_connect.CreateCommand())
                                {
                                    queryString.CommandText = "DELETE FROM Kategorie WHERE ID = @id_value";

                                    queryString.Parameters.AddWithValue("@id_value", textBox13.Text);

                                    queryString.ExecuteNonQuery();

                                }

                                db_connect.Close();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Nemůžeš vymazat kategorii, kam se má přesunout zboží!", "ProjektY - DB obchodu - Admin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Nemůžeš vymazat default kategorii!", "ProjektY - DB obchodu - Admin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }

                Project_Reload_All();
            }
            catch
            {

                MessageBox.Show("Chyba: P_BT10", "ProjektY - DB obchodu - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                if (Int32.Parse(label34.Text) < 2)
                {

                    MessageBox.Show("Nemáš dostatečná oprávnění!", "ProjektY - DB obchodu - User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (textBox14.Text != "0")
                {

                    string connectionString = Properties.Settings.Default.cn;
                    using (SqlConnection db_connect = new SqlConnection(connectionString))
                    {
                        db_connect.Open();

                        using (SqlCommand queryString = db_connect.CreateCommand())
                        {
                            queryString.CommandText = "DELETE FROM Zbozi WHERE ID = @id_value";

                            queryString.Parameters.AddWithValue("@id_value", textBox14.Text);

                            queryString.ExecuteNonQuery();

                        }

                        db_connect.Close();
                    }

                }

                Project_Reload_All();
            }
            catch
            {

                MessageBox.Show("Chyba: P_BT13", "ProjektY - DB obchodu - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void button12_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Project_Reload_Admin();
        }

        /*  List Selected */

        // Page - Přidat
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count >= 1)
            {
                var item = listView1.SelectedItems[0];

                string item_ID = item.SubItems[0].Text;
                string item_Name = item.SubItems[1].Text;
                string item_Prince = item.SubItems[2].Text.Trim(new Char[] { 'K', 'č' });
                item_Prince = item_Prince.Replace(" ", "").ToLower();
                item_Prince = item_Prince.Replace(".", ",").ToLower();
                string item_Category = item.SubItems[3].Text;
                string item_Number = item.SubItems[4].Text;

                textBox6.Text = item_ID;
                textBox7.Text = item_Name;
                textBox3.Text = item_Number;
                label13.Text = item_Number;
                textBox4.Text = item_Prince;
                comboBox2.SelectedIndex = comboBox2.FindStringExact(item_Category.ToString());

            }
        }

        // Page - Kategorie
        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listView3.SelectedItems.Count >= 1)
            {
                var item = listView3.SelectedItems[0];

                string item_ID = item.SubItems[0].Text;
                string item_Name = item.SubItems[1].Text;
                string item_Number = item.SubItems[2].Text;

                textBox10.Text = item_ID;
                textBox9.Text = item_Name;
                textBox11.Text = item_Number;

            }

        }

        // Page - Admin

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView4.SelectedItems.Count >= 1)
            {
                var item = listView4.SelectedItems[0];

                string item_ID = item.SubItems[0].Text;
                string item_Name = item.SubItems[1].Text;

                textBox13.Text = item_ID;
                textBox15.Text = item_Name;

            }
        }

        private void listView5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView5.SelectedItems.Count >= 1)
            {
                var item = listView5.SelectedItems[0];

                string item_ID = item.SubItems[0].Text;

                textBox14.Text = item_ID;

            }
        }

        /* Vlastní funkce */

        // Page - Reload all

        private void Project_Reload_All()
        {
            
            Project_Reload_Add();
            Project_Reload_Category();

            
            Project_Reload_Inventory();
            Project_Reload_Admin();
        }


        // Page - Přidat
        private void Project_Reload_Add()
        {

            try
            {

                // Add clear
                textBox1.Text = "";
                textBox2.Text = "";
                textBox8.Text = "";
                comboBox1.Items.Clear();

                // Data number clear

                label8.Text = "0";
                label9.Text = "0";

                // Update clear
                comboBox2.Items.Clear();
                listView1.Items.Clear();
                textBox6.Text = "0";
                textBox7.Text = "Null";
                textBox3.Text = "0";
                label13.Text = "0";
                textBox4.Text = "0";
                comboBox2.Items.Clear();

                // Load
                string connectionString = Properties.Settings.Default.cn;
                using (SqlConnection db_connect = new SqlConnection(connectionString))
                {
                    db_connect.Open();

                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {
                        queryString.CommandText = "SELECT COUNT(*) FROM Zbozi";
                        label8.Text = queryString.ExecuteScalar().ToString();
                    }

                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {
                        queryString.CommandText = "SELECT COUNT(*) FROM Kategorie";
                        label9.Text = queryString.ExecuteScalar().ToString();
                    }

                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {

                        queryString.CommandText = "SELECT Name FROM Kategorie";
                        using (SqlDataReader dataReader = queryString.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                comboBox1.Items.Add(dataReader["Name"].ToString());
                                comboBox2.Items.Add(dataReader["Name"].ToString());
                            }
                        }

                    }

                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {

                        queryString.CommandText = "SELECT Zbozi.ID,Zbozi.Name,Zbozi.Prince,Kategorie.Name AS C_Name,Zbozi.Prince, Zbozi.Number FROM Zbozi,Kategorie WHERE Zbozi.CategoryID = Kategorie.Id";
                        using (SqlDataReader dataReader = queryString.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                string[] row = { dataReader["ID"].ToString(), dataReader["Name"].ToString(), String.Format(CultureInfo.InvariantCulture, "{0:### ### ##0.00} Kč", dataReader["Prince"]), dataReader["C_Name"].ToString(), dataReader["Number"].ToString() };
                                var listViewItem = new ListViewItem(row);
                                listView1.Items.Add(listViewItem);
                            }
                        }

                    }

                    db_connect.Close();
                }

                // auto resize
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch
            {

                MessageBox.Show("Chyba: FC_RE_ADD", "ProjektY - DB obchodu - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        // Page - Sklad
        private void Project_Reload_Inventory()
        {

            try
            {
                // clear
                listView2.Items.Clear();

                // load
                string connectionString = Properties.Settings.Default.cn;
                using (SqlConnection db_connect = new SqlConnection(connectionString))
                {
                    db_connect.Open();

                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {

                        queryString.CommandText = "SELECT Zbozi.ID,Zbozi.Name,Zbozi.Prince,Kategorie.Name AS C_Name,Zbozi.Prince, Zbozi.Number FROM Zbozi,Kategorie WHERE Zbozi.CategoryID = Kategorie.Id";
                        using (SqlDataReader dataReader = queryString.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                string[] row = { dataReader["ID"].ToString(), dataReader["Name"].ToString(), String.Format(CultureInfo.InvariantCulture, "{0:### ### ##0.00} Kč", dataReader["Prince"]), dataReader["C_Name"].ToString(), dataReader["Number"].ToString() };
                                var listViewItem = new ListViewItem(row);
                                listView2.Items.Add(listViewItem);
                            }
                        }

                    }

                    db_connect.Close();
                }

                // auto resize
                listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch
            {

                MessageBox.Show("Chyba: FC_RE_IN", "ProjektY - DB obchodu - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        // Page - Kategorie
        private void Project_Reload_Category() 
        {

            try
            {

                // Add clear
                textBox5.Text = "";

                // Data number clear
                label18.Text = "0";
                label17.Text = "0";

                // Update clear
                textBox10.Text = "0";
                textBox9.Text = "Null";
                textBox11.Text = "0";
                listView3.Items.Clear();

                // Load
                string connectionString = Properties.Settings.Default.cn;
                using (SqlConnection db_connect = new SqlConnection(connectionString))
                {
                    db_connect.Open();

                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {
                        queryString.CommandText = "SELECT COUNT(*) FROM Zbozi";
                        label18.Text = queryString.ExecuteScalar().ToString();
                    }

                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {
                        queryString.CommandText = "SELECT COUNT(*) FROM Kategorie";
                        label17.Text = queryString.ExecuteScalar().ToString();
                    }

                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {

                        queryString.CommandText = "SELECT Kategorie.ID, Kategorie.Name, ( SELECT COUNT(Zbozi.CategoryID) FROM Zbozi WHERE Kategorie.ID = Zbozi.CategoryID ) AS N_Zbozi FROM Kategorie";
                        using (SqlDataReader dataReader = queryString.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                string[] row = { dataReader["ID"].ToString(), dataReader["Name"].ToString(), dataReader["N_Zbozi"].ToString() };
                                var listViewItem = new ListViewItem(row);
                                listView3.Items.Add(listViewItem);
                            }
                        }

                    }

                    db_connect.Close();
                }

                // auto resize
                listView3.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView3.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch
            {

                MessageBox.Show("Chyba: FC_RE_CA", "ProjektY - DB obchodu - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        // Page - Admin
        private void Project_Reload_Admin()
        {

            try
            {

                // Kategorie

                listView4.Items.Clear();
                listView5.Items.Clear();
                comboBox3.Items.Clear();

                textBox13.Text = "0";
                textBox14.Text = "0";


                string connectionString = Properties.Settings.Default.cn;
                using (SqlConnection db_connect = new SqlConnection(connectionString))
                {
                    db_connect.Open();

                    //Kategorie
                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {

                        queryString.CommandText = "SELECT Kategorie.ID, Kategorie.Name, ( SELECT COUNT(Zbozi.CategoryID) FROM Zbozi WHERE Kategorie.ID = Zbozi.CategoryID ) AS N_Zbozi FROM Kategorie";
                        using (SqlDataReader dataReader = queryString.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                string[] row = { dataReader["ID"].ToString(), dataReader["Name"].ToString(), dataReader["N_Zbozi"].ToString() };
                                var listViewItem = new ListViewItem(row);
                                listView4.Items.Add(listViewItem);
                            }
                        }

                    }


                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {

                        queryString.CommandText = "SELECT Name FROM Kategorie";
                        using (SqlDataReader dataReader = queryString.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                comboBox3.Items.Add(dataReader["Name"].ToString());
                                comboBox3.SelectedIndex = 0;
                            }
                        }

                    }

                    //Zbozi
                    using (SqlCommand queryString = db_connect.CreateCommand())
                    {

                        queryString.CommandText = "SELECT Zbozi.ID,Zbozi.Name,Zbozi.Prince,Kategorie.Name AS C_Name,Zbozi.Prince, Zbozi.Number FROM Zbozi,Kategorie WHERE Zbozi.CategoryID = Kategorie.Id";
                        using (SqlDataReader dataReader = queryString.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                string[] row = { dataReader["ID"].ToString(), dataReader["Name"].ToString(), String.Format(CultureInfo.InvariantCulture, "{0:### ### ##0.00} Kč", dataReader["Prince"]), dataReader["C_Name"].ToString(), dataReader["Number"].ToString() };
                                var listViewItem = new ListViewItem(row);
                                listView5.Items.Add(listViewItem);
                            }
                        }

                    }

                    db_connect.Close();
                }

                // auto resize
                listView4.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView4.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                listView5.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView5.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch
            {

                MessageBox.Show("Chyba: FC_RE_AD", "ProjektY - DB obchodu - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public void Project_SetLvl( string name, int lvl )
        {

            label35.Text = name.ToString();
            label34.Text = lvl.ToString();
        
        }

    }
}

