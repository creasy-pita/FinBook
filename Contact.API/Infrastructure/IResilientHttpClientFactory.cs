using BuildingBlocks.Resilience.Http;
using System;

namespace Contact.API.Infrastructure
{
    public interface IResilientHttpClientFactory
    {
        ResilientHttpClient CreateResilientHttpClient();
    }
}