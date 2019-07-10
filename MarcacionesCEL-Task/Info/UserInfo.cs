namespace BioMetrixCore
{
    internal class UserInfo
    {
        [System.ComponentModel.DisplayName("Terminal")]
        public int MachineNumber { get; set; }

        [System.ComponentModel.DisplayName("Carnet de empleado")]
        public string EnrollNumber { get; set; }

        [System.ComponentModel.DisplayName("Nombre")]
        public string Name { get; set; }

        [System.ComponentModel.DisplayName("Id de huella")]
        public int FingerIndex { get; set; }

        //[System.ComponentModel.DisplayName("Metadata")]
        //public string TmpData { get; set; }

        [System.ComponentModel.DisplayName("Privilegio")]
        public int Privelage { get; set; }

        [System.ComponentModel.DisplayName("Contraseña")]
        public string Password { get; set; }

        [System.ComponentModel.DisplayName("Habilitado")]
        public bool Enabled { get; set; }

        //[System.ComponentModel.DisplayName("")]
        //public string iFlag { get; set; }

    }
}
