using Microsoft.Extensions.Azure;
using PatientManagementServices.Application.Common;
using PatientManagementServices.Application.Core;
using PatientManagementServices.Domain.Entities;
using PatientManagementServices.Domain.Interfaces.IRepository;
using PatientManagementServices.Services.Events;
using PatientManagementServices.Services.Interfaces;

namespace PatientManagementServices.Services
{
    /// <summary>
    /// Generic attribute service
    /// </summary>
    public partial class GenericAttributeService : IGenericAttributeService
    {
        #region Fields

        //private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IGenericAttributeRepository _genericAttributeRepository;

        #endregion

        #region Ctor

        public GenericAttributeService(IGenericAttributeRepository genericAttributeRepository, ILogger logger)
        {
           // _eventPublisher = eventPublisher;
            _genericAttributeRepository = genericAttributeRepository;
            _logger = logger;   
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes an attribute
        /// </summary>
        /// <param name="attribute">Attribute</param>
        public async Task DeleteAttribute(int attribute)
        {
            if (attribute == 0)
                throw new ArgumentNullException(nameof(attribute));

           await _genericAttributeRepository.DeleteAsync(attribute);

            //event notification
            //_eventPublisher.EntityDeleted(attribute);
        }
        /// <summary>
        /// Deletes an attribute
        /// </summary>
        /// <param name="attribute">Attribute</param>
        public async Task DeleteAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            await _genericAttributeRepository.DeleteAsync(attribute);

            //event notification
            //_eventPublisher.EntityDeleted(attribute);
        }
        /// <summary>
        /// Deletes an attributes
        /// </summary>
        /// <param name="attributes">Attributes</param>
        public async Task DeleteAttributes(IList<GenericAttribute> attributes)
        {
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes));

           await _genericAttributeRepository.DeleteAsync(attributes);

            //event notification
            //foreach (var attribute in attributes)
            //{
            //    _eventPublisher.EntityDeleted(attribute);
            //}
        }

        /// <summary>
        /// Gets an attribute
        /// </summary>
        /// <param name="attributeId">Attribute identifier</param>
        /// <returns>An attribute</returns>
        public  async Task<GenericAttribute> GetAttributeById(int attributeId)
        {
            if (attributeId == 0)
                return null;

            return await _genericAttributeRepository.GetByIdAsync(attributeId);
        }

        /// <summary>
        /// Inserts an attribute
        /// </summary>
        /// <param name="attribute">attribute</param>
        public async Task InsertAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            attribute.CreatedOrUpdatedDateUTC = DateTime.UtcNow;
           await _genericAttributeRepository.InsertAsync(attribute);

            //event notification
           // _eventPublisher.EntityInserted(attribute);
        }

        /// <summary>
        /// Updates the attribute
        /// </summary>
        /// <param name="attribute">Attribute</param>
        public async Task UpdateAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            attribute.CreatedOrUpdatedDateUTC = DateTime.UtcNow;
           await _genericAttributeRepository.UpdateAsync(attribute);

            //event notification
            //_eventPublisher.EntityUpdated(attribute);
        }

        /// <summary>
        /// Get attributes
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="keyGroup">Key group</param>
        /// <returns>Get attributes</returns>
        public async Task<IList<GenericAttribute>> GetAttributesForEntity(int entityId, string keyGroup)
        {
            //we cannot inject ICacheKeyService into constructor because it'll cause circular references.
            //that's why we resolve it here this way

            var query = from ga in _genericAttributeRepository.Queryable()
                        where ga.EntityId == entityId &&
                              ga.KeyGroup == keyGroup
                        select ga;
            //var attributes = query.ToCachedList(key);

            return query.ToList();
        }

        /// <summary>
        /// Save attribute value
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="Tenant">Tanent identifier; pass 0 if this attribute will be available for all stores</param>
        /// Task IGenericAttributeService.SaveAttribute<TPropType>(BaseEntity entity, string key, TPropType value, int storeId)
        public async Task SaveAttribute<TPropType>(BaseEntity entity, string key, TPropType value, int tenantId = 0)
        {
            //if (!String.IsNullOrEmpty(details.Gender))
            //    _genericAttributeService.SaveAttribute<string>(currentCustomer, NopCustomerDefaults.GenderAttribute, details.Gender);
            //if (!String.IsNullOrEmpty(details.PhoneNumber))
            //    _genericAttributeService.SaveAttribute<string>(currentCustomer, NopCustomerDefaults.PhoneAttribute, details.PhoneNumber);
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var keyGroup = entity.GetType().Name;

            var props = _genericAttributeRepository.Queryable()
                .Where(x => x.TenantId == tenantId)
                .ToList();
            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            var valueStr = CommonHelper.To<string>(value);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(valueStr))
                {
                    //delete
                   await DeleteAttribute(prop);
                }
                else
                {
                    //update
                    prop.Value = valueStr;
                   await UpdateAttribute(prop);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(valueStr))
                    return;

                //insert
                prop = new GenericAttribute
                {
                    EntityId = entity.Id,
                    Key = key,
                    KeyGroup = keyGroup,
                    Value = valueStr,
                    TenantId = tenantId
                };

               await InsertAttribute(prop);
            }
        }

        /// <summary>
        /// Get an attribute of an entity
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="storeId">Load a value specific for a certain store; pass 0 to load a value shared for all stores</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Attribute</returns>
        public async Task<TPropType> GetAttribute<TPropType>(BaseEntity entity, string key, int tenantId = 0, TPropType defaultValue = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var keyGroup = entity.GetType().Name;

            var props = from ga in _genericAttributeRepository.Queryable()
                        where ga.EntityId == entity.Id &&
                              ga.KeyGroup == keyGroup
                        select ga;

            //little hack here (only for unit testing). we should write expect-return rules in unit tests for such cases
            if (props == null)
                return defaultValue;

            props = (IQueryable<GenericAttribute>)props.Where(x => x.TenantId == tenantId).ToList();
            if (!props.Any())
                return defaultValue;

            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            if (prop == null || string.IsNullOrEmpty(prop.Value))
                return defaultValue;

            return CommonHelper.To<TPropType>(prop.Value);
        }

        /// <summary>
        /// Get an attribute of an entity
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="key">Key</param>
        /// <param name="storeId">Load a value specific for a certain store; pass 0 to load a value shared for all stores</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Attribute</returns>
        public async Task<TPropType> GetAttribute<TEntity, TPropType>(int entityId, string key, int tenantId = 0, TPropType defaultValue = default)
            where TEntity : BaseEntity
        {
            var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            entity.Id = entityId;

            return await GetAttribute(entity, key, tenantId, defaultValue);
        }

        #endregion
    }
}
