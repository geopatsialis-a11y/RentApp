import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import {
  AssetDto, AssetCreateDto, AssetUpdateDto, AssetStatusUpdateDto,
  AssetLookupDto, AssetTypeLookupDto, AssetTypeDto,
  CostAssetHistDto, CostAssetHistCreateDto,
  PhotoDto,
  AssetSearchRequest,
  AssetDetailDto,
  AssetContractHistDto,
  CostAssetHistUpdateDto
} from '../../types/asset';
import { PaginatedResult } from '../../types/pagination';

@Injectable({ providedIn: 'root' })
export class AssetService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}asset`;
  private typeBase = `${environment.apiUrl}assettype`;

getAssets(pageNumber: number, pageSize: number, searchTerm?: string, assetTypeId?: string, status?: number, sortBy?: string) {
      let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    if (searchTerm) params = params.set('search', searchTerm);
    if (assetTypeId) params = params.set('assetTypeId', assetTypeId);
    if (status !== undefined && status !== null) params = params.set('status', status);
    if (sortBy) params = params.set('sortBy', sortBy);

    return this.http.get<PaginatedResult<AssetDto>>(this.base, { params });
  }

  getById(id: string) { return this.http.get<AssetDetailDto>(`${this.base}/${id}`); }

  create(dto: AssetCreateDto) { return this.http.post<AssetDto>(this.base, dto); }

  update(id: string, dto: AssetUpdateDto) { return this.http.put<AssetDto>(`${this.base}/${id}`, dto); }

  updateStatus(id: string, dto: AssetStatusUpdateDto) {
    return this.http.patch<AssetDto>(`${this.base}/${id}/status`, dto);
  }

  delete(id: string) { return this.http.delete(`${this.base}/${id}`); }

  getAssetTypes() { return this.http.get<AssetTypeLookupDto[]>(`${this.typeBase}/lookup`); }

  getAssetTypeById(id: string) { return this.http.get<AssetTypeDto>(`${this.typeBase}/${id}`); }

 getMaintenanceHistory(assetId: string, page = 1, pageSize = 5) {
    const params = new HttpParams().set('pageNumber', page).set('pageSize', pageSize);
    return this.http.get<PaginatedResult<CostAssetHistDto>>(`${this.base}/${assetId}/maintenance-history`, { params });
  }

  addMaintenanceRecord(assetId: string, dto: CostAssetHistCreateDto) {
    return this.http.post<CostAssetHistDto>(`${this.base}/${assetId}/maintenance-history`, dto);
  }

  
  getContractHistory(assetId: string, page = 1, pageSize = 5) {
    const params = new HttpParams().set('pageNumber', page).set('pageSize', pageSize);
    return this.http.get<PaginatedResult<AssetContractHistDto>>(`${this.base}/${assetId}/contracts`, { params });
  }

  updateMaintenanceRecord(assetId: string, recordId: string, dto: CostAssetHistUpdateDto) {
    return this.http.put<CostAssetHistDto>(`${this.base}/${assetId}/maintenance-history/${recordId}`, dto);
  }

  deleteMaintenanceRecord(assetId: string, recordId: string) {
    return this.http.delete(`${this.base}/${assetId}/maintenance-history/${recordId}`);
  }

  searchAssets(req: AssetSearchRequest) {
    const body = {
      assetTypeId: req.assetTypeId,
      status: req.status,
      search: req.search,
      page: req.pageNumber ?? 1,
      pageSize: req.pageSize ?? 12,
      sortBy: req.sortBy,
      filters: req.filters.map(f => ({ fieldName: f.fieldName, equals: f.equals }))
    };
    return this.http.post<PaginatedResult<AssetDto>>(`${this.base}/search`, body);
  }


  
  addPhoto(assetId: string, file: File) {
    const form = new FormData();
    form.append('file', file);
    return this.http.post<PhotoDto>(`${this.base}/${assetId}/photos`, form);
  }

  deletePhoto(assetId: string, photoId: string) {
    return this.http.delete(`${this.base}/${assetId}/photos/${photoId}`);
  }

  setMainPhoto(assetId: string, photoId: string) {
    return this.http.patch<AssetDetailDto>(`${this.base}/${assetId}/photos/${photoId}/main`, {});
  }
}
