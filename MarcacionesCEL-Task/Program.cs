using BioMetrixCore;
using BioMetrixCore.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MarcacionesCEL_Task
{
    class Program
    {
        static DeviceManipulator manipulator = new DeviceManipulator();
        public static ZkemClient objZkeeper;
        private static List<UbicacionesModel> marcadores = new List<UbicacionesModel>();

        static void Main(string[] args)
        {
            CargarMarcadores();
            for (var i = 0; i < marcadores.Count(); i++)
            {
                conectar(i);
                Sincronizar(i);
            }
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
            }
            catch (Exception ex)
            {
            }
        }

        private static void Sincronizar(int index)
        {
            var lstMachineInfo = manipulator.GetLogData(objZkeeper, 1);

            if (InsertarEvento("El usuario BERIVERA ha iniciado la sincronización de la terminal " + marcadores[index].UBICACION))
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

                        InsertarEvento("El proceso de sincronización iniciado por BERIVERA en la terminal '" + marcadores[index].UBICACION + "' ha finalizado. Registros insertados: " + total);
                        //ShowStatusBar("El proceso de sincronización a finalizado exitosamente. Registros insertados: " + total, true);
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
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
            string connectionString = "Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 192.168.1.14)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = FINANC)));User ID=berivera;Password=papaya2;";
            string query = "SELECT NUMEROTERMINAL, UBICACION, IP, PUERTO FROM TEMP_MRC_MARCADORES";

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
            }
        }

        private static bool InsertarEvento(string evento)
        {
            var success = false;
            string connectionString = "Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 192.168.1.14)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = FINANC)));User ID=berivera;Password=papaya2;";
            var queryString = string.Format(@"INSERT INTO TEMP_MRC_LOG(FECHAEVENTO, LOG) VALUES (:FECHAEVENTO, :LOG )");

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
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    con.Close();
                    if (rowsUpdated > 0) success = true;
                }
                catch
                {
                    con.Close();
                    return false;
                }
            }

            return success;
        }

        public static bool InsertRecord(int terminal, int codigo, DateTime fecha, int entradasalida, int index)
        {
            var success = false;
            string connectionString = "Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 192.168.1.14)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = FINANC)));User ID=berivera;Password=papaya2;";
            var queryString = string.Format(@"INSERT INTO TEST_MRC_MARCACION(MRC_CODCIA, MRC_FECHA, MRC_EVENTO, MRC_SITIO, MRC_CODCEL, MRC_FECHACREA) VALUES (:MRC_CODCIA, :MRC_FECHA, :MRC_EVENTO, :MRC_SITIO, :MRC_CODCEL, :MRC_FECHACREA )");

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

                Console.WriteLine(">>> Ingresando información a la terminal: " + marcadores[index].UBICACION);

                try
                {
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    con.Close();
                    if (rowsUpdated > 0) success = true;
                }
                catch (Exception ex)
                {
                    con.Close();
                    return false;
                }

            }

            return success;
        }
    }
}
