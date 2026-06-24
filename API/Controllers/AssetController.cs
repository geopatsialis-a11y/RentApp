using API.Data.Contexts;
using API.DTOs;
using API.DTOs.Asset;
using API.Entities;
using API.Errors;
using API.Extensions;
using API.Helper;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static API.Entities.Enums;

namespace API.Controllers;
   
[Authorize]
public class AssetController(IAssetService assetService) : BaseApiController
{
    // GET api/asset?page=1&pageSize=20&search=&assetTypeId=&status=
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagingParams pagingParams,
        [FromQuery] Guid? assetTypeId,
        [FromQuery] AssetStatus? status)
    {
        var result = await assetService.GetAllAsync(pagingParams, assetTypeId, status);
        return Ok(result);
    }

    // GET api/asset/lookup?search=&assetTypeId=
    [HttpGet("lookup")]
    public async Task<IActionResult> GetLookup([FromQuery] string? search, [FromQuery] Guid? assetTypeId)
    {
        var result = await assetService.GetLookupAsync(search, assetTypeId);
        return Ok(result);
    }

    // GET api/asset/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AssetDto>> GetById(Guid id)
    {
        var asset = await assetService.GetByIdAsync(id);
        return asset == null ? NotFound() : Ok(asset);
    }

    // POST api/asset
    [HttpPost]
    public async Task<ActionResult<AssetDto>> Create(AssetCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var created = await assetService.CreateAsync(dto, User.GetMemberId().ToString());
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT api/asset/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AssetDto>> Update(Guid id, AssetUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updated = await assetService.UpdateAsync(id, dto, User.GetMemberId().ToString());
            return Ok(updated);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PATCH api/asset/{id}/status
    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<AssetDto>> UpdateStatus(Guid id, AssetStatusUpdateDto dto)
    {
        try
        {
            var updated = await assetService.UpdateStatusAsync(id, dto, User.GetMemberId().ToString());
            return Ok(updated);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // DELETE api/asset/{id}   (soft delete)
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await assetService.DeleteAsync(id, User.GetMemberId().ToString());
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // ----------------------------------------------------------------
    //  Dynamic facet search — "eBay-style" filter panel
    //  POST (not GET) because Filters is an arbitrary-length list of
    //  objects that doesn't map cleanly to a query string.
    // ----------------------------------------------------------------

    // POST api/asset/search
    [HttpPost("search")]
    public async Task<IActionResult> Search(AssetSearchRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await assetService.SearchAsync(request);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // ----------------------------------------------------------------
    //  Maintenance / cost history (sub-resource)
    // ----------------------------------------------------------------

    // GET api/asset/{id}/maintenance-history
    [HttpGet("{id:guid}/maintenance-history")]
    public async Task<IActionResult> GetMaintenanceHistory(Guid id)
    {
        var history = await assetService.GetMaintenanceHistoryAsync(id);
        return Ok(history);
    }

    // POST api/asset/{id}/maintenance-history
    [HttpPost("{id:guid}/maintenance-history")]
    public async Task<IActionResult> AddMaintenanceRecord(Guid id, CostAssetHistCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var record = await assetService.AddMaintenanceRecordAsync(id, dto, User.GetMemberId().ToString());
            return Ok(record);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}

 
