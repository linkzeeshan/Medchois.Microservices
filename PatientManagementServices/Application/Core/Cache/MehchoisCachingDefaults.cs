﻿namespace PatientManagementServices.Application.Core.Cache
{
    /// <summary>
    /// Represents default values related to caching
    /// </summary>
    public static partial class MehchoisCachingDefaults
    {
        /// <summary>
        /// Gets the default cache time in minutes
        /// </summary>
        public static int CacheTime => 60;

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : Entity type name
        /// {1} : Entity id
        /// </remarks>
        public static string MedchoisEntityCacheKey => "Medchois.{0}.id-{1}";
    }
}
