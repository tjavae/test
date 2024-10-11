namespace CUDC.Common.Dtos
{
    public class NLogDto
    {
        public string MachineName { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
    }
}
