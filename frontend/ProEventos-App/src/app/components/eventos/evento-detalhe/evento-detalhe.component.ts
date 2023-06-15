import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { Evento } from '@app/model/Evento';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
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
  public estadoSalvar: string = 'post';

  get f(): any {
    return this.form.controls;
  }

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private eventoService: EventoService,
    private activatedRoute: ActivatedRoute,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService) {
    this.localeService.use('pt-br');
  }

  ngOnInit(): void {
    this.carregarEvento();
    this.validation();
  }

  public carregarEvento(): void {
    const eventoIdParam = this.activatedRoute.snapshot.paramMap.get('id');

    if (eventoIdParam != null) {
      this.estadoSalvar = 'put'
      this.spinner.show();
      this.eventoService.getEventoById(+eventoIdParam).subscribe({
        next: (_evento: Evento) => {
          this.evento = {..._evento};
          this.form.patchValue(this.evento);
        },
        error: () => {
          this.toastr.error('Erro ao carregar os evento', 'Erro!');
          this.spinner.hide();
        },
        complete: () => this.spinner.hide()
      });
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
    });
  }

  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }

  public isInvalid(formControl: any): boolean {
    return formControl?.errors && formControl?.touched;
  }

  public save(): void {
    this.showSpinner = true;

    if (this.form.valid) {
      this.evento = {id: this.evento.id, ...this.form.value};
      this.eventoService[this.estadoSalvar](this.evento).subscribe({
        next: () => {
          this.toastr.success('Evento salvo com sucesso!', 'Sucesso');
        },
        error: () => {
          this.showSpinner = false;
          this.toastr.error('Erro ao salvar evento!', 'Erro');
        },
        complete: () => {
          this.showSpinner = false;
        }
      });
    }
  }
}
