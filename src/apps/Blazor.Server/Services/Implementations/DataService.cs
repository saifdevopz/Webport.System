using Blazor.Server.Common.HttpClients;
using Common.Domain.Errors;
using Common.Domain.Results;


namespace Blazor.Server.Services.Implementations;

public class DataService(BaseHttpClient BaseHttpClient, TenantHttpClient TenantHttpClient)
{
    private readonly BaseHttpClient _baseHttpClient = BaseHttpClient;
    private readonly TenantHttpClient _tenantHttpClient = TenantHttpClient;

    private HttpClient GetClient(bool useBaseClient)
    {
        return useBaseClient
            ? _baseHttpClient.GetPrivateHttpClient()
            : _tenantHttpClient.GetPrivateHttpClient();
    }

    public async Task<Result<T>> GetAllAsync<T>(string source, bool useBaseClient = false)
    {
        try
        {
            HttpClient client = GetClient(useBaseClient);
            var response = await client.GetFromJsonAsync<Result<T>>(source);

            if (response == null)
                return Result.Failure<T>(CustomError.Conflict(",", "No response from server."));

            if (!response.IsSuccess)
                return Result.Failure<T>(CustomError.Conflict(",", "No response from server."));

            return Result.Success(response.Data);
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<T>(CustomError.Conflict(",", $"{ex.Message}"));
        }
    }

    public async Task<Result<T>> GetByIdAsync<T>(string basePath, string id)
    {
        try
        {
            HttpClient client = GetClient(true);
            var response = await client.GetFromJsonAsync<Result<T>>($"{basePath}/{id}");

            if (response == null)
                return Result.Failure<T>(CustomError.Conflict(",", "No response from server."));

            if (!response.IsSuccess)
                return Result.Failure<T>(CustomError.Conflict(",", "No response from server."));

            return Result.Success(response.Data);
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<T>(CustomError.Conflict(",", $"{ex.Message}"));
        }
    }

    public async Task<Result> PostAsync<T>(string source, T obj)
    {
        try
        {
            HttpClient client = GetClient(true);
            var response = await client.PostAsJsonAsync(source, obj);

            var result = await response.Content.ReadFromJsonAsync<Result>();

            if (response == null)
                return Result.Failure<T>(CustomError.Conflict(",", "No response from server."));

            if (result!.IsFailure)
                return Result.Failure<T>(CustomError.Conflict(",", "No response from server."));

            return result.IsSuccess
                ? Result.Success()
                : Result.Failure<T>(CustomError.Conflict(",", "No response from server."));
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<T>(CustomError.Conflict(",", $"{ex.Message}"));
        }
    }

    public async Task<Result> PutAsync<T>(string source, T obj)
    {
        try
        {
            HttpClient client = GetClient(true);
            var response = await client.PutAsJsonAsync(source, obj);

            var result = response.Content.ReadFromJsonAsync<Result>();

            if (response == null)
                return Result.Failure<T>(CustomError.Conflict(",", "No response from server."));

            if (result.IsFaulted)
                return Result.Failure<T>(CustomError.Conflict(",", "No response from server."));

            return response.IsSuccessStatusCode
                ? Result.Success()
                : Result.Failure(CustomError.Conflict(",", "No response from server."));
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<T>(CustomError.Conflict(",", $"{ex.Message}"));
        }
    }


    public async Task<Result> DeleteByIdAsync(string source, int id)
    {
        try
        {
            var deleteUri = new Uri($"{source}/{id}", UriKind.Relative);

            HttpClient client = GetClient(true);
            var response = await client.DeleteAsync(deleteUri);

            if (response == null)
                return Result.Failure(CustomError.Conflict(",", "No response from server."));

            return response.IsSuccessStatusCode ? Result.Success()
                : Result.Failure(CustomError.Conflict(",", "No response from server."));
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure(CustomError.Conflict(",", $"{ex.Message}"));
        }
    }
}
