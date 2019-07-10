namespace BioMetrixCore
{
    internal class UserIDInfo
    {
        [System.ComponentModel.DisplayName("Número de terminal")]
        public int MachineNumber { get; set; }
        public int EnrollNumber { get; set; }
        public int BackUpNumber { get; set; }
        public int Privelage { get; set; }

        [System.ComponentModel.DisplayName("Activo")]
        public int Enabled { get; set; }

    }
}
