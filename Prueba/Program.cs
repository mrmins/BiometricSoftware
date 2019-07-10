using BioMetrixCore;
using BioMetrixCore.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using zkemkeeper;
using Microsoft.Win32;

namespace Prueba
{
    class Program
    {
        static DeviceManipulator manipulator = new DeviceManipulator();
        public static ZkemClient objZkeeper;
        private static List<UbicacionesModel> marcadores = new List<UbicacionesModel>();

        private static void Get45or451FromRegistry()
        {
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
                int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                if (true)
                {
                    Console.WriteLine("Ejecutando NET FRAMEWORK: " + CheckFor45DotVersion(releaseKey)  + " con release key: " + releaseKey);
                }
            }
        }

        private static string CheckFor45DotVersion(int releaseKey)
        {
            if (releaseKey >= 461808)
            {
                return "4.7.2 or later";
            }
            if (releaseKey >= 461308)
            {
                return "4.7.1 or later";
            }
            if (releaseKey >= 460798)
            {
                return "4.7 or later";
            }
            if (releaseKey >= 394802)
            {
                return "4.6.2 or later";
            }
            if (releaseKey >= 394254)
            {
                return "4.6.1 or later";
            }
            if (releaseKey >= 393295)
            {
                return "4.6 or later";
            }
            if (releaseKey >= 393273)
            {
                return "4.6 RC or later";
            }
            if ((releaseKey >= 379893))
            {
                return "4.5.2 or later";
            }
            if ((releaseKey >= 378675))
            {
                return "4.5.1 or later";
            }
            if ((releaseKey >= 378389))
            {
                return "4.5 or later";
            }
            // This line should never execute. A non-null release key should mean 
            // that 4.5 or later is installed. 
            return "No 4.5 or later version detected";
        }

        static void Main(string[] args)
        {
            Get45or451FromRegistry();
            CargarMarcadores();
            for (var i = 0; i < marcadores.Count(); i++)
            {
                conectar(i);
                Sincronizar(i);
            }
            Sincronizacion();
        }

        private static void conectar(int index)
        {
            try
            {
                string ipAddress = marcadores[index].IP; //cmbUbicacion.SelectedValue.ToString(); //tbxDeviceIP.Text.Trim();
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
                Console.WriteLine("Se ha conectado a la terminal: " + deviceInfo + " - en ubicación: " + marcadores[index].UBICACION );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! " + ex.Message);
            }
        }

        private static void Sincronizar(int index)
        {
            var lstMachineInfo = manipulator.GetLogData(objZkeeper, 1);
            Console.WriteLine("Machine info nula? :" + (lstMachineInfo == null));
            Console.WriteLine("Total de eventos a cargar :" + lstMachineInfo.Count());

            if (InsertarEvento("El usuario TESTMARCA ha iniciado la sincronización de la terminal " + marcadores[index].UBICACION))
            {
                try
                {
                    var total = 0;
                    var errores = 0;

                    if (lstMachineInfo != null && lstMachineInfo.Any())
                    {
                        foreach (var item in lstMachineInfo)
                        {
                            if (InsertRecord(0, item.IndRegID, DateTime.Parse(item.DateTimeRecord), item.InOutModel, index))
                            {
                                total++;
                            }
                            else
                            {
                                errores++;
                            }
                        }

                        InsertarEvento("El proceso de sincronización iniciado por TESTMARCA en la terminal '" + marcadores[index].UBICACION + "' ha finalizado. Registros insertados: " + total);
                        //ShowStatusBar("El proceso de sincronización a finalizado exitosamente. Registros insertados: " + total, true);
                    }
                    else
                    {
                        Console.WriteLine("~> No se extrajeron resultados.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("~> " + ex.Message);
                }
            }
        }

        private static void RaiseDeviceEvent(object sender, string actionType)
        {
            switch (actionType)
            {
                case UniversalStatic.acx_Disconnect:
                    {
                        break;
                    }
                default:
                    break;
            }

        }

        public static void CargarMarcadores()
        {
            string connectionString = "Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 192.168.1.14)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = FINANC)));User ID=TESTMARCA;Password=marcacion1004;";
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
                        Numero = Convert.ToInt32(dr.Field<Int64>("NUMEROTERMINAL")),
                    }).ToList();

                marcadores = list;
                Console.WriteLine("Marcadores cargados: " + list.Count());
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

        public static bool InsertRecord(int terminal, int codigo, DateTime fecha, int entradasalida, int index)
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
                cmd.Parameters.Add(":MRC_SITIO", marcadores[index].Numero);
                cmd.Parameters.Add(":MRC_CODCEL", codigo.ToString().Trim());
                cmd.Parameters.Add(":MRC_FECHACREA", DateTime.Now);

                Console.WriteLine(">>> Ingresando información a la terminal: " + marcadores[index].UBICACION + " - Código: " + codigo.ToString().Trim());

                try
                {
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    con.Close();


                    con.Open();
                    OracleCommand cmd2 = con.CreateCommand();                    
                    cmd2.Parameters.Add(":MRC_CODCIA", "001");
                    cmd2.Parameters.Add(":MRC_FECHA", fecha);
                    cmd2.Parameters.Add(":MRC_EVENTO", (entradasalida == 0) ? "I" : "O");
                    cmd2.Parameters.Add(":MRC_SITIO", marcadores[index].Numero);
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

        public static void Sincronizacion()
        {
            MailAddress from = new MailAddress("marcaciones@lan.cel.gob.sv", "Marcaciones");
            MailAddress to = new MailAddress("jmelendez@cel.gob.sv", "Jaime Meléndez");
            List<MailAddress> cc = new List<MailAddress>
            {
                new MailAddress("berivera@cel.gob.sv", "Benjamín Rivera")
            };

            List<MailAddress> bcc = new List<MailAddress>
            {
                new MailAddress("berivera@cel.gob.sv", "Benjamín Rivera")
            };

            SendEmail("Las terminales han sido actualizadas exitosamente.", from, to, cc, bcc);
        }

        protected static void SendEmail(string _subject, MailAddress _from, MailAddress _to, List<MailAddress> _cc, List<MailAddress> _bcc = null)
        {
            string Text = "";
            SmtpClient mailClient = new SmtpClient("zimbra.lan.cel.gob.sv")
            {
                Port = 25
            };
            MailMessage msgMail;
            Text = "Las terminales han sido sincronizadas exitosamente.";
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

