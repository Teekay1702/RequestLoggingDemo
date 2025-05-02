namespace RequestLoggingDemo.Models
{
    public class RequestLog
    {
        public int Id { get; set; }
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public string Headers { get; set; }
        public string IpAddress { get; set; }
        public int StatusCode { get; set; }
        public string ResponseTime { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
