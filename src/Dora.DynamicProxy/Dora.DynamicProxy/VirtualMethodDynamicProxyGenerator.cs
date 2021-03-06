﻿using System;

namespace Dora.DynamicProxy
{
    /// <summary>
    /// Virtual method interception based type dynamic proxy generator.
    /// </summary>                                                          
    public class VirtualMethodDynamicProxyGenerator : ITypeDynamicProxyGenerator
    {
        /// <summary>
        /// Determines whether this specified type can be intercepted.
        /// </summary>
        /// <param name="type">The type to intercept.</param>
        /// <returns>
        ///   <c>true</c> if the specified type can be intercepted; otherwise, <c>false</c>.
        /// </returns>
        public bool CanIntercept(Type type)
        {
            return !Guard.ArgumentNotNull(type, nameof(type)).IsSealed;
        }

        /// <summary>
        /// Creates a new interceptable dynamic proxy.
        /// </summary>
        /// <param name="type">The type to intercept.</param>
        /// <param name="interceptors">The <see cref="InterceptorDecoration" /> representing the type members decorated with interceptors.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider" /> helping the creating object.</param>
        /// <returns>
        /// The interceptable dynamic proxy.
        /// </returns>
        public object Create(Type type, InterceptorDecoration interceptors, IServiceProvider serviceProvider)
        {
            Guard.ArgumentNotNull(type, nameof(type));
            Guard.ArgumentNotNull(interceptors, nameof(interceptors));
            Guard.ArgumentNotNull(serviceProvider, nameof(serviceProvider));
            if (this.CanIntercept(type) && !interceptors.IsEmpty)
            {
                var factory = DynamicProxyFactoryCache.Instance.GetTypeFactory(type, interceptors);
                return factory(interceptors, serviceProvider);
            }
            return serviceProvider.GetService(type);
        }
    }
} 