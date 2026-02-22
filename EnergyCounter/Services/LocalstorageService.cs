

using Microsoft.Extensions.Primitives;
using Microsoft.JSInterop;
using Microsoft.Net.Http.Headers;

class LocalStorageService(IJSRuntime js)
{
    public async Task<string?> Get(string key)
    {
        string val = await js.InvokeAsync<string?>("localStorage.getItem", key);
        if (val is null || string.IsNullOrEmpty(val))
        {
            return null;
        }

        return val;
    }

    public async Task Set(string key, string value)
    {
        await js.InvokeAsync<string>("localStorage.setItem", key, value);
    }
}