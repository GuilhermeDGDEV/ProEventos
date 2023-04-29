import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';

@Component({
  selector: 'app-titulo',
  templateUrl: './titulo.component.html',
  styleUrls: ['./titulo.component.scss']
})
export class TituloComponent {
  @Input() titulo: string = '';
  @Input() subTitulo: string = 'Desde 2023';
  @Input() icon: IconDefinition | undefined;
  @Input() botaoListar: boolean = false;

  constructor(private router: Router) {}

  public listar(): void {
    this.router.navigate([`/${this.titulo.toLocaleLowerCase()}/lista`]);
  }
}
