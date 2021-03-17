import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { ValidarRequisitosLiquidacionComponent } from './components/validar-requisitos-liquidacion/validar-requisitos-liquidacion.component';
import { ValidarBalanceComponent } from './components/validar-balance/validar-balance.component';
import { RecursosComprometidosPagadosComponent } from './components/recursos-comprometidos-pagados/recursos-comprometidos-pagados.component';
import { VerDetalleComponent } from './components/ver-detalle/ver-detalle.component';
import { VerEjecucionFinancieraComponent } from './components/ver-ejecucion-financiera/ver-ejecucion-financiera.component';
import { VerTrasladosRecursosComponent } from './components/ver-traslados-recursos/ver-traslados-recursos.component';
import { DetalleTrasladosComponent } from './components/detalle-traslados/detalle-traslados.component';
import { ValidarInformeFinalComponent } from './components/validar-informe-final/validar-informe-final.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'obra/validarRequisitos/:id',
    component: ValidarRequisitosLiquidacionComponent
  },
  {
    path: 'obra/validarRequisitos/:id/validarBalance/:id',
    component: ValidarBalanceComponent
  },
  {
    path: 'obra/validarRequisitos/:id/validarBalance/:id/verRecursos',
    component: RecursosComprometidosPagadosComponent
  },
  {
    path: 'obra/validarRequisitos/:id/validarBalance/:id/verRecursos/verDetalle/:id',
    component: VerDetalleComponent
  },
  {
    path: 'obra/validarRequisitos/:id/validarBalance/:id/verEjecucionFinanciera',
    component: VerEjecucionFinancieraComponent
  },
  {
    path: 'obra/validarRequisitos/:id/validarBalance/:id/verTrasladosRecursos',
    component: VerTrasladosRecursosComponent
  },
  {
    path: 'obra/validarRequisitos/:id/validarBalance/:id/verTrasladosRecursos/verDetalle/:id',
    component: DetalleTrasladosComponent
  },
  {
    path: 'obra/validarRequisitos/:id/informeFinal/:id',
    component: ValidarInformeFinalComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarValidarSolicitudLiquidacionContractualRoutingModule { }
