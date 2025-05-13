import { Component } from '@angular/core';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faUser } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-palestrantes',
  templateUrl: './palestrantes.component.html',
  styleUrl: './palestrantes.component.scss'
})
export class PalestrantesComponent {

  public faUser: IconDefinition = faUser;

}
