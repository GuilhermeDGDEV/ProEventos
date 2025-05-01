import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faMoneyBillWave, faPlusCircle, faWindowClose } from '@fortawesome/free-solid-svg-icons';
import { Lote } from '@app/models/Lote';
import { LoteService } from '@app/services/lote.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { environment } from '@environments/environment';
import { DateTimeFormatPipe } from '@app/helpers/date-time-format.pipe';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrl: './evento-detalhe.component.scss'
})
export class EventoDetalheComponent implements OnInit {

  public evento = {} as Evento;
  public eventoId: number = 0;
  public loteAtual?: Lote;
  public loteAtualIndice: number = 0;
  public form!: FormGroup;
  public estadoSalvar: string = 'post';
  public imagemURL: (string | ArrayBuffer) = '/assets/img/upload.png';
  public file: (File | null) = null;

  public faMoneyBillWave: IconDefinition = faMoneyBillWave;
  public faPlusCircle: IconDefinition = faPlusCircle;
  public faWindowClose: IconDefinition = faWindowClose;

  public modalRef?: BsModalRef;

  get f(): any {
    return this.form.controls;
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get bsConfig(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY HH:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  get bsConfigLote(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private eventoService: EventoService,
    private loteService: LoteService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private modalService: BsModalService
  ) {
    this.localeService.use('pt-br');
  }

  public ngOnInit(): void {
    this.carregarEvento();
    this.validator();
  }

  public validator(): void {
    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdpessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemURL: [''],
      lotes: this.fb.array([])
    });
  }

  public adicionarLote(): void {
    this.lotes.push(this.criarLote({ id: 0 } as Lote));
  }

  public criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dateInicio: [lote.dateInicio],
      dateFim: [lote.dateFim],
    });
  }

  public resetForm(event: MouseEvent): void {
    event.preventDefault();
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl | null): object {
    return { 'is-invalid': campoForm?.errors && campoForm?.touched };
  }

  public carregarEvento(): void {
    this.eventoId = +(this.activatedRoute.snapshot.paramMap.get('id') ?? 0);
    if (this.eventoId) {
      this.spinner.show();
      this.estadoSalvar = 'put';
      this.eventoService.getEventoById(this.eventoId).subscribe({
        next: (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
          this.evento.lotes.forEach(lote => {
            this.lotes.push(this.criarLote(lote));
          });

          if (this.evento.imagemURL) {
            this.imagemURL = environment.apiURL + 'Resources/images/' + this.evento.imagemURL;
          }
        },
        error: () => this.toastr.error('Erro ao tentar carregar evento.', 'Erro!')
      }).add(() => this.spinner.hide());
    }
  }

  public salvarEvento(): void {
    if (this.form.valid) {
      this.spinner.show();
      this.evento = {
        id: this.estadoSalvar === 'put' ? this.evento.id : 0,
        ...this.form.value
      };
      this.eventoService[this.estadoSalvar](this.evento).subscribe({
        next: (eventoRetorno: Evento) => {
          this.toastr.success('Evento salvo com sucesso!', 'Sucesso');
          this.router.navigate([`/eventos/detalhe/${eventoRetorno.id}`]);
        },
        error: () => this.toastr.error('Erro ao salvar evento', 'Erro')
      }).add(() => this.spinner.hide());
    }
  }

  public salvarLotes(): void {
    if (this.form.controls['lotes'].valid) {
      this.spinner.show();
      this.loteService.saveLotes(this.eventoId, this.form.value.lotes)
        .subscribe({
          next: () => {
            this.toastr.success('Lotes salvos com sucesso!', 'Sucesso');
            this.router.navigate([`/eventos/detalhe/${this.eventoId}`]);
          },
          error: (err) => {
            this.toastr.error('Erro ao salvar lotes', 'Erro');
            console.error(err);
          }
        }).add(() => this.spinner.hide());
    }
  }

  public removerLote(indice: number, template: TemplateRef<any>): void {
    this.loteAtual = this.lotes.controls[indice].value;
    this.loteAtualIndice = indice;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public confirmDeleteLote(): void {
    this.modalRef?.hide();
    this.lotes.removeAt(this.loteAtualIndice);
    if (this.loteAtual?.id) {
      this.spinner.show();
      this.loteService.deleteLote(this.eventoId, this.loteAtual?.id ?? 0)
      .subscribe({
        next: () => this.toastr.success('Lote deletado com sucesso!', 'Sucesso'),
        error: () => this.toastr.error('Erro ao deletar lote', 'Erro')
      }).add(() => this.spinner.hide());
    }
  }

  public declineDeleteLote(): void {
    this.modalRef?.hide();
  }

  public onFileChange(event: Event): void {
    const reader = new FileReader();

    reader.onload = (e: ProgressEvent<FileReader>) =>
      this.imagemURL = e.target?.result ?? "";

    const htmlElement = (<HTMLInputElement>event.target);
    this.file = htmlElement.files ? htmlElement.files[0] : null;
    reader.readAsDataURL(this.file ?? new Blob());

    this.uploadImagem();
  }

  public uploadImagem(): void {
    this.spinner.show();
    this.eventoService.postUpload(this.eventoId, this.file).subscribe({
      next: (eventoRetorno: Evento) => {
        this.toastr.success('Imagem atualizada com sucesso!', 'Sucesso');
        this.router.navigate([`/eventos/detalhe/${eventoRetorno.id}`]);
      },
      error: (e) => {
        this.toastr.error('Erro ao fazer upload de imagem.', 'Erro');
        console.log(e)
      }
    }).add(() => this.spinner.hide());
  }

  public retornaData(data: any): string {
    try {
      const retorno = new Date(data);
      return retorno.toLocaleString() != 'Invalid Date' ?
        retorno.toLocaleString().replace(',', '') : data;
    }
    catch (e) {
      return data;
    }
  }

}
