import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '@app/services/evento.service';
import { Evento } from '@app/models/Evento';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEye, faEyeSlash, faEdit, faTrash, faPlusCircle } from '@fortawesome/free-solid-svg-icons';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrl: './evento-lista.component.scss'
})
export class EventoListaComponent implements OnInit {

  public faEye: IconDefinition = faEye;
  public faEyeSlash: IconDefinition = faEyeSlash;
  public faEdit: IconDefinition = faEdit;
  public faTrash: IconDefinition = faTrash;
  public faPlusCircle: IconDefinition = faPlusCircle;

  public modalRef?: BsModalRef;

  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public eventoId: number = 0;
  public larguraImagem: number = 150;
  public margemImagem: number = 2;
  public exibirImagem: boolean = true;
  private _filtroLista: string = '';

  public get filtroLista(): string {
    return this._filtroLista;
  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  public ngOnInit(): void {
    this.carregarEventos();
  }

  public carregarEventos(): void {
    this.spinner.show();
    this.eventoService.getEventos().subscribe({
      next: (_eventos: Evento[]) => {
        this.eventos = _eventos;
        this.eventosFiltrados = this.eventos;
      },
      error: () => this.toastr.error('Erro ao carregar os Eventos', 'Erro!')
    }).add(() => this.spinner.hide());
  }

  public alterarImagem(): void {
    this.exibirImagem = !this.exibirImagem;
  }

  public filtrarEventos(filtrarPor: string): any {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: any) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
        || evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  public openModal(event: MouseEvent, template: TemplateRef<any>, eventoId: number) {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();
    this.eventoService.deleteEvento(this.eventoId).subscribe({
      next: (result: any) => {
        if (result.message === 'Deletado') {
          this.toastr.success('O Evento foi deletado com sucesso', 'Deletado');
          this.carregarEventos();
        }
      },
      error: () => this.toastr.error(`Erro ao tentar deletar o evento ${this.eventoId}`, 'Erro')
    }).add(() => this.spinner.hide());
  }
 
  public decline(): void {
    this.modalRef?.hide();
  }

  public detalheEventos(id: number): void {
    this.router.navigate([`/eventos/detalhe/${id}`]);
  }

  public mostraImagem(imagemURL: string): string {
    return imagemURL ?
      environment.apiURL + 'Resources/images/' + imagemURL :
      "/assets/img/semImagem.jpeg";
  }

}
