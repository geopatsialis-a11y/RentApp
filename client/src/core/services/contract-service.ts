import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import {
  ContractListItemDto, ContractDetailDto, ContractCreateDto,
  ContractUpdateDto, AvailableAssetDto, RentalStatus
} from '../../types/contract';
import { PaginatedResult } from '../../types/pagination';

@Injectable({ providedIn: 'root' })
export class ContractService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}contract`;

  getAll(pageNumber: number, pageSize: number, search?: string, status?: RentalStatus) {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    if (search) params = params.set('search', search);
    if (status !== undefined && status !== null) params = params.set('status', status);
    return this.http.get<PaginatedResult<ContractListItemDto>>(this.base, { params });
  }

  getById(id: string) {
    return this.http.get<ContractDetailDto>(`${this.base}/${id}`);
  }

  getAvailableAssets(startDate: string, endDate: string, excludeContractId?: string) {
    let params = new HttpParams()
      .set('startDate', startDate)
      .set('endDate', endDate);
    if (excludeContractId) params = params.set('excludeContractId', excludeContractId);
    return this.http.get<AvailableAssetDto[]>(`${this.base}/available-assets`, { params });
  }

  create(dto: ContractCreateDto) {
    return this.http.post<ContractDetailDto>(this.base, dto);
  }

  update(id: string, dto: ContractUpdateDto) {
    return this.http.put<ContractDetailDto>(`${this.base}/${id}`, dto);
  }

  delete(id: string) {
    return this.http.delete(`${this.base}/${id}`);
  }
}
