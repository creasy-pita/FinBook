using BuildingBlocks.Resilience.Http;
using System;

namespace Recommend.Infrastructure
{
    public interface IResilientHttpClientFactory
    {
        ResilientHttpClient CreateResilientHttpClient();
    }
}