using BioMetrixCore;
using MarcacionesCEL.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarcacionesCEL
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var resultado = EvaluarConexion(txtUsuario.Text, txtContrasenya.Text);
            if(resultado.Value)
            {
                this.Hide();
                Master master = new Master(txtUsuario.Text, txtContrasenya.Text);
                master.Show();
            } else
            {
                MessageBox.Show(resultado.Message);
            }
        }

        private BoolModel EvaluarConexion(string usuario, string contrasenya)
        {
            string connectionString = "Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 192.168.1.14)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = FINANC)));User ID=" + usuario + ";Password=" + contrasenya + ";";
            string query = "select * from TEMP_MRC_MARCADORES  where rownum= 1  ";

            try
            {
                using (OracleConnection con = new OracleConnection(connectionString))
                {
                    con.Close();
                    con.Open();
                    OracleCommand cmd = new OracleCommand(query, con);
                    OracleDataAdapter oda = new OracleDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    oda.Fill(ds);

                    return new BoolModel { Value = true, Message = "" };

                }
            }
            catch (Exception ex)
            {
                return new BoolModel { Value = false, Message = ex.Message };
            }
        }

        
    }
}
