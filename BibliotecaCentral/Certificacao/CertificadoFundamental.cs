namespace BibliotecaCentral.Certificacao
{
    public struct CertificadoFundamental
    {
        public string Subject { get; set; }
        public string SerialNumber { get; set; }

        public CertificadoFundamental(string subject, string serial)
        {
            Subject = subject;
            SerialNumber = serial;
        }
    }
}
