
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using BioMetrixCore.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Net.Mail;
using MarcacionesCEL.Models;

namespace BioMetrixCore
{
    public partial class Master : Form
    {
        DeviceManipulator manipulator = new DeviceManipulator();
        public ZkemClient objZkeeper;
        private List<UbicacionesModel> marcadores = new List<UbicacionesModel>();
        private string usuario = "";
        private string contrasenya = "";

        private void ToggleControls(bool value)
        {
            btnBeep.Enabled = value;
            btnDownloadFingerPrint.Enabled = value;
            btnPullData.Enabled = value;
            btnPowerOff.Enabled = value;
            btnRestartDevice.Enabled = value;
            btnGetDeviceTime.Enabled = value;
            btnEnableDevice.Enabled = value;
            btnDisableDevice.Enabled = value;
            btnPingDevice.Enabled = value;
            btnSync.Enabled = value;
            btn_eventos.Enabled = value;
            btnSyncAll.Enabled = value;
        }

        public Master(string usuario, string contrasenya)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.contrasenya = contrasenya;
            Start();
        }

        public void Start()
        {
            ToggleControls(false);
            ShowStatusBar(string.Empty, true);
            DisplayEmpty();

            CargarMarcadores();
        }

        private void RaiseDeviceEvent(object sender, string actionType)
        {
            switch (actionType)
            {
                case UniversalStatic.acx_Disconnect:
                    {
                        ShowStatusBar("La terminal ha sido apagada", true);
                        DisplayEmpty();
                        ToggleControls(false);
                        break;
                    }
                default:
                    break;
            }

        }

        private void conectar(int index)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ShowStatusBar(string.Empty, true);

                string ipAddress = marcadores[index].IP; 
                string port = marcadores[index].PUERTO.ToString();
                if (ipAddress == string.Empty || port == string.Empty)
                    throw new Exception("La dirección IP y el número de puerto son requeridos.");

                int portNumber = 4370;
                if (!int.TryParse(port, out portNumber))
                    throw new Exception("No es un número IP válido");

                bool isValidIpA = UniversalStatic.ValidateIP(ipAddress);
                if (!isValidIpA)
                    throw new Exception("La dirección IP no es una dirección IVP4 válida.");

                isValidIpA = UniversalStatic.PingTheDevice(ipAddress);
                if (!isValidIpA)
                    throw new Exception("La terminal con IP " + ipAddress + ":" + port + " no pudo ser alcanzada.");

                objZkeeper = new ZkemClient(RaiseDeviceEvent);
                objZkeeper.Connect_Net(ipAddress, portNumber);

                string deviceInfo = manipulator.FetchDeviceInfo(objZkeeper, 1);
                 lblDeviceInfo.Text = deviceInfo;

                pullData();
                ToggleControls(true);
            }
            catch (Exception ex)
            {
                ShowStatusBar(ex.Message, false);
            }
            this.Cursor = Cursors.Default;
        }


        public void ShowStatusBar(string message, bool type)
        {
            if (message.Trim() == string.Empty)
            {
                lblStatus.Visible = false;
                return;
            }

            lblStatus.Visible = true;
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.White;

            if (type)
                lblStatus.BackColor = Color.FromArgb(0, 128, 0);
            else
                lblStatus.BackColor = Color.FromArgb(230, 112, 134);
        }


        private void btnPingDevice_Click(object sender, EventArgs e)
        {
            ShowStatusBar(string.Empty, true);

            string ipAddress = marcadores[Convert.ToInt32(cmbUbicacion.SelectedIndex)].IP;

            bool isValidIpA = UniversalStatic.ValidateIP(ipAddress);
            if (!isValidIpA)
                throw new Exception("La IP del dispositivo no es válido.");

            isValidIpA = UniversalStatic.PingTheDevice(ipAddress);
            if (isValidIpA)
                ShowStatusBar("El dispositivo se encuentra activo", true);
            else
                ShowStatusBar("No se pudo obtener resputas", false);
        }
        
        private void btnBeep_Click(object sender, EventArgs e)
        {
            objZkeeper.Beep(100);
            InsertarEvento("El usuario " + usuario + " enviado un 'beep' a la terminal " + marcadores[Convert.ToInt32(cmbUbicacion.SelectedIndex)].UBICACION );
        }

        private void btnDownloadFingerPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ShowStatusBar(string.Empty, true);

                ICollection<UserInfo> lstFingerPrintTemplates = manipulator.GetAllUserInfo(objZkeeper, 1 );
                lstFingerPrintTemplates = lstFingerPrintTemplates.OrderBy(x => x.EnrollNumber).ToList();
                if (lstFingerPrintTemplates != null && lstFingerPrintTemplates.Count > 0)
                {
                    BindToGridView(lstFingerPrintTemplates);
                    ShowStatusBar(lstFingerPrintTemplates.Count + " registros encontrados.", true);
                }
                else
                    DisplayListOutput("No se encontraron registros.");
            }
            catch (Exception ex)
            {
                DisplayListOutput(ex.Message);
            }

        }

        private void pullData()
        {
            try
            {
                ClearGrid();
                ShowStatusBar(string.Empty, true);

                var lstMachineInfo = manipulator.GetLogData(objZkeeper, 1);

                if (lstMachineInfo != null && lstMachineInfo.Count > 0)
                {
                    BindToGridView(lstMachineInfo);
                    ShowStatusBar(lstMachineInfo.Count + " registros encontrados.", true);
                }
                else
                {
                    DisplayListOutput("No se encontraron registros.");
                }
            }
            catch (Exception ex)
            {
                DisplayListOutput(ex.Message);
            }
        }

        private void btnPullData_Click(object sender, EventArgs e)
        {
            pullData();
        }


        private void ClearGrid()
        {
            if (dgvRecords.Controls.Count > 2)
            { dgvRecords.Controls.RemoveAt(2); }


            dgvRecords.DataSource = null;
            dgvRecords.Controls.Clear();
            dgvRecords.Rows.Clear();
            dgvRecords.Columns.Clear();
        }

        private void BindToGridView(object list)
        {
            ClearGrid();
            dgvRecords.DataSource = list;
            dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            UniversalStatic.ChangeGridProperties(dgvRecords);

            dgvRecords.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvRecords.AllowUserToResizeRows = true;
        }

        private void DisplayListOutput(string message)
        {
            if (dgvRecords.Controls.Count > 2)
            {
                dgvRecords.Controls.RemoveAt(2);
            }

            ShowStatusBar(message, false);
        }

        private void DisplayEmpty()
        {
            ClearGrid();
        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {
            UniversalStatic.DrawLineInFooter(pnlHeader, Color.FromArgb(204, 204, 204), 2);
        }

        private void btnPowerOff_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var resultDia = DialogResult.None;
            resultDia = MessageBox.Show("¿Se encuentra seguro de apagar esta terminal? Este proceso no puede ser revertido.", "Apagar terminal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultDia == DialogResult.Yes)
            {
                bool deviceOff = objZkeeper.PowerOffDevice(1);
                InsertarEvento("El usuario " + usuario + " ha APAGADO la terminal en " + marcadores[Convert.ToInt32(cmbUbicacion.SelectedIndex)].UBICACION);
            }

            this.Cursor = Cursors.Default;
        }

        private void btnRestartDevice_Click(object sender, EventArgs e)
        {
            DialogResult rslt = MessageBox.Show("¿Se encuentra seguro que desea reiniciar esta terminal?", "Reiniciar terminal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rslt == DialogResult.Yes)
            {
                if (objZkeeper.RestartDevice(1)) { 
                    ShowStatusBar("La terminal se encuentra reiniciando. Por favor, espere...", true);
                    InsertarEvento("El usuario " + usuario + " ha reiniciado la terminal " + marcadores[Convert.ToInt32(cmbUbicacion.SelectedIndex)].UBICACION);
                }
                else
                    ShowStatusBar("La operación ha fallado. Intentelo nuevamente.", false);
            }
        }

        private void btnGetDeviceTime_Click(object sender, EventArgs e)
        {
            int machineNumber = 1;
            int dwYear = 0;
            int dwMonth = 0;
            int dwDay = 0;
            int dwHour = 0;
            int dwMinute = 0;
            int dwSecond = 0;

            bool result = objZkeeper.GetDeviceTime(machineNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);

            string deviceTime = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond).ToString();
            List<DeviceTimeInfo> lstDeviceInfo = new List<DeviceTimeInfo>();
            lstDeviceInfo.Add(new DeviceTimeInfo() { DeviceTime = deviceTime });
            BindToGridView(lstDeviceInfo);
        }

        private void btnEnableDevice_Click(object sender, EventArgs e)
        {
            bool deviceEnabled = objZkeeper.EnableDevice(1, true);
            InsertarEvento("El usuario " + usuario + " ha debloqueado la terminal " + marcadores[Convert.ToInt32(cmbUbicacion.SelectedIndex)].UBICACION);
        }

        private void btnDisableDevice_Click(object sender, EventArgs e)
        {
            bool deviceDisabled = objZkeeper.DisableDeviceWithTimeOut(1, 3000);
            InsertarEvento("El usuario " + usuario + " ha desabilitado la terminal " + marcadores[Convert.ToInt32(cmbUbicacion.SelectedIndex)].UBICACION);
        }        

        private void cmbUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            conectar(cmbUbicacion.SelectedIndex);
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            Sincronizar( cmbUbicacion.SelectedIndex );
        }

        private void Sincronizar(int index)
        {
            this.Cursor = Cursors.WaitCursor;
            ToggleControls(false);

            var lstMachineInfo = manipulator.GetLogData(objZkeeper, 1);

            if (InsertarEvento("El usuario " + usuario + " ha iniciado la sincronización de la terminal " + marcadores[index].UBICACION))
            {
                try
                {
                    var total = 0;
                    var errores = 0;

                    if (lstMachineInfo != null && lstMachineInfo.Any())
                    {
                        BindToGridView(lstMachineInfo);
                        foreach (var item in lstMachineInfo)
                        {
                            if (InsertRecord(0, item.IndRegID, DateTime.Parse(item.DateTimeRecord), item.InOutModel, index))
                            {
                                total++;
                                ShowStatusBar("El proceso de sincronización a finalizado exitosamente.", true);
                            }
                            else
                            {
                                errores++;
                                ShowStatusBar("Error al insertar el código " + item.IndRegID, true);
                            }
                        }
                        ToggleControls(true);
                        this.Cursor = Cursors.Default;

                        InsertarEvento("El proceso de sincronización iniciado por " + usuario + " en la terminal '" + marcadores[index].UBICACION + "' ha finalizado. Registros insertados: " + total);
                        ShowStatusBar("El proceso de sincronización a finalizado exitosamente. Registros insertados: " + total, true);
                    }
                    else
                    {
                        DisplayListOutput("No se encontraron registros.");
                        this.Cursor = Cursors.WaitCursor;
                        ShowStatusBar("No se encuentra información para sincronizar.", true);
                    }
                }
                catch (Exception ex)
                {
                    ShowStatusBar(ex.Message, false);
                }
            }
        }

        public bool InsertRecord(int terminal, int codigo, DateTime fecha, int entradasalida, int index)
        {
            var success = false;
            string connectionString = "Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 192.168.1.14)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = FINANC)));User ID=TESTMARCA;Password=marcacion1004;";
            //var queryString = string.Format(@"INSERT INTO              TEST_MRC_MARCACION(MRC_CODCIA, MRC_FECHA, MRC_EVENTO, MRC_SITIO, MRC_CODCEL, MRC_FECHACREA) VALUES (:MRC_CODCIA, :MRC_FECHA, :MRC_EVENTO, :MRC_SITIO, :MRC_CODCEL, :MRC_FECHACREA )");
            var queryString = string.Format(@"INSERT INTO SISRRH.PLA_VMA_VALIDA_MARCACION(VMA_CODCIA, VMA_FECHA, VMA_EVENTO, VMA_SITIO, VMA_CODCEL, VMA_FECCAR) VALUES (:MRC_CODCIA, :MRC_FECHA, :MRC_EVENTO, :MRC_SITIO, :MRC_CODCEL, :MRC_FECHACREA )");
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Close();
                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = queryString;
                cmd.Parameters.Add(":MRC_CODCIA", "001");
                cmd.Parameters.Add(":MRC_FECHA", fecha);
                cmd.Parameters.Add(":MRC_EVENTO", (entradasalida == 0) ? "I" : "O");
                cmd.Parameters.Add(":MRC_SITIO", marcadores[Convert.ToInt32(cmbUbicacion.SelectedIndex)].Numero);
                cmd.Parameters.Add(":MRC_CODCEL", codigo.ToString().Trim());
                cmd.Parameters.Add(":MRC_FECHACREA", DateTime.Now);

                Console.WriteLine(">>> Ingresando información a la terminal: " + marcadores[Convert.ToInt32(cmbUbicacion.SelectedIndex)].UBICACION + " - Código: " + codigo.ToString().Trim());

                try
                {
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    con.Close();


                    con.Open();
                    OracleCommand cmd2 = con.CreateCommand();
                    cmd2.Parameters.Add(":MRC_CODCIA", "001");
                    cmd2.Parameters.Add(":MRC_FECHA", fecha);
                    cmd2.Parameters.Add(":MRC_EVENTO", (entradasalida == 0) ? "I" : "O");
                    cmd2.Parameters.Add(":MRC_SITIO", marcadores[Convert.ToInt32(cmbUbicacion.SelectedIndex)].Numero);
                    cmd2.Parameters.Add(":MRC_CODCEL", codigo.ToString().Trim());
                    //cmd2.Parameters.Add(":MRC_FECHACREA", DateTime.Now);

                    var queryString2 = string.Format(@"INSERT INTO SISRRH.PLA_MRC_MARCACION(MRC_CODCIA, MRC_FECHA, MRC_EVENTO, MRC_SITIO,  MRC_CODCEL) VALUES (:MRC_CODCIA, :MRC_FECHA, :MRC_EVENTO, :MRC_SITIO, :MRC_CODCEL )");
                    cmd2.CommandText = queryString2;

                    int rowsUpdated2 = cmd2.ExecuteNonQuery();
                    con.Close();

                    if (rowsUpdated > 0) success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(">>> Error al ingresar el código: " + codigo.ToString().Trim());
                    Console.WriteLine(">>> " + ex.Message);
                    con.Close();
                    return false;
                }

            }

            return success;
        }

        private void btn_eventos_Click(object sender, EventArgs e)
        {
            ObtenerEventos();
        }

        private void ObtenerEventos()
        {
            string connectionString = "Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 192.168.1.14)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = FINANC)));User ID=" + usuario + ";Password=" + contrasenya + ";";
            string query = "SELECT ID, FECHAEVENTO, LOG FROM TEMP_MRC_LOG ORDER BY FECHAEVENTO DESC FETCH FIRST 100 ROWS ONLY";

            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Close();
                con.Open();
                OracleCommand cmd = new OracleCommand(query, con);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                oda.Fill(ds);

                var list = ds.Tables[0].AsEnumerable()
                    .Select(dr => new LogModel
                    {
                        LOG = dr.Field<string>("LOG"),
                        FECHAEVENTO = dr.Field<DateTime>("FECHAEVENTO"),
                    }).ToList();

                BindToGridView(list);
            }
        }

        public void CargarMarcadores()
        {
            string connectionString = "Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 192.168.1.14)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = FINANC)));User ID=" + usuario + ";Password=" + contrasenya + ";";
            string query = "SELECT MAR_CODIGO NUMEROTERMINAL, MAR_UBICACION UBICACION, MAR_IP IP, MAR_PUERTO PUERTO FROM PLA_MAR_MARCADORES";

            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Close();
                con.Open();
                OracleCommand cmd = new OracleCommand(query, con);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                oda.Fill(ds);

                var list = ds.Tables[0].AsEnumerable()
                    .Select(dr => new UbicacionesModel
                  {
                      UBICACION = dr.Field<string>("UBICACION"),
                      IP = dr.Field<string>("IP"),
                      PUERTO = dr.Field<Int16>("PUERTO").ToString(),
                      Numero = Convert.ToInt32( dr.Field<Int64>("NUMEROTERMINAL")),
                    }).ToList();

                marcadores = list;
                cmbUbicacion.DataSource = list;

                /*var l = new List<UbicacionesModel>();
                l.Add(new UbicacionesModel()
                {
                    IP = "192.168.5.19",
                    PUERTO = "4370",
                    Numero = 1,
                    UBICACION = "Oficina ARBD"
                });
                marcadores = l;
                cmbUbicacion.DataSource = l;*/

                cmbUbicacion.DisplayMember = "UBICACION";
                cmbUbicacion.ValueMember = "IP";
            }
        }

        private static bool InsertarEvento(string evento)
        {
            var success = false;
            string connectionString = "Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 192.168.1.14)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = FINANC)));User ID=TESTMARCA;Password=marcacion1004;";
            var queryString = string.Format(@"INSERT INTO PLA_BIT_BITACORA_MARCACION(BIT_FECHA_EVENTO, BIT_LOG) VALUES (:FECHAEVENTO, :LOG )");

            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Close();
                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = queryString;
                cmd.Parameters.Add(":FECHAEVENTO", DateTime.Now);
                cmd.Parameters.Add(":LOG", evento);

                try
                {
                    Console.WriteLine(">>> Se ha escrito el evento");
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    con.Close();
                    if (rowsUpdated > 0) success = true;
                }
                catch
                {
                    Console.WriteLine("~> Error al ingresar evento");
                    con.Close();
                    return false;
                }
            }

            return success;
        }



        private void btnSyncAll_Click(object sender, EventArgs e)
        {
            DialogResult rslt = MessageBox.Show("¿Se encuentra seguro que desea sincronizar todas las terminales? Este proceso puede tardar pocos minutos en finalizar.", "Sincronización de terminales", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rslt == DialogResult.Yes)
            {
                InsertarEvento("El usuario " + usuario + " ha iniciado la sincronización automática de todos los marcadores.");

                for (var i = 0; i < marcadores.Count(); i++)
                {
                    conectar(i);
                    Sincronizar(i);
                }

                cmbUbicacion.SelectedIndex = 0;

                InsertarEvento("La sincronización automática de los marcadores ha sido finalizada.");
                ShowStatusBar("El proceso de sincronización de terminales ha finalizado.", true);
            } else
            {
                ShowStatusBar("El proceso de sincronización de terminales ha sido cancelado.", false);
            }
        }

        private void btnSincronizarHora_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < marcadores.Count(); i++)
            {
                conectar(i);
                objZkeeper.SetDeviceTime2(1, DateTime.Now);
                ShowStatusBar("La actualización de horas en las terminales se ha ejecutado exitosamente.", true);
            }

            MailAddress from = new MailAddress("marcaciones@lan.cel.gob.sv", "Marcaciones");
            MailAddress to = new MailAddress("nflores@lan.cel.gob.sv", "Nelson Flores");
            List<MailAddress> cc = new List<MailAddress>
            {
                new MailAddress("" + usuario + "@lan.cel.gob.sv", "Benjamín Rivera")
            };

            List<MailAddress> bcc = new List<MailAddress>
            {
                new MailAddress("csanchez@lan.cel.gob.sv", "Carlos Sánchez")
            };

            //SendEmail("Este es un correo enviado desde zimbra sin buzón", from, to, cc, bcc);
        }

        

        protected void SendEmail(string _subject, MailAddress _from, MailAddress _to, List<MailAddress> _cc, List<MailAddress> _bcc = null)
        {
            string Text = "";
            SmtpClient mailClient = new SmtpClient("zimbra.lan.cel.gob.sv")
            {
                Port = 25
            };
            MailMessage msgMail;
            Text = "Correo sin buzón de salida";
            msgMail = new MailMessage
            {
                From = _from
            };
            msgMail.To.Add(_to);
            foreach (MailAddress addr in _cc)
            {
                msgMail.CC.Add(addr);
            }
            
            if (_bcc != null)
            {
                foreach (MailAddress addr in _bcc)
                {
                    msgMail.Bcc.Add(addr);
                }
            }
            msgMail.Subject = _subject;
            msgMail.Body = Text;
            msgMail.IsBodyHtml = true;
            mailClient.Send(msgMail);
            msgMail.Dispose();
        }
    }
}
