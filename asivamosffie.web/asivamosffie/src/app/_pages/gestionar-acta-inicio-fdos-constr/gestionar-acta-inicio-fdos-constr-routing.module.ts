import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FormGenerarActaInicioConstTecnicoComponent } from './components/form-generar-acta-inicio-const-tecnico/form-generar-acta-inicio-const-tecnico.component';
import { GestionarActaInicioFdosConstrComponent } from './components/gestionar-acta-inicio-fdos-constr/gestionar-acta-inicio-fdos-constr.component';
import { VerDetalleTecnicoFdosConstrComponent } from './components/ver-detalle-tecnico-fdos-constr/ver-detalle-tecnico-fdos-constr.component';


const routes: Routes = [
  {
    path: '',
    component: GestionarActaInicioFdosConstrComponent
  },
  {
    path:'generarActaFDos/:id',
    component: FormGenerarActaInicioConstTecnicoComponent
  },
  {
    path:'verDetalleActaConstruccion/:id',
    component: VerDetalleTecnicoFdosConstrComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarActaInicioFdosConstrRoutingModule { }

