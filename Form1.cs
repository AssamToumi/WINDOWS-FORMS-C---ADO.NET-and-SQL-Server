using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection("Data Source=WESSVIP\\SQLEXPRESS;Initial Catalog=YOUTUBE;Integrated Security=True");
        SqlDataAdapter da = new SqlDataAdapter();

        BindingSource tblNamesBS = new BindingSource();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Connect to Database //
            //SqlConnection con = new SqlConnection("Data Source=WESSVIP\\SQLEXPRESS;Initial Catalog=YOUTUBE;Integrated Security=True");
            //con.Open();
            //MessageBox.Show(con.State.ToString());
            //con.Close();

            // Insert Into Database //
            da.InsertCommand = new SqlCommand("INSERT INTO tblContacts VALUES (@FIRSNAME, @LASTNAME)", con);
            da.InsertCommand.Parameters.Add("@FIRSNAME", SqlDbType.VarChar).Value = txtFirstName.Text;
            da.InsertCommand.Parameters.Add("@LASTNAME", SqlDbType.VarChar).Value = txtLastName.Text;
            
            con.Open();
            da.InsertCommand.ExecuteNonQuery();
            con.Close();
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            da.SelectCommand = new SqlCommand("SELECT * FROM tblContacts", con);
            
            ds.Clear();
           
            da.Fill(ds);

            dg.DataSource = ds.Tables[0];

            tblNamesBS.DataSource = ds.Tables[0];

            txtFirstName.DataBindings.Add(new Binding("Text", tblNamesBS, "FIRSNAME"));
            txtLastName.DataBindings.Add(new Binding("Text", tblNamesBS, "LASTNAME"));
            records();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tblNamesBS.MoveNext();
            dgUpdate();
            records();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            tblNamesBS.MovePrevious();
            dgUpdate();
            records();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            tblNamesBS.MoveFirst();
            dgUpdate();
            records();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            tblNamesBS.MoveLast();
            dgUpdate();
            records();
        }

        private void dgUpdate()
        {
            dg.ClearSelection();
            dg.Rows[tblNamesBS.Position].Selected = true;
            records();
        }
        private void records()
        {
            label3.Text = "Record " + tblNamesBS.Position + " of " + (tblNamesBS.Count - 1);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int x; 
            
            da.UpdateCommand = new SqlCommand("UPDATE tblContacts SET FIRSNAME = @FIRSNAME, LASTNAME = @LASTNAME WHERE ID = @ID", con);
            da.UpdateCommand.Parameters.Add("@FIRSNAME", SqlDbType.VarChar).Value = txtFirstName.Text;
            da.UpdateCommand.Parameters.Add("@LASTNAME", SqlDbType.VarChar).Value = txtLastName.Text;
            da.UpdateCommand.Parameters.Add("@ID", SqlDbType.Int).Value = ds.Tables[0].Rows[tblNamesBS.Position][0];

            con.Open();
            x =  da.UpdateCommand.ExecuteNonQuery();
            con.Close();

            if (x >= 1)
                MessageBox.Show("Record(s) has been updated");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr;

            dr = MessageBox.Show("Are you sure to Delete?\n There is no undo once date is deleted" , "Confirm Deletion", MessageBoxButtons.YesNo);
         
            if (dr == DialogResult.Yes)
            {
                da.DeleteCommand = new SqlCommand("DELETE FROM tblContacts WHERE ID = @ID", con);
                da.DeleteCommand.Parameters.Add("@ID", SqlDbType.Int).Value = ds.Tables[0].Rows[tblNamesBS.Position][0];

                con.Open();
                da.DeleteCommand.ExecuteNonQuery();
                con.Close();
                ds.Clear();
                da.Fill(ds);
            }
            else
            {
                MessageBox.Show("Deletion canceled");
            }
            
        }
    }
}
