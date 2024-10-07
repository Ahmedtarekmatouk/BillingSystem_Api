using Billing.DTO.TypeDTO;
using Billing.Models;
using Billing.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Type = Billing.Models.Type;

namespace Billing.Services
{
    public class TypeService : ITypeService
    {
        private readonly ITypeRepository _typeRepository;
        BillingContext billingContext;
        public TypeService(ITypeRepository typeRepository,BillingContext billingContext)
        {
            _typeRepository = typeRepository;
            this.billingContext = billingContext;
        }

        // Return all types
        public List<TypeDTO> GetAll()
        {
            List<Type> type= _typeRepository.GetAll();
            List<TypeDTO> typeDTO = new List<TypeDTO>();
            foreach(Type Item in type)
            {
                TypeDTO typ1 = new TypeDTO()
                {
                    id = Item.Id,
                    name=Item.Name,
                    notes=Item.Notes,
                    CompanyId=Item.CompanyId
                };
                typeDTO.Add(typ1);
            }
            return typeDTO;
        }

        // Get a type by its ID
        public TypeDTO GetById(int id)
        {
            
              var data=  _typeRepository.GetById(id);
            TypeDTO typedto = new TypeDTO()
            {
                id = data.Id,
                name=data.Name,
                notes=data.Notes,
                CompanyId=data.CompanyId
            };
            return typedto;
        }

        // Create a new type after checking for uniqueness
        public TypeDTO Create(TypeDTO entity)
        {
            
            var newType = new Type
            {
                Id =entity.id,
                Name = entity.name,
                CompanyId = entity.CompanyId,
                Notes = entity.notes,
            };
            _typeRepository.Add(newType);
            return entity;
          
        }

        // Update an existing type
        public TypeDTO Update(TypeDTO entity)
        {
            Type type = new Type()
            {
                Id= entity.id,
                Name = entity.name,
                Notes = entity.notes,
                CompanyId = entity.CompanyId
            };
            var data =_typeRepository.Update(type);
            TypeDTO entityDto= new TypeDTO() { id=data.Id,name=data.Name,notes=data.Notes,CompanyId=data.CompanyId };
            return entityDto;
            
        }

        
        public void Delete(int id)
        {
            _typeRepository.Delete(id);
        }

        
        public bool IsTypeNameUnique(string typeName)
        {
            return _typeRepository.IsTypeNameUnique(typeName);
        }

        public List<Models.Type> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        List<TypeDTO> ICRUDService<TypeDTO>.GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}