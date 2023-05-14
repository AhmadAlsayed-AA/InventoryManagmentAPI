using System;
namespace Warehouse.Services.Helpers.Validation
{
    public class ValidationResponse
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
    }
}

