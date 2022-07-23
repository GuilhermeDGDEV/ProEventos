import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '../../../models/Evento';
import { EventoService } from '../../../services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  public modalRef: BsModalRef | undefined;

  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];

  public imageWidth = 150;
  public imageMargin = 2;
  public imageShow = true;
  private filtroListado = '';

  public get filtroLista(): string {
    return this.filtroListado;
  }

  public set filtroLista(value: string) {
    this.filtroListado = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  constructor(
    private eventoService: EventoService,
    private modelService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router,
  ) { }

  public ngOnInit(): void {
    this.spinner.show();
    this.getEventos();
  }

  private filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: Evento) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
        || evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  public getEventos(): void {
    this.eventoService.getEventos().subscribe(
      {
        next: (eventos) => {
          this.eventos = eventos;
          this.eventosFiltrados = this.eventos;
        },
        error: (error) => {
          this.spinner.hide();
          this.toastr.error('Erro ao carregar os eventos: ' + error.message, 'Erro!');
        },
        complete: () => this.spinner.hide()
      }
    );
  }

  public changeImageView(): void {
    this.imageShow = !this.imageShow;
  }

  public openModal(template: TemplateRef<any>): void {
    this.modalRef = this.modelService.show(template, {class: 'modal-sm'})
  }

  public confirm(): void {
    this.modalRef?.hide();
    this.toastr.success('O evento foi deletado com sucesso.', 'Deletado!');
    this.router.navigate([`eventos/lista`]);
  }

  public decline(): void {
    this.modalRef?.hide();
    this.router.navigate([`eventos/lista`]);
  }

  public detalheEvento(eventoId: number): void {
    this.router.navigate([`eventos/detalhe/${eventoId}`]);
  }

}
