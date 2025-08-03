﻿using Blazor.WebportSystem.Common.HttpClients;
using Common.Domain.Errors;
using Common.Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace Blazor.WebportSystem.Common.Services.Implementations;

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

    public async Task<Result<T>> GetByIdAsync<T>(string basePath, string id, bool useBaseClient = false)
    {
        try
        {
            HttpClient client = GetClient(useBaseClient);
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

    public async Task<Result> PostAsync<T>(string source, T obj, bool useBaseClient = false)
    {
        try
        {
            HttpClient client = GetClient(useBaseClient);
            var response = await client.PostAsJsonAsync(source, obj);

            if (response == null)
                return Result.Failure<T>(CustomError.Conflict("NETWORK", "No response from server."));

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Result>();

                if (result == null || result.IsFailure)
                    return Result.Failure<T>(CustomError.Conflict("RESPONSE", "Unexpected empty or failed result."));

                return Result.Success(result);
            }

            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            if (problem != null)
            {
                return Result.Failure<T>(CustomError.Failure(problem.Title!, problem.Detail!));
            }

            return Result.Failure<T>(CustomError.Conflict("HTTP", $"Unexpected error: {response.StatusCode}"));
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
