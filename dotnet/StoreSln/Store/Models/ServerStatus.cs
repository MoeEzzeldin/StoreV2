using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Store.Models
{
    public class ServerStatus
    {
        // Basic status
        public bool IsOnline { get; set; } = true;
        public string Message { get; set; } = "Server is online";
        public DateTime StartTime { get; set; }
        public TimeSpan Uptime => DateTime.UtcNow - StartTime;
        
        // System information
        public string? HostName { get; set; }
        public string? OSVersion { get; set; }
        public string? RuntimeVersion { get; set; }
        public int ProcessorCount { get; set; }
        
        // Application information
        public string? ApplicationName { get; set; }
        public string? Version { get; set; }
        public string? Environment { get; set; }
        
        // Database information
        public bool DatabaseConnected { get; set; }
        
        // Endpoints information
        public List<string>? Endpoints { get; set; }
    }
}
