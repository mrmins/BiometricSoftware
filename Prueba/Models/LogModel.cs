using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace BioMetrixCore.Models
{
    public class LogModel
    {
        [System.ComponentModel.DisplayName("Evento")]
        public string LOG { get; set; }

        [System.ComponentModel.Browsable(false)]
        public DateTime FECHAEVENTO { get; set; }

        [System.ComponentModel.DisplayName("Fecha del evento")]
        public string TimeOnlyRecord
        {
            get { return FECHAEVENTO.ToString("yyyy-MM-dd hh:mm:ss tt"); }
        }
    }
}
