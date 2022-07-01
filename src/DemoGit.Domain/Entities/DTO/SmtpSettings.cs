using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoGit.Domain.Entities.DTO
{
    public class SmtpSettings
    {
        public string? Host { get; set; }
        public string? Port { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
    }
}