namespace EDI.Models.ErrorModels
{
    public class ErrorModel
    {
        public string IpAddress { get; set; }
        public string UserId { get; set; }
        public string ClassName { get; set; }
        public string FunctionName { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string Resolved { get; set; }
    }
}
