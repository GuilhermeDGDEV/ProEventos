import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss'],
})
export class EventosComponent implements OnInit {

  public eventos: any = [];
  public eventosFiltrados: any = [];

  widthImg: number = 150;
  marginImg: number = 2;
  showImg: boolean = true;
  private _listFilter: string = '';

  public get listFilter(): string {
    return this._listFilter;
  }

  public set listFilter(value: string) {
    this._listFilter = value;
    this.eventosFiltrados = this.listFilter ? this.filtrarEventos(this.listFilter) : this.eventos;
  }

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getEventos();
  }

  getEventos(): any {
    this.http.get('https://localhost:7071/api/eventos').subscribe(
      response => this.eventos = this.eventosFiltrados = response,
      error => console.log(error)
    );
  }

  filtrarEventos(filtro: string): any {
    filtro = filtro.toLowerCase();
    return this.eventos.filter(
      (evento: any) => evento.tema.toLowerCase().indexOf(filtro) !== -1
        || evento.local.toLowerCase().indexOf(filtro) !== -1
    );
  }
}
