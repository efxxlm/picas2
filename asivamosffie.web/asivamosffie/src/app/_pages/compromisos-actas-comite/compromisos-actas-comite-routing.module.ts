import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CompromisosActasComponent } from './components/compromisos-actas/compromisos-actas.component';
import { ReporteAvanceCompromisoComponent } from './components/reporte-avance-compromiso/reporte-avance-compromiso.component';
import { RevisionActaComponent } from './components/revision-acta/revision-acta.component';


const routes: Routes = [
  {
    path: '',
    component: CompromisosActasComponent
  },
  {
    path: 'reporteAvanceCompromiso/:id',
    component: ReporteAvanceCompromisoComponent
  },
  {
    path: 'revisionActa',
    component: RevisionActaComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CompromisosActasComiteRoutingModule { }
