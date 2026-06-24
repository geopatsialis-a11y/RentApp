using System;
using API.DTOs.Customer;
using API.Entities;
using API.Helper;

namespace API.Interfaces;

public interface ICustomerRepository
{
    Task<PaginatedResult<CustomerDto>> GetAllAsync(PagingParams pagingParams);
    Task<CustomerDto?> GetByIdAsync(Guid id);
    Task<List<CustomerLookupDto>> GetLookupAsync(string? search);
 
    Task AddAsync(Customer customer);
    void Update(Customer customer);
    void SoftDelete(Customer customer);
 
    // Helpers used by the service layer for validation before mutating
    Task<Customer?> GetEntityByIdAsync(Guid id);
    Task<bool> AfmExistsAsync(string afm, Guid? excludingId = null);
    Task<bool> HasActiveContractsAsync(Guid customerId);
 
    // Contacts (sub-resource)
    Task AddContactAsync(Contact contact);
    Task<Contact?> GetContactEntityByIdAsync(Guid contactId);
    void RemoveContact(Contact contact);
 
}