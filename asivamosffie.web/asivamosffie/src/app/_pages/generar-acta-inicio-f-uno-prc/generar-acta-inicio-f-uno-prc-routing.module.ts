import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GeneracionActaIniFIPreconstruccionComponent } from './components/generacion-acta-ini-f-i-prc/generacion-acta-ini-f-i-prc.component';
import { GenerarActaInicioFaseunoPreconstruccionComponent } from './components/generar-acta-inicio-f-i-prc/generar-acta-inicio-f-i-prc.component';
import { ValidarActaDeInicioFIPreconstruccionComponent } from './components/validar-acta-de-inicio-f-i-prc/validar-acta-de-inicio-f-i-prc.component';
import { VerDetalleActaIniFIPreconstruccioComponent } from './components/ver-detalle-acta-ini-f-i-prc/ver-detalle-acta-ini-f-i-prc.component';
import { VerDetalleEditarActaIniFIPreconstruccioComponent } from './components/ver-detalle-editar-acta-ini-f-i-prc/ver-detalle-editar-acta-ini-f-i-prc.component';

const routes: Routes = [
  {
    path: '',
    component: GenerarActaInicioFaseunoPreconstruccionComponent
  },
  {
    path: 'generarActa/:id',
    component: GeneracionActaIniFIPreconstruccionComponent
  },
  {
    path: 'verDetalleEditarActa/:id',
    component: VerDetalleEditarActaIniFIPreconstruccioComponent
  },
  {
    path: 'verDetalleActa/:id',
    component: VerDetalleActaIniFIPreconstruccioComponent
  },
  {
    path: 'validarActaDeInicio/:id',
    component: ValidarActaDeInicioFIPreconstruccionComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GenerarActaInicioFaseunoPreconstruccionRoutingModule { }