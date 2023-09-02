import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormGroup, Validators, FormBuilder, FormArray, FormControl, AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { Evento } from '@app/model/Evento';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faMoneyBillWave, faWindowClose, faPlusCircle } from '@fortawesome/free-solid-svg-icons';
import { Lote } from '@app/model/Lote';
import { LoteService } from '@app/services/lote.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
  public eventoId: number = 0;
  public evento = {} as Evento;
  public form: FormGroup = new FormGroup({});
  public showSpinner: boolean = false;
  public datePickerConfig: object = {
    isAnimated: true,
    adaptivePosition: true,
    dateInputFormat: 'DD/MM/YYYY hh:mm a',
    containerClass: 'theme-default',
    showWeekNumbers: false
  }
  public datePickerConfigLote: object = {
    isAnimated: true,
    adaptivePosition: true,
    dateInputFormat: 'DD/MM/YYYY',
    containerClass: 'theme-default',
    showWeekNumbers: false
  }
  public estadoSalvar: string = 'post';

  public faMoneyBillWave: IconDefinition = faMoneyBillWave;
  public faWindowClose: IconDefinition = faWindowClose;
  public faPlusCircle: IconDefinition = faPlusCircle;

  public modalRef: BsModalRef | undefined;
  public loteAtual = {id: 0, nome: '', indice: 0};
  public indiceLoteAtual: number = 0;

  get modoEditar(): boolean {
    return this.estadoSalvar == 'put';
  }

  get f(): any {
    return this.form.controls;
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private eventoService: EventoService,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private loteService: LoteService,
    private modalService: BsModalService) {
    this.localeService.use('pt-br');
  }

  ngOnInit(): void {
    this.carregarEvento();
    this.validation();
  }

  public carregarEvento(): void {
    this.eventoId = Number.parseInt(this.activatedRoute.snapshot.paramMap.get('id'));

    if (this.eventoId) {
      this.estadoSalvar = 'put'
      this.spinner.show();
      this.eventoService.getEventoById(this.eventoId).subscribe({
        next: (_evento: Evento) => {
          this.evento = {..._evento};
          this.form.patchValue(this.evento);
          this.evento.lotes.forEach(lote => this.lotes.push(this.criarLote(lote)))
        },
        error: () => {
          this.toastr.error('Erro ao carregar os evento', 'Erro!');
          this.spinner.hide();
        }
      }).add(() => this.spinner.hide());
    }
  }

  public validation(): void {
    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      lotes: this.fb.array([])
    });
  }

  public adicionarLote(): void {
    this.lotes.push(this.criarLote({id: 0} as Lote));
  }

  public criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio, Validators.required],
      dataFim: [lote.dataFim, Validators.required],
      qtd: [lote.qtd, Validators.required]
    })
  }

  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }

  public isInvalid(formControl: FormControl | AbstractControl): boolean {
    return formControl?.errors && formControl?.touched;
  }

  public salvarEvento(): void {
    if (this.form.valid) {
      this.showSpinner = true;
      this.evento = {id: this.evento.id, ...this.form.value};
      this.eventoService[this.estadoSalvar](this.evento).subscribe({
        next: (evento: Evento) => {
          this.toastr.success('Evento salvo com sucesso!', 'Sucesso');
          this.router.navigate([`/eventos/detalhe/${evento.id}`]);
        },
        error: () => {
          this.showSpinner = false;
          this.toastr.error('Erro ao salvar evento!', 'Erro');
        }
      }).add(() => this.showSpinner = false);
    }
  }

  public salvarLotes(): void {
    if (this.form.controls['lotes'].valid) {
      this.spinner.show();
      this.loteService.saveLote(this.evento.id, this.form.value.lotes).subscribe({
        next: () => this.toastr.success('Lotes salvos com sucesso!', 'Sucesso!'),
        error: () => this.toastr.error('Erro ao tentar salvar lotes.', 'Erro!')
      }).add(() => this.spinner.hide());
    }
  }

  public removerLote(template: TemplateRef<any>, indice: number): void {
    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public confirmDeliteLote(): void {
    this.modalRef?.hide();
    this.spinner.show();
    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe({
      next: () => {
        this.toastr.success('Lote deletado com sucesso!', 'Sucesso!');
        this.lotes.removeAt(this.loteAtual.indice);
      },
      error: () => this.toastr.error('Erro ao tentar deletar lote.', 'Erro!')
    }).add(() => this.spinner.hide());
  }

  public declineDeliteLote(): void {
    this.modalRef?.hide();
  }

  public mudarValorData(value: Date, indice: number, campo: string): void {
    this.lotes.value[indice][campo] = value;
  }

  public retornaTituloLote(indiceDoLote: number): string {
    return this.lotes.get(indiceDoLote + '.nome').value && this.lotes.get(indiceDoLote + '.nome').value.trim() != ''
      ? this.lotes.get(indiceDoLote + '.nome').value
      : 'Nome do lote'
  }

}
