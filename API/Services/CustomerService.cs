using System;
using API.DTOs.Contacts;
using API.DTOs.Customer;
using API.Entities;
using API.Helper;
using API.Interfaces;
using API.Errors;


namespace API.Services;

public class CustomerService (IUnitOfWork uow, ITenantProvider tenantProvider): ICustomerService
{
    public Task<PaginatedResult<CustomerDto>> GetAllAsync(PagingParams pagingParams)
        => uow.CustomerRepository.GetAllAsync(pagingParams);
 
    public Task<CustomerDto?> GetByIdAsync(Guid id)
        => uow.CustomerRepository.GetByIdAsync(id);
 
    public Task<List<CustomerLookupDto>> GetLookupAsync(string? search)
        => uow.CustomerRepository.GetLookupAsync(search);

    
    public async Task<CustomerDto> CreateAsync(CustomerCreateDto dto, string currentUserId)
    {
        // Catching this here gives a clean 400 instead of letting the
        // unique index (TenantId, Afm) throw a raw DbUpdateException.
        if (await uow.CustomerRepository.AfmExistsAsync(dto.Afm))
            throw new BadRequestException($"A customer with AFM '{dto.Afm}' already exists.");
 
        var customer = new Customer
        {
            TenantId = tenantProvider.TenantId,
            Type = dto.Type,
            Name = dto.Name,
            Afm = dto.Afm,
            Dou = dto.Dou,
            Address = dto.Address,
            Representative = dto.Representative,
            CreatedBy = currentUserId
        };
 
        await uow.CustomerRepository.AddAsync(customer);
        await uow.Complete();
 
        return (await uow.CustomerRepository.GetByIdAsync(customer.Id))!;
    }
 
    public async Task<CustomerDto> UpdateAsync(Guid id, CustomerUpdateDto dto, string currentUserId)
    {
        var customer = await uow.CustomerRepository.GetEntityByIdAsync(id)
            ?? throw new NotFoundException($"Customer '{id}' was not found.");
 
        if (await uow.CustomerRepository.AfmExistsAsync(dto.Afm, excludingId: id))
            throw new BadRequestException($"Another customer already uses AFM '{dto.Afm}'.");
 
        customer.Type = dto.Type;
        customer.Name = dto.Name;
        customer.Afm = dto.Afm;
        customer.Dou = dto.Dou;
        customer.Address = dto.Address;
        customer.Representative = dto.Representative;
        customer.UpdatedAt = DateTime.UtcNow;
        customer.UpdatedBy = currentUserId;
 
        uow.CustomerRepository.Update(customer);
        await uow.Complete();
 
        return (await uow.CustomerRepository.GetByIdAsync(id))!;
    }
 
    public async Task DeleteAsync(Guid id, string currentUserId)
    {
        var customer = await uow.CustomerRepository.GetEntityByIdAsync(id)
            ?? throw new NotFoundException($"Customer '{id}' was not found.");
 
        // Customer→Contract FK is Restrict, so a hard delete would throw anyway —
        // this gives a meaningful message instead of a raw DbUpdateException.
        if (await uow.CustomerRepository.HasActiveContractsAsync(id))
            throw new BadRequestException("Cannot delete a customer with active contracts.");
 
        customer.DeletedBy = currentUserId;
        uow.CustomerRepository.SoftDelete(customer);
        await uow.Complete();
    }
 
    public async Task<ContactDto> AddContactAsync(Guid customerId, ContactCreateDto dto, string currentUserId)
    {
        var customer = await uow.CustomerRepository.GetEntityByIdAsync(customerId)
            ?? throw new NotFoundException($"Customer '{customerId}' was not found.");
 
        var contact = new Contact
        {
            TenantId = customer.TenantId,
            CustomerId = customerId,
            Name = dto.Name,
            Phone = dto.Phone,
            Email = dto.Email,
            CanUseAsset = dto.CanUseAsset,
            Notes = dto.Notes,
            CreatedBy = currentUserId
        };
 
        await uow.CustomerRepository.AddContactAsync(contact);
        await uow.Complete();
 
        return new ContactDto
        {
            Id = contact.Id,
            Name = contact.Name,
            Phone = contact.Phone,
            Email = contact.Email,
            CanUseAsset = contact.CanUseAsset,
            Notes = contact.Notes
        };
    }
 
    public async Task RemoveContactAsync(Guid customerId, Guid contactId)
    {
        var contact = await uow.CustomerRepository.GetContactEntityByIdAsync(contactId);
 
        if (contact == null || contact.CustomerId != customerId)
            throw new NotFoundException($"Contact '{contactId}' was not found for this customer.");
 
        uow.CustomerRepository.RemoveContact(contact);
        await uow.Complete();
    }

    public async Task<CustomerStatsDto> GetCustomerStatsAsync()
    {
        return await uow.CustomerRepository.GetCustomerStatsAsync();
    }
}

