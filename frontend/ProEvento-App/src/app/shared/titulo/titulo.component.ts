import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-titulo',
  templateUrl: './titulo.component.html',
  styleUrls: ['./titulo.component.scss']
})
export class TituloComponent implements OnInit {

  @Input() titulo?: string;
  @Input() subtitulo: string = 'Desde 2022';
  @Input() iconClass: string = 'fa fa-user';
  @Input() botaoListar: boolean = false;

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  listar(): void {
    this.router.navigate([`/${this.titulo?.toLowerCase()}/lista`])
  }

}
