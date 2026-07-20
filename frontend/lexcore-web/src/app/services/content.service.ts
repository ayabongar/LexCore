import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ContentItem, CreateContentItemRequest, UpdateContentItemRequest } from '../models/content-item';

@Injectable({
  providedIn: 'root'
})
export class ContentService {
  private apiUrl = 'http://localhost:5000/api/content';

  constructor(private http: HttpClient) { }

  getItems(language?: string, status?: string): Observable<ContentItem[]> {
    let params = new HttpParams();
    if (language) params = params.set('language', language);
    if (status) params = params.set('status', status);
    return this.http.get<ContentItem[]>(this.apiUrl, { params });
  }

  getItemById(id: string): Observable<ContentItem> {
    return this.http.get<ContentItem>(`${this.apiUrl}/${id}`);
  }

  createItem(request: CreateContentItemRequest): Observable<ContentItem> {
    return this.http.post<ContentItem>(this.apiUrl, request);
  }

  updateItem(id: string, request: UpdateContentItemRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  deleteItem(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
