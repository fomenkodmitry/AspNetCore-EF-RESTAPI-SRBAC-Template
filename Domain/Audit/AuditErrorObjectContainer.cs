using System;

namespace Domain.Audit
{
    public class AuditErrorObjectContainer
    {
        public Exception Error { get; set; }
        public object Model { get; set; }
    }
}