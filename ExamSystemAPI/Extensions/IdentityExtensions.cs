using System.Text.Json;

namespace Microsoft.AspNetCore.Identity
{
    public static class IdentityExtensions
    {
        public static async Task CheckAsync(this Task<IdentityResult> task) { 
            var r  = await task;
            if (!r.Succeeded)
                throw new InvalidOperationException(JsonSerializer.Serialize(r.Errors));
        }
    }
}
