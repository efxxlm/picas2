import { GestionarAcuerdoCofinanciacionRoutingModule } from './_pages/gestionar-acuerdo-cofinanciacion/gestionar-acuerdo-cofinanciacion-routing.module';

import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules, PreloadingStrategy, NoPreloading } from '@angular/router';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  MAT_MOMENT_DATE_FORMATS,
  MomentDateAdapter,
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';

import { LayoutComponent } from './layout/layout.component';

import { AuthGuard } from './_guards/auth.guard';
import { RegistrarAcuerdoComponent } from './_pages/gestionar-acuerdo-cofinanciacion/components/registrar-acuerdo/registrar-acuerdo.component';
import { RegistrarComponent } from './_pages/gestionar-fuentes-de-financiacion/components/registrar/registrar.component';
import { CanDeactivateGuard } from './_guards/can-deactivate.guard';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: '',
        redirectTo: '/inicio',
        pathMatch: 'full'
      },
      {
        path: 'inicio',
        loadChildren: () => import('./_pages/inicio/inicio.module').then(m => m.InicioModule)
      },
      {
        path: 'home',
        // canActivate: [AuthGuard],
        loadChildren: () => import('./_pages/home/home.module').then(m => m.HomeModule)
      },
      {
        path: 'cambiarContrasena',
        loadChildren: () => import('./_pages/cambiar-contrasena/cambiar-contrasena.module').then(m => m.CambiarContrasenaModule)
      },
      {
        path: 'cargarMasivamente',
        // tslint:disable-next-line: max-line-length
        loadChildren: () => import('./_pages/cargar-masivamente-proyectos-viabilizados/cargar-masivamente-proyectos-viabilizados.module')
          .then(m => m.CargarMasivamenteProyectosViabilizadosModule)
      },
      {
        path: 'gestionarAcuerdos',
        loadChildren: () => import('./_pages/gestionar-acuerdo-cofinanciacion/gestionar-acuerdo-cofinanciacion.module')
        .then(m => m.GestionarAcuerdoCofinanciacionModule)
      },
      {
        path: 'crearProyecto',
        loadChildren: () => import('./_pages/crear-proyecto-tecnico/crear-proyecto-tecnico.module')
          .then(m => m.CrearProyectoTecnicoModule)
      }
      ,
      {
        path: 'crearProyectoAdministrativo',
        loadChildren: () => import('./_pages/crear-proyecto-admin/crear-proyecto-admin.module')
          .then(m => m.CrearProyectoAdminModule)
      }
      ,
      {
        path: 'gestionarFuentes',
        loadChildren: () => import('./_pages/gestionar-fuentes-de-financiacion/gestionar-fuentes-de-financiacion.module')
        .then(m => m.GestionarFuentesDeFinanciacionModule)
      },
      {
        path: 'registrarAcuerdos/:id',
        component: RegistrarAcuerdoComponent,
      },
      {
        path: 'gestionarFuentes',
        loadChildren: () => import('./_pages/gestionar-fuentes-de-financiacion/gestionar-fuentes-de-financiacion.module')
        .then(m => m.GestionarFuentesDeFinanciacionModule)

      },
      {
        path: 'generarRegistroPresupuestal',
        loadChildren: () => import('./_pages/generar-registro-presupuestal/generar-registro-presupuestal.module')
        .then(m => m.GenerarRegistroPresupuestalModule)
      },
      {
        path: 'generarActaInicioFaseIPreconstruccion',
        loadChildren: () => import('./_pages/generar-acta-inicio-f-uno-prc/generar-acta-inicio-f-uno-prc.module')
        .then(m => m.GenerarActaInicioFaseunoPreconstruccionModule)
      },
      {
        path: 'generarActaInicioFaseIPreconstruccion',
        loadChildren: () => import('./_pages/generar-acta-inicio-f-uno-prc/generar-acta-inicio-f-uno-prc.module')
        .then(m => m.GenerarActaInicioFaseunoPreconstruccionModule)
      },
      {
        path: 'registrarFuentes/:idTipoAportante/:idAportante',
        component: RegistrarComponent,

      },
      {
        path: 'solicitarDisponibilidadPresupuestal',
        loadChildren: () => import('./_pages/solicitar-disponibilidad-presupuestal/solicitar-disponibilidad-presupuestal.module')
          .then(m => m.SolicitarDisponibilidadPresupuestalModule)
      },
      {
        path: 'validarDisponibilidadPresupuesto',
        loadChildren: () => import('./_pages/validar-disponibilidad-presupuesto/validar-disponibilidad-presupuesto.module')
        .then(m => m.ValidarDisponibilidadPresupuestoModule)
      },
      {
        path: 'comiteTecnico',
        loadChildren: () => import('./_pages/comite-tecnico/comite-tecnico.module').then(m => m.ComiteTecnicoModule)
      },
      {
        path: 'seleccion',
        loadChildren: () => import('./_pages/gestionar-procesos-de-seleccion/gestionar-procesos-de-seleccion.module')
        .then(m => m.GestionarProcesosDeSeleccionModule)
      },
      {
        path: 'contratosModificacionesContractuales',
        loadChildren: () => import( './_pages/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.module' )
          .then( module => module.ContratosModificacionesContractualesModule )
      },
      {
        path: 'generarPolizasYGarantias',
        loadChildren: () => import('./_pages/generar-polizas-y-garantias/generar-polizas-y-garantias.module')
          .then(m => m.GenerarPolizasYGarantiasModule)
      },
      {
        path: 'comiteFiduciario',
        loadChildren: () => import( './_pages/sesion-comite-fiduciario/comite-fiduciario.module' )
          .then( module => module.ComiteFiduciarioModule )
      },
      {

        path: 'procesosContractuales',
        loadChildren: () => import( './_pages/gestionar-procesos-contractuales/gestionar-procesos-contractuales.module' )
          .then( module => module.GestionarProcesosContractualesModule )
      },
      {
        path: 'solicitarContratacion',
        loadChildren: () => import('./_pages/solicitar-contratacion/solicitar-contratacion.module')
        .then(m => m.SolicitarContratacionModule)
      },
      {
        path: 'preconstruccion',
        loadChildren: () => import('./_pages/fase-preconstruccion/fase-preconstruccion.module')
        .then(m => m.FasePreconstruccionModule)
      },
      {
        path: 'generarDisponibilidadPresupuestal',
        loadChildren: () => import('./_pages/generar-disponibilidad-presupuestal/generar-disponibilidad-presupuestal.module')
        .then(m => m.GenerarDisponibilidadPresupuestalModule)
      },
      {
        path: 'verificarPreconstruccion',
        loadChildren: () => import('./_pages/verificar-preconstruccion/verificar-preconstruccion.module')
        .then(m => m.VerificarPreconstruccionModule)
      },
      {
        path: 'aprobarPreconstruccion',
        loadChildren: () => import('./_pages/aprobar-preconstruccion/aprobar-preconstruccion.module')
        .then(m => m.AprobarPreconstruccionModule)
      },
      {
        path: 'generarActaInicioFaseIPreconstruccion',
        loadChildren: () => import('./_pages/generar-acta-inicio-f-uno-prc/generar-acta-inicio-f-uno-prc.module')
        .then(m => m.GenerarActaInicioFaseunoPreconstruccionModule)
      },
      {
        path: 'generarDisponibilidadPresupuestal',
        loadChildren: () => import('./_pages/generar-disponibilidad-presupuestal/generar-disponibilidad-presupuestal.module')
        .then(m => m.GenerarDisponibilidadPresupuestalModule)
      },
      {
        path: 'compromisosActasComite',
        loadChildren: () => import( './_pages/compromisos-actas-comite/compromisos-actas-comite.module' )
          .then( module => module.CompromisosActasComiteModule ),
      },
      {
        path: 'compromisosActasComite',
        loadChildren: () => import( './_pages/compromisos-actas-comite/compromisos-actas-comite.module' )
          .then( module => module.CompromisosActasComiteModule )
      },
	  {
        path: 'generarActaInicioConstruccion',
        loadChildren: () => import('./_pages/gestionar-acta-inicio-fdos-constr/gestionar-acta-inicio-fdos-constr.module')
        .then(m => m.GestionarActaInicioFdosConstrModule)
    },
      {
        path: 'registroSeguimientoDiario',
        loadChildren: () => import('./_pages/registro-seguimiento-diario/registro-seguimiento-diario.module')
        .then(m => m.RegistroSeguimientoDiarioModule),
      },
      {
        path: 'verificarSeguimientoDiario',
        loadChildren: () => import('./_pages/verificar-seguimiento-diario/verificar-seguimiento-diario.module')
        .then(m => m.VerificarSeguimientoDiarioModule),
      },
      {
        path: 'aprobarSeguimientoDiario',
        loadChildren: () => import('./_pages/aprobar-seguimiento-diario/aprobar-seguimiento-diario.module')
        .then(m => m.AprobarSeguimientoDiarioModule),
          
      },
      {
        path: 'gestionarTramiteControversiasContractuales',
        loadChildren: () => import( './_pages/gestionar-tramite-controversias-contractuales/gestionar-tramite-controversias-contractuales.module' )
          .then( module => module.GestionarTramiteControversiasContractualesModule ),

      },
      {
        path: 'registrarSolicitudNovedadContractual',
        loadChildren: () => import('./_pages/registrar-solicitud-novedad-contractual/registrar-solicitud-novedad-contractual.module')
        .then(m => m.RegistrarSolicitudNovedadContractualModule),
      },
	    {

        path: 'generarActaInicioConstruccion',
        loadChildren: () => import('./_pages/gestionar-acta-inicio-fdos-constr/gestionar-acta-inicio-fdos-constr.module')
        .then(m => m.GestionarActaInicioFdosConstrModule)
      },
      {
        path: 'programacionPersonalObra',
        loadChildren: () => import( './_pages/programacion-personal-obra/programacion-personal-obra.module' )
          .then( module => module.ProgramacionPersonalObraModule )
      },
      {
        path: 'registrarAvanceSemanal',
        loadChildren: () => import( './_pages/registrar-avance-semanal/registrar-avance-semanal.module' )
          .then( module => module.RegistrarAvanceSemanalModule )
      },
      {
        path: 'aprobarRequisitosTecnicosConstruccion',
        loadChildren: () => import( './_pages/aprobar-requisitos-construccion/aprobar-requisitos-construccion.module' )
          .then( module => module.AprobarRequisitosConstruccionModule )
      },
      {
        path: 'verificarRequisitosTecnicosConstruccion',
        loadChildren: () => import( './_pages/verificar-requisitos-construccion/verificar-requisitos-construccion.module' )
          .then( module => module.VerificarRequisitosConstruccionModule )
      },
      {
        path: 'verificarAvanceSemanal',
        loadChildren: () => import( './_pages/verificar-avance-semanal/verificar-avance-semanal.module' )
          .then( module => module.VerificarAvanceSemanalModule )
      },
      {
        path: 'requisitosTecnicosConstruccion',
        loadChildren: () => import( './_pages/requisitos-tecnicos-construccion/requisitos-tecnicos-construccion.module' )
          .then( module => module.RequisitosTecnicosConstruccionModule )
      },      
      {
        path: 'validarAvanceSemanal',
        loadChildren: () => import( './_pages/validar-avance-semanal/validar-avance-semanal.module' )
          .then( module => module.ValidarAvanceSemanalModule )
      },
      {
        path: 'requisitosTecnicosConstruccion',
        loadChildren: () => import( './_pages/requisitos-tecnicos-construccion/requisitos-tecnicos-construccion.module' )
          .then( module => module.RequisitosTecnicosConstruccionModule )
      }, 
      {
        path: 'verificarSolicitudDeNovedades',
        loadChildren: () => import('./_pages/verificar-solicitud-de-novedades/verificar-solicitud-de-novedades.module')
        .then(m => m.VerificarSolicitudDeNovedadesModule)
      },
      {
        path: 'validarSolicitudDeNovedades',
        loadChildren: () => import('./_pages/validar-solicitud-novedades/validar-solicitud-novedades.module')
        .then(m => m.ValidarSolicitudNovedadesModule)
      },
      {
        path: 'gestionarTramiteNovedadesContractualesAprobadas',
        loadChildren: () => import('./_pages/gestionar-novedades-aprobadas/gestionar-novedades-aprobadas.module')
        .then(m => m.GestionarNovedadesAprobadasModule)
      },
      {
        path: 'registratAjusteProgramacion',
        loadChildren: () => import('./_pages/registrar-ajuste-programacion/registrar-ajuste-programacion.module')
        .then(m => m.RegistrarAjusteProgramacionModule)
      },
      {
        path: 'validarAjusteProgramacion',
        loadChildren: () => import('./_pages/validar-ajuste-programacion/validar-ajuste-programacion.module')
        .then(m => m.ValidarAjusteProgramacionModule)
      },
      {
        path: 'cargarEnlaceMonitoreoEnLinea',
        loadChildren: () => import( './_pages/cargar-enlace-sistema-monitoreo-linea/cargar-enlace-sistema-monitoreo-linea.module' )
          .then( module => module.CargarEnlaceSistemaMonitoreoLineaModule )
      },      
      {    
        path: 'visualizarAvanceObraTiempoReal',
        loadChildren: () => import( './_pages/visualizar-avance-obra-tiempo-real/visualizar-avance-obra-tiempo-real.module' )
          .then( module => module.VisualizarAvanceObraTiempoRealModule )
      },
      {
        path: 'registrarActuacionesControversiasContractuales',
        loadChildren: () => import('./_pages/registrar-actuacion-controv-contrc/registrar-actuacion-controv-contrc.module')
        .then(m => m.RegistrarActuacionControvContrcModule)
      },
      {
        path: 'gestionarTramiteControversiasContractuales',
        loadChildren: () => import( './_pages/gestionar-tramite-controversias-contractuales/gestionar-tramite-controversias-contractuales.module' )
          .then( module => module.GestionarTramiteControversiasContractualesModule ),

      },
      {
        path: 'programacionPersonalObra',
        loadChildren: () => import( './_pages/programacion-personal-obra/programacion-personal-obra.module' )
          .then( module => module.ProgramacionPersonalObraModule )
      },
      {
        path: 'registrarAvanceSemanal',
        loadChildren: () => import( './_pages/registrar-avance-semanal/registrar-avance-semanal.module' )
          .then( module => module.RegistrarAvanceSemanalModule )
      },
      {
        path: 'registrarValidarRequisitosPago',
        loadChildren: () => import('./_pages/registrar-validar-requisitos-pago/registrar-validar-requisitos-pago.module')
        .then(m => m.RegistrarValidarRequisitosPagoModule)
      },
      {
        path: 'aprobarSolicitudesPago',
        loadChildren: () => import('./_pages/aprobar-solicitudes-pago/aprobar-solicitudes-pago.module')
        .then(m => m.AprobarSolicitudesPagoModule)
      },
      {
        path: 'autorizarSolicitudDePago',
        loadChildren: () => import('./_pages/autorizar-solicitud-pago/autorizar-solicitud-pago.module')
        .then(m => m.AutorizarSolicitudPagoModule)
      },      
      {
        path: 'verificarFinancieramenteSolicitudDePago',
        loadChildren: () => import('./_pages/verificar-financ-solicitud-pago/verificar-financ-solicitud-pago.module')
        .then(m => m.VerificarFinancSolicitudPagoModule)
      },
      {
        path: 'validarFinancieramenteSolicitudDePago',
        loadChildren: () => import('./_pages/validar-financ-solicitud-pago/validar-financ-solicitud-pago.module')
        .then(m => m.ValidarFinancSolicitudPagoModule)
      },
      {
        path: 'generarOrdenDeGiro',
        loadChildren: () => import('./_pages/generar-orden-giro/generar-orden-giro.module')
        .then(m => m.GenerarOrdenGiroModule)
      },
    ]

  },
  {
    path: '**',
    // redirectTo: '/inicio',
    loadChildren: () => import('./page-not-found/page-not-found.module').then(m => m.PageNotFoundModule)
  }
];
@NgModule({
  imports: [RouterModule.forRoot(routes, {
    preloadingStrategy: NoPreloading
  }),
  MatMomentDateModule,
  MatDatepickerModule],
  exports: [RouterModule],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'es'},
    AuthGuard, CanDeactivateGuard
  ]
})
export class AppRoutingModule { }
