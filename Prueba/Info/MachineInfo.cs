using System;

namespace BioMetrixCore
{
    public class MachineInfo
    {
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DisplayName("Terminal")]
        public int MachineNumber { get; set; }

        [System.ComponentModel.DisplayName("Carnet")]
        public int IndRegID { get; set; }

        [System.ComponentModel.DisplayName("Fecha/Hora")]
        public string DateTimeRecord { get; set; }

        //public DateTime DateOnlyRecord
        //{
        //    get { return DateTime.Parse(DateTime.Parse(DateTimeRecord).ToString("yyyy-MM-dd")); }
        //}
        //public DateTime TimeOnlyRecord
        //{
        //    get { return DateTime.Parse(DateTime.Parse(DateTimeRecord).ToString("hh:mm:ss tt")); }
        //}

        [System.ComponentModel.Browsable(false)]

        public int InOutModel { get; set; }

        [System.ComponentModel.DisplayName("Tipo de marcación")]
        public string ModoMarcacion
        {
            get { return (InOutModel == 0) ? "Entrada" : "Salida"; }
        }

    }
}
