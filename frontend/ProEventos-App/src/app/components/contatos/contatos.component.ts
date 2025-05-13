import { Component } from '@angular/core';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faEnvelopeOpenText } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-contatos',
  templateUrl: './contatos.component.html',
  styleUrl: './contatos.component.scss'
})
export class ContatosComponent {

  public faEnvelopeOpenText: IconDefinition = faEnvelopeOpenText;

}
