using System.Text.Json;
using NuGet.Protocol;

namespace BookstoreApi.Models;

public class ErrorDetails
{
    public int  StatusCode { get; set; }
    public string? Message { get; set; }
    

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
   
}