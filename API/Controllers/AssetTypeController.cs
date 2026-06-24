using API.DTOs.Asset;
using API.Errors;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[Authorize]
public class AssetTypeController(IAssetService assetService) : BaseApiController
{
    // GET api/assettype
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var types = await assetService.GetAssetTypesAsync();
        return Ok(types);
    }

    // GET api/assettype/lookup   (dropdown for "create asset" / "search" pickers)
    [HttpGet("lookup")]
    public async Task<IActionResult> GetLookup()
    {
        var types = await assetService.GetAssetTypeLookupAsync();
        return Ok(types);
    }

    // GET api/assettype/{id}   (includes the full field schema, for the
    // Angular EAV field-renderer to build the create/edit form dynamically)
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AssetTypeDto>> GetById(Guid id)
    {
        var type = await assetService.GetAssetTypeByIdAsync(id);
        return type == null ? NotFound() : Ok(type);
    }

    // POST api/assettype
    [HttpPost]
    public async Task<ActionResult<AssetTypeDto>> Create(AssetTypeCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var created = await assetService.CreateAssetTypeAsync(dto, User.GetMemberId().ToString());
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT api/assettype/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AssetTypeDto>> Update(Guid id, AssetTypeUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updated = await assetService.UpdateAssetTypeAsync(id, dto, User.GetMemberId().ToString());
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

    // DELETE api/assettype/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await assetService.DeleteAssetTypeAsync(id);
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
    //  Dynamic field schema (sub-resource)
    // ----------------------------------------------------------------

    // POST api/assettype/{id}/fields
    [HttpPost("{id:guid}/fields")]
    public async Task<ActionResult<AssetTypeFieldDto>> AddField(Guid id, AssetTypeFieldCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var field = await assetService.AddFieldAsync(id, dto, User.GetMemberId().ToString());
            return Ok(field);
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

    // PUT api/assettype/{id}/fields/{fieldId}
    [HttpPut("{id:guid}/fields/{fieldId:guid}")]
    public async Task<ActionResult<AssetTypeFieldDto>> UpdateField(Guid id, Guid fieldId, AssetTypeFieldUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var field = await assetService.UpdateFieldAsync(id, fieldId, dto, User.GetMemberId().ToString());
            return Ok(field);
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

    // DELETE api/assettype/{id}/fields/{fieldId}
    [HttpDelete("{id:guid}/fields/{fieldId:guid}")]
    public async Task<IActionResult> DeleteField(Guid id, Guid fieldId)
    {
        try
        {
            await assetService.DeleteFieldAsync(id, fieldId);
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
}

 