import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../_models/Evento';

@Injectable({
  providedIn: 'root'
})
export class EventoService {

  baseURL = 'http://localhost:5000/v1/evento';

  constructor(private http: HttpClient) { }

  getAllEventos(): Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseURL);
  }

  getEventoByTema(tema: string): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/tema/${tema}`);
  }

  getEventoById(id: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/${id}`);
  }

  postEvento(evento: Evento) {
    return this.http.post<Evento>(this.baseURL, evento);
  }

  putEvento(evento: Evento) {
    return this.http.put<Evento>(`${[this.baseURL]}/${evento.id}`, evento);
  }

  deleteEvento(evento: Evento) {
    return this.http.delete<Evento>(`${[this.baseURL]}/${evento.id}`);
  }
}
