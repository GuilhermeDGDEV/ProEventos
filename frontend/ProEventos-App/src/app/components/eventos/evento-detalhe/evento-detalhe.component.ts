import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrl: './evento-detalhe.component.scss'
})
export class EventoDetalheComponent implements OnInit {

  public evento = {} as Evento;
  public form!: FormGroup;
  public estadoSalvar: string = 'post';

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
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
    });
  }

  public resetForm(event: MouseEvent): void {
    event.preventDefault();
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl): object {
    return { 'is-invalid': campoForm.errors && campoForm.touched }
  }

  public carregarEvento(): void {
    const eventoIdParam = this.router.snapshot.paramMap.get('id') ?? 0;
    if (eventoIdParam) {
      this.spinner.show();
      this.estadoSalvar = 'put';
      this.eventoService.getEventoById(+eventoIdParam).subscribe({
        next: (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
        },
        error: () => this.toastr.error('Erro ao tentar carregar evento.', 'Erro!')
      }).add(() => this.spinner.hide());
    }
  }

  public salvarAlteracao(): void {
    this.spinner.show();
    if (this.form.valid) {
      this.evento = {
        id: this.estadoSalvar === 'put' ? this.evento.id : 0,
        ...this.form.value
      };
      this.eventoService[this.estadoSalvar](this.evento).subscribe({
        next: () => this.toastr.success('Evento salvo com sucesso!', 'Sucesso'),
        error: (err: any) => {
          console.error(err);
          this.toastr.error('Erro ao salvar evento', 'Erro');
        }
      }).add(() => this.spinner.hide());
    }
  }

}
