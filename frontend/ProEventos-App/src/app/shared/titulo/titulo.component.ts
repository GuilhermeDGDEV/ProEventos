import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';

@Component({
  selector: 'app-titulo',
  templateUrl: './titulo.component.html',
  styleUrl: './titulo.component.scss'
})
export class TituloComponent {

  @Input()
  public titulo!: string;

  @Input()
  public subtitulo: string = 'Desde 2024';

  @Input()
  public botaoListar!: boolean;

  @Input()
  public iconClass!: IconDefinition;

  constructor(private router: Router) {}

  public listar(): void {
    this.router.navigate([`/${this.titulo.toLocaleLowerCase()}/lista`])
  }

}
