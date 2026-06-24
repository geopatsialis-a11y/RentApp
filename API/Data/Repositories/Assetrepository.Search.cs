using System;
using System.Text;
using API.DTOs.Asset;
using API.Helper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace API.Data.Repositories;


public partial class AssetRepository
{
    // ==================================================================
    //  DYNAMIC FACET SEARCH (the "eBay-style" filter panel)
    //
    //  Filters are matched against the AssetTypeField.Name keys stored in
    //  Asset.PropertiesJson (jsonb). Each filter is translated into a
    //  PostgreSQL jsonb operator on "PropertiesJson"->>'<field>', cast to
    //  the right type, and all filters are AND-combined.
    //
    //  SECURITY:
    //  - All filter VALUES are bound as Npgsql parameters, never concatenated.
    //  - Only the FIELD NAME is interpolated into the SQL text, and it is
    //    validated by AssetService.SearchAsync against the AssetType's own
    //    schema before this method ever runs, so it can only be one of a
    //    small set of known machine keys belonging to this tenant — never
    //    arbitrary user input. EscapeJsonKey() additionally strips quote
    //    characters as a defensive second layer.
    //  - FromSqlRaw bypasses EF Core's global tenant/soft-delete query
    //    filter, so the WHERE clause restates "TenantId"/"IsDeleted"
    //    explicitly AND the result is filtered again via LINQ .Where()
    //    on the strongly-typed TenantId property as defense-in-depth.
    // ==================================================================
 
    public async Task<PaginatedResult<AssetDto>> SearchAsync(AssetSearchRequest request)
    {
        var tenantId = tenantProvider.TenantId;
 
        var sql = new StringBuilder(@"
            SELECT * FROM ""Assets""
            WHERE ""AssetTypeId"" = @assetTypeId
              AND ""TenantId"" = @tenantId
              AND ""IsDeleted"" = false");
 
        var parameters = new List<NpgsqlParameter>
        {
            new("assetTypeId", request.AssetTypeId),
            new("tenantId", tenantId)
        };
 
        if (request.Status.HasValue)
        {
            sql.Append(@" AND ""Status"" = @status");
            parameters.Add(new NpgsqlParameter("status", (int)request.Status.Value));
        }
 
        var paramIndex = 0;
        foreach (var filter in request.Filters)
        {
            paramIndex++;
            var jsonPath = $@"""PropertiesJson""->>'{EscapeJsonKey(filter.FieldName)}'";
 
            if (!string.IsNullOrWhiteSpace(filter.Equals))
            {
                var pName = $"eq{paramIndex}";
                sql.Append($@" AND {jsonPath} = @{pName}");
                parameters.Add(new NpgsqlParameter(pName, filter.Equals));
            }
 
            if (filter.MinValue.HasValue)
            {
                var pName = $"min{paramIndex}";
                sql.Append($@" AND ({jsonPath})::numeric >= @{pName}");
                parameters.Add(new NpgsqlParameter(pName, NpgsqlDbType.Numeric) { Value = filter.MinValue.Value });
            }
 
            if (filter.MaxValue.HasValue)
            {
                var pName = $"max{paramIndex}";
                sql.Append($@" AND ({jsonPath})::numeric <= @{pName}");
                parameters.Add(new NpgsqlParameter(pName, NpgsqlDbType.Numeric) { Value = filter.MaxValue.Value });
            }
 
            if (filter.MinDate.HasValue)
            {
                var pName = $"mind{paramIndex}";
                sql.Append($@" AND ({jsonPath})::timestamp >= @{pName}");
                parameters.Add(new NpgsqlParameter(pName, NpgsqlDbType.Timestamp) { Value = filter.MinDate.Value });
            }
 
            if (filter.MaxDate.HasValue)
            {
                var pName = $"maxd{paramIndex}";
                sql.Append($@" AND ({jsonPath})::timestamp <= @{pName}");
                parameters.Add(new NpgsqlParameter(pName, NpgsqlDbType.Timestamp) { Value = filter.MaxDate.Value });
            }
        }
 
        // ---- total count (mirrors the same WHERE, without paging) ----
        var countSql = sql.ToString().Replace(@"SELECT * FROM ""Assets""", @"SELECT COUNT(*) FROM ""Assets""");
        var totalCount = await context.Database
            .SqlQueryRaw<int>(countSql, CloneAll(parameters))
            .FirstAsync();
 
        // ---- paged rows ----
        sql.Append(@" ORDER BY ""CreatedAt"" DESC");
        sql.Append(@" OFFSET @offset LIMIT @limit");
        parameters.Add(new NpgsqlParameter("offset", (request.Page - 1) * request.PageSize));
        parameters.Add(new NpgsqlParameter("limit", request.PageSize));
 
        var assets = await context.Assets
            .FromSqlRaw(sql.ToString(), CloneAll(parameters))
            // Defense-in-depth: re-assert tenant scoping via LINQ even though
            // the raw SQL already filters it, in case the SQL above is ever
            // edited without preserving the WHERE clause.
            .Where(a => a.TenantId == tenantId && !a.IsDeleted)
            .Include(a => a.AssetType)
            .Include(a => a.AttributeValues).ThenInclude(v => v.AssetTypeField)
            .AsNoTracking()
            .ToListAsync();
 
        return new PaginatedResult<AssetDto>
        {
            Items = assets.Select(MapToDto).ToList(),
            Metadata = new PaginationMetadata
            {
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            }
        };
    }
 
    // Field names are validated as exact matches against this tenant's own
    // AssetTypeField.Name (machine keys, e.g. "mileage", "color") by
    // AssetService.SearchAsync before this method is ever called. This strip
    // of quote characters is a defensive second layer, not the primary guard.
    private static string EscapeJsonKey(string key) => key.Replace("'", "").Replace("\"", "");
 
    // NpgsqlParameter instances are consumed/disposed by the command that
    // executes them — the count query and the paged query are two separate
    // commands, so each needs its own parameter objects built from the same values.
    private static object[] CloneAll(List<NpgsqlParameter> source) =>
        source.Select(p => (object)new NpgsqlParameter(p.ParameterName, p.NpgsqlDbType) { Value = p.Value })
              .ToArray();
}
 