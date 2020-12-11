import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActualizarTramiteRaccComponent } from './components/actualizar-tramite-racc/actualizar-tramite-racc.component';
import { DetalleAvanceActuaDerivadasComponent } from './components/detalle-avance-actua-derivadas/detalle-avance-actua-derivadas.component';
import { RegistrarActuacionControvContrcComponent } from './components/registrar-actuacion-controv-contrc/registrar-actuacion-controv-contrc.component';
import { RegistrarAvanceActuaDerivadasComponent } from './components/registrar-avance-actua-derivadas/registrar-avance-actua-derivadas.component';
import { VerdetalleeditAvanceActuaDerivadasComponent } from './components/verdetalleedit-avance-actua-derivadas/verdetalleedit-avance-actua-derivadas.component';

const routes: Routes = [
  {
    path: '',
    component: RegistrarActuacionControvContrcComponent
  },
  {
    path: 'actualizarTramite/:id',
    component: ActualizarTramiteRaccComponent
  },
  {
    path:'registrarActuacionDerivada',
    component: RegistrarAvanceActuaDerivadasComponent
  },
  {
    path:'verDetalleEditarActuacionDerivada/:id',
    component: VerdetalleeditAvanceActuaDerivadasComponent
  },
  {
    path:'verDetalleActuacionDerivada/:id',
    component: DetalleAvanceActuaDerivadasComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarActuacionControvContrcRoutingModule { }
