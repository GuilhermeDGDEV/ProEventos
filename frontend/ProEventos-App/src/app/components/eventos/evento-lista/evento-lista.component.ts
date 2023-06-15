import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEye, faEyeSlash, faEdit, faTrash, faPlusCircle } from '@fortawesome/free-solid-svg-icons';

import { Evento } from '@app/model/Evento';
import { EventoService } from '@app/services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  public faEye: IconDefinition = faEye;
  public faEyeSlash: IconDefinition = faEyeSlash;
  public faEdit: IconDefinition = faEdit;
  public faTrash: IconDefinition = faTrash;
  public faPlusCircle: IconDefinition = faPlusCircle;

  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public eventoId: number = 0;

  public widthImg: number = 150;
  public marginImg: number = 2;
  public showImg: boolean = true;
  private _listFilter: string = '';

  public modalRef: BsModalRef | undefined;

  public get listFilter(): string {
    return this._listFilter;
  }

  public set listFilter(value: string) {
    this._listFilter = value;
    this.eventosFiltrados = this.listFilter ? this.filtrarEventos(this.listFilter) : this.eventos;
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router) {}

  public ngOnInit(): void {
    this.spinner.show();
    this.getEventos();
  }

  public getEventos(): void {
    this.eventoService.getEventos().subscribe(
      {
        next: (_eventos: Evento[]) => this.eventos = this.eventosFiltrados = _eventos,
        error: () => {
          this.toastr.error('Erro ao carregar os eventos', 'Erro!');
          this.spinner.hide();
        },
        complete: () => this.spinner.hide()
      }
    );
  }

  public filtrarEventos(filtro: string): Evento[] {
    filtro = filtro.toLowerCase();
    return this.eventos.filter(
      (evento: Evento) => (evento.tema && evento.tema.toLowerCase().indexOf(filtro) !== -1)
        || evento.local && evento.local.toLowerCase().indexOf(filtro) !== -1
    );
  }

  public confirm(): void {
    this.closeModal();
    this.spinner.show();

    this.eventoService.deleteEvento(this.eventoId).subscribe({
      next: () => this.toastr.success('O evento foi deletado com sucesso.', 'Deletado!'),
      error: () => this.toastr.error(`Erro ao tentar deletar evento (id: ${this.eventoId}).`, 'Erro!')
    }).add(() => {
      this.spinner.hide();
      this.getEventos();
    });
  }

  public decline(): void {
    this.closeModal();
  }

  public openModal(event: any, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  private closeModal(): void {
    this.modalRef?.hide();
  }

  public detalheEvento(id: number): void {
    this.router.navigate([`/eventos/detalhe/${id}`]);
  }

}
