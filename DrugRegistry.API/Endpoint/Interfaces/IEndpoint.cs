﻿namespace DrugRegistry.API.Endpoint.Interfaces;

public interface IEndpoint
{
    public IServiceCollection RegisterServices(IServiceCollection collection);
    public WebApplication MapEndpoints(WebApplication app);
}