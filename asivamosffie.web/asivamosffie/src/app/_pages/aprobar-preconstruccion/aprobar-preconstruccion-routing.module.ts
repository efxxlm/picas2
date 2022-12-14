import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TituloComponent } from './components/titulo/titulo.component';
import { ExpansionValidarRequisitosComponent } from './components/expansion-validar-requisitos/expansion-validar-requisitos.component';
import { ExpansionInterValidarRequisitosComponent } from './components/expansion-inter-validar-requisitos/expansion-inter-validar-requisitos.component';
import { VerDetalleAprobarPreconstruccionComponent } from './components/ver-detalle-aprobar-preconstruccion/ver-detalle-aprobar-preconstruccion.component';

const routes: Routes = [
  {
    path: '',
    component: TituloComponent
  },
  {
    path: 'obraValidarRequisitos/:id',
    component: ExpansionValidarRequisitosComponent
  },
  {
    path: 'interventoriaValidarRequisitos/:id',
    component: ExpansionInterValidarRequisitosComponent
  },
  {
    path: 'verDetalle/:id',
    component: VerDetalleAprobarPreconstruccionComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarPreconstruccionRoutingModule { }
