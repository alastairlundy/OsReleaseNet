/*
    OsReleaseNet
    Copyright 2020-2025 Alastair Lundy

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
 */

using System;
using Microsoft.Extensions.DependencyInjection;
using OsReleaseNet.Abstractions;
using OsReleaseNet.Abstractions.Parsers;
using OsReleaseNet.Parsers;

namespace OsReleaseNet.Extensions.DependencyInjection;

/// <summary>
/// Facilitates the integration of OsReleaseNet into the dependency injection container,
/// allowing it to be easily accessed throughout the application.
/// </summary>
public static class OsReleaseNetServiceRegistration
{
    /// <summary>
    /// Facilitates the integration of OsReleaseNet into the dependency injection container,
    /// allowing it to be easily accessed throughout the application.
    /// </summary>
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers the necessary services for OsReleaseNet functionality.
        /// </summary>
        /// <param name="serviceLifetime">The lifetime of the service instance.</param>
        /// <returns>The updated IServiceCollection with registered services.</returns>
        /// <exception cref="NotSupportedException">Thrown when an unsupported service lifetime is provided.</exception>
        public IServiceCollection AddOsReleaseNet(ServiceLifetime serviceLifetime)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped<ILinuxOsReleaseParser, LinuxOsReleaseParser>();
                    services.AddScoped<IFreeBsdOsReleaseParser, FreeBsdOsReleaseParser>();
                    services.AddScoped<ILinuxOsReleaseProvider, LinuxOsReleaseProvider>();
                    services.AddScoped<IFreeBsdOsReleaseProvider, FreeBsdOsReleaseProvider>();
                    services.AddScoped<ISteamOsInfoProvider, SteamOsInfoProvider>();
                    break;
                case ServiceLifetime.Singleton:
                    services.AddSingleton<ILinuxOsReleaseParser, LinuxOsReleaseParser>();
                    services.AddSingleton<IFreeBsdOsReleaseParser, FreeBsdOsReleaseParser>();
                    services.AddSingleton<ILinuxOsReleaseProvider, LinuxOsReleaseProvider>();
                    services.AddSingleton<IFreeBsdOsReleaseProvider, FreeBsdOsReleaseProvider>();
                    services.AddSingleton<ISteamOsInfoProvider, SteamOsInfoProvider>();
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient<ILinuxOsReleaseParser, LinuxOsReleaseParser>();
                    services.AddTransient<IFreeBsdOsReleaseParser, FreeBsdOsReleaseParser>();
                    services.AddTransient<ILinuxOsReleaseProvider, LinuxOsReleaseProvider>();
                    services.AddTransient<IFreeBsdOsReleaseProvider, FreeBsdOsReleaseProvider>();
                    services.AddTransient<ISteamOsInfoProvider, SteamOsInfoProvider>();
                    break;
                default:
                    throw new NotSupportedException("The service lifetime provided is not supported");
            }
            
            return services;
        }
    }
}