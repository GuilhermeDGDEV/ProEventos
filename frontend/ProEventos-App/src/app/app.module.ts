import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { EventosComponent } from './components/eventos/eventos.component';
import { PalestrantesComponent } from './components/palestrantes/palestrantes.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NavComponent } from './shared/nav/nav.component';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { EventoService } from './services/evento.service';
import { DateTimeFormatPipe } from './helpers/date-time-format.pipe';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from "ngx-spinner";
import { TituloComponent } from './shared/titulo/titulo.component';
import { ContatosComponent } from './components/contatos/contatos.component';
import { PerfilComponent } from './components/perfil/perfil.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
    EventosComponent,
    PalestrantesComponent,
    NavComponent,
    DateTimeFormatPipe,
    TituloComponent,
    ContatosComponent,
    PerfilComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    FontAwesomeModule,
    CollapseModule.forRoot(),
    TooltipModule.forRoot(),
    BsDropdownModule.forRoot(),
    ModalModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 4000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }),
    NgxSpinnerModule.forRoot()
  ],
  providers: [
    provideHttpClient(),
    EventoService
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
