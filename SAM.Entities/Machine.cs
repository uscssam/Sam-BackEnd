namespace SAM.Entities
{
    public class Machine
    {
        public int IdMachine { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Status Ativa ou Inativa
        /// </summary>
        public bool Status { get; set; }
        public DateTime LastMaintenance { get; set; }
        public DateTime Preventive { get; set; }


    }
}
