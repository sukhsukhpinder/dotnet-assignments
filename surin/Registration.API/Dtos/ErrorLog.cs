namespace Registration.API.Dtos
{
    //TODO:: Create customized table for serilog 
    public class ErrorLog
    {
        public int ID { get; set; }
        public int? LoginUser { get; set; }
        public string? PageName { get; set; }
        public string? LogAction { get; set; }
        public string? ErrorMessage { get; set; }
        public string? MachineAddress { get; set; }
        public string? Browser { get; set; }
        public DateTime? CreatedDateTime { get; set; }

    }
}
