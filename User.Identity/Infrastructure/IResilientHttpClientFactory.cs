using BuildingBlocks.Resilience.Http;
using System;

namespace User.Identity.Infrastructure
{
    public interface IResilientHttpClientFactory
    {
        ResilientHttpClient CreateResilientHttpClient();
    }
}