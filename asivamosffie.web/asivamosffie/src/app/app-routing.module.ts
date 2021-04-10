import { ActualUserGuard } from './_guards/actual-user.guard';
import { GestionarAcuerdoCofinanciacionRoutingModule } from './_pages/gestionar-acuerdo-cofinanciacion/gestionar-acuerdo-cofinanciacion-routing.module';
import { CambiarContrasenaModule } from './_pages/cambiar-contrasena/cambiar-contrasena.module';
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
import { LayoutReportesComponent } from './layout-reportes/layout-reportes.component';

import { AuthGuard } from './_guards/auth.guard';
import { RegistrarAcuerdoComponent } from './_pages/gestionar-acuerdo-cofinanciacion/components/registrar-acuerdo/registrar-acuerdo.component';
import { RegistrarComponent } from './_pages/gestionar-fuentes-de-financiacion/components/registrar/registrar.component';
import { CanDeactivateGuard } from './_guards/can-deactivate.guard';
import { AuthenticateGuard } from './_guards/authenticate.guard';

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
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/home/home.module').then(m => m.HomeModule),

      },
      {
        path: 'cambiarContrasena',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/cambiar-contrasena/cambiar-contrasena.module').then(m => m.CambiarContrasenaModule),

      },
      //verificar ruta duplicada
      {
        path: 'cargarMasivamente',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/cargar-masivamente-proyectos-viabilizados/cargar-masivamente-proyectos-viabilizados.module')
          .then(m => m.CargarMasivamenteProyectosViabilizadosModule),

      },
      {
        path: 'gestionarAcuerdos',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/gestionar-acuerdo-cofinanciacion/gestionar-acuerdo-cofinanciacion.module')
        .then(m => m.GestionarAcuerdoCofinanciacionModule),

      },
      {
        path: 'crearProyecto',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/crear-proyecto-tecnico/crear-proyecto-tecnico.module')
          .then(m => m.CrearProyectoTecnicoModule),

      }
      ,
      {
        path: 'crearProyectoAdministrativo',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/crear-proyecto-admin/crear-proyecto-admin.module')
          .then(m => m.CrearProyectoAdminModule),

      }
      ,
      {
        path: 'gestionarFuentes',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/gestionar-fuentes-de-financiacion/gestionar-fuentes-de-financiacion.module')
        .then(m => m.GestionarFuentesDeFinanciacionModule),

      },
      {
        path: 'registrarAcuerdos/:id',
        component: RegistrarAcuerdoComponent,

      },
      {
        path: 'cargarMasivamente',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/cargar-masivamente-proyectos-viabilizados/cargar-masivamente-proyectos-viabilizados.module')
          .then(m => m.CargarMasivamenteProyectosViabilizadosModule)
      },
      {
        path: 'gestionarFuentes',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/gestionar-fuentes-de-financiacion/gestionar-fuentes-de-financiacion.module')
        .then(m => m.GestionarFuentesDeFinanciacionModule)

      },
      {
        path: 'generarRegistroPresupuestal',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/generar-registro-presupuestal/generar-registro-presupuestal.module')
        .then(m => m.GenerarRegistroPresupuestalModule)
      },
      {
        path: 'generarActaInicioFaseIPreconstruccion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/generar-acta-inicio-f-uno-prc/generar-acta-inicio-f-uno-prc.module')
        .then(m => m.GenerarActaInicioFaseunoPreconstruccionModule)
      },
      {
        path: 'generarActaInicioFaseIPreconstruccion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/generar-acta-inicio-f-uno-prc/generar-acta-inicio-f-uno-prc.module')
        .then(m => m.GenerarActaInicioFaseunoPreconstruccionModule)
      },
      {
        path: 'registrarFuentes/:idTipoAportante/:idAportante',
        component: RegistrarComponent,

      },
      {
        path: 'solicitarDisponibilidadPresupuestal',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/solicitar-disponibilidad-presupuestal/solicitar-disponibilidad-presupuestal.module')
          .then(m => m.SolicitarDisponibilidadPresupuestalModule),
      },
      {
        path: 'validarDisponibilidadPresupuesto',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/validar-disponibilidad-presupuesto/validar-disponibilidad-presupuesto.module')
        .then(m => m.ValidarDisponibilidadPresupuestoModule),

      },
      {
        path: 'comiteTecnico',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/comite-tecnico/comite-tecnico.module').then(m => m.ComiteTecnicoModule),

      },
      {
        path: 'seleccion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/gestionar-procesos-de-seleccion/gestionar-procesos-de-seleccion.module')
        .then(m => m.GestionarProcesosDeSeleccionModule),


      },
      {
        path: 'contratosModificacionesContractuales',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.module' )
          .then( module => module.ContratosModificacionesContractualesModule ),

      },
      {
        path: 'generarPolizasYGarantias',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/generar-polizas-y-garantias/generar-polizas-y-garantias.module')
          .then(m => m.GenerarPolizasYGarantiasModule),

      },
      {
        path: 'comiteFiduciario',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/sesion-comite-fiduciario/comite-fiduciario.module' )
          .then( module => module.ComiteFiduciarioModule ),

      },
      {
 
        path: 'procesosContractuales',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/gestionar-procesos-contractuales/gestionar-procesos-contractuales.module' )
          .then( module => module.GestionarProcesosContractualesModule ),

      },
      {
        path: 'solicitarContratacion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/solicitar-contratacion/solicitar-contratacion.module')
        .then(m => m.SolicitarContratacionModule)
      },
      {
        path: 'preconstruccion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/fase-preconstruccion/fase-preconstruccion.module')
        .then(m => m.FasePreconstruccionModule)
      },
      {
        path: 'generarDisponibilidadPresupuestal',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/generar-disponibilidad-presupuestal/generar-disponibilidad-presupuestal.module')
        .then(m => m.GenerarDisponibilidadPresupuestalModule)
      },
      {
        path: 'verificarPreconstruccion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/verificar-preconstruccion/verificar-preconstruccion.module')
        .then(m => m.VerificarPreconstruccionModule)
      },
      {
        path: 'aprobarPreconstruccion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/aprobar-preconstruccion/aprobar-preconstruccion.module')
        .then(m => m.AprobarPreconstruccionModule)
      },
      {
        path: 'generarActaInicioFaseIPreconstruccion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/generar-acta-inicio-f-uno-prc/generar-acta-inicio-f-uno-prc.module')
        .then(m => m.GenerarActaInicioFaseunoPreconstruccionModule)
      },
      {
        path: 'generarDisponibilidadPresupuestal',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/generar-disponibilidad-presupuestal/generar-disponibilidad-presupuestal.module')
        .then(m => m.GenerarDisponibilidadPresupuestalModule)
      },
      {
        path: 'compromisosActasComite',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/compromisos-actas-comite/compromisos-actas-comite.module' )
          .then( module => module.CompromisosActasComiteModule ),
      },
      {
        path: 'compromisosActasComite',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/compromisos-actas-comite/compromisos-actas-comite.module' )
          .then( module => module.CompromisosActasComiteModule )
      },
	  {
        path: 'generarActaInicioConstruccion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/gestionar-acta-inicio-fdos-constr/gestionar-acta-inicio-fdos-constr.module')
        .then(m => m.GestionarActaInicioFdosConstrModule)
    },
      {
        path: 'registroSeguimientoDiario',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/registro-seguimiento-diario/registro-seguimiento-diario.module')
        .then(m => m.RegistroSeguimientoDiarioModule),
      },
      {
        path: 'verificarSeguimientoDiario',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/verificar-seguimiento-diario/verificar-seguimiento-diario.module')
        .then(m => m.VerificarSeguimientoDiarioModule),
      },
      {
        path: 'aprobarSeguimientoDiario',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/aprobar-seguimiento-diario/aprobar-seguimiento-diario.module')
        .then(m => m.AprobarSeguimientoDiarioModule),
          
      },
      {
        path: 'gestionarTramiteControversiasContractuales',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/gestionar-tramite-controversias-contractuales/gestionar-tramite-controversias-contractuales.module' )
          .then( module => module.GestionarTramiteControversiasContractualesModule ),

      },
      {
        path: 'registrarSolicitudNovedadContractual',
        canLoad: [ AuthenticateGuard, ActualUserGuard ],
        loadChildren: () => import('./_pages/registrar-solicitud-novedad-contractual/registrar-solicitud-novedad-contractual.module')
        .then(m => m.RegistrarSolicitudNovedadContractualModule),
      },
	    {

        path: 'generarActaInicioConstruccion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/gestionar-acta-inicio-fdos-constr/gestionar-acta-inicio-fdos-constr.module')
        .then(m => m.GestionarActaInicioFdosConstrModule)
      },
      {
        path: 'programacionPersonalObra',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/programacion-personal-obra/programacion-personal-obra.module' )
          .then( module => module.ProgramacionPersonalObraModule )
      },
      {
        path: 'registrarAvanceSemanal',
        canLoad: [ AuthenticateGuard, ActualUserGuard ],
        loadChildren: () => import( './_pages/registrar-avance-semanal/registrar-avance-semanal.module' )
          .then( module => module.RegistrarAvanceSemanalModule )
      },
      {
        path: 'aprobarRequisitosTecnicosConstruccion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/aprobar-requisitos-construccion/aprobar-requisitos-construccion.module' )
          .then( module => module.AprobarRequisitosConstruccionModule )
 
      },
      {
        path: 'verificarRequisitosTecnicosConstruccion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/verificar-requisitos-construccion/verificar-requisitos-construccion.module' )
          .then( module => module.VerificarRequisitosConstruccionModule )
      },
      {
        path: 'verificarAvanceSemanal',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/verificar-avance-semanal/verificar-avance-semanal.module' )
          .then( module => module.VerificarAvanceSemanalModule )
      },
      {
        path: 'requisitosTecnicosConstruccion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/requisitos-tecnicos-construccion/requisitos-tecnicos-construccion.module' )
          .then( module => module.RequisitosTecnicosConstruccionModule )
      },      
      {
        path: 'validarAvanceSemanal',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/validar-avance-semanal/validar-avance-semanal.module' )
          .then( module => module.ValidarAvanceSemanalModule )
      },
      {
        path: 'requisitosTecnicosConstruccion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/requisitos-tecnicos-construccion/requisitos-tecnicos-construccion.module' )
          .then( module => module.RequisitosTecnicosConstruccionModule )
      }, 
      {
        path: 'registrarInformeFinalProyecto',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/registrar-informe-final-proyecto/registrar-informe-final-proyecto.module' )
          .then( module => module.RegistrarInformeFinalProyectoModule )
      },
      {
        path: 'verificarInformeFinalProyecto',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/verificar-informe-final-proyecto/verificar-informe-final-proyecto.module' )
          .then( module => module.VerificarInformeFinalProyectoModule )
      },
      {
        path: 'validarInformeFinalProyecto',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/validar-informe-del-proyecto/validar-informe-del-proyecto.module' )
          .then( module => module.ValidarInformeDelProyectoModule )
      },
      {
        path: 'registrarTransferenciaProyectosETC',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/registrar-transferencia-etc/registrar-transferencia-etc.module' )
          .then( module => module.RegistrarTransferenciaEtcModule )
      },
      {
        path: 'registrarValidarDolicitudLiquidacionContractual',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/registrar-validar-solicitud-liquidacion-contractual/registrar-validar-solicitud-liquidacion-contractual.module' )
          .then( module => module.RegistrarValidarSolicitudLiquidacionContractualModule )
      },
      {
        path: 'registrarSolicitudLiquidacionContractual',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/registrar-solicitud-liquidacion-contractual/registrar-solicitud-liquidacion-contractual.module' )
          .then( module => module.RegistrarSolicitudLiquidacionContractualModule )
      },
      {
        path: 'aprobarSolicitudLiquidacionContractual',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/aprobar-solicitud-liquidacion-contractual/aprobar-solicitud-liquidacion-contractual.module' )
          .then( module => module.AprobarSolicitudLiquidacionContractualModule )
      },
      {
        path: 'validarCumplimientoInformeFinalProyecto',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/validar-cumplimiento-informe-final-proyecto/validar-cumplimiento-informe-final-proyecto.module' )
          .then( module => module.ValidarCumplimientoInformeFinalProyectoModule )
      },
    { 
        path: 'verificarSolicitudDeNovedades',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/verificar-solicitud-de-novedades/verificar-solicitud-de-novedades.module')
        .then(m => m.VerificarSolicitudDeNovedadesModule)
      },
      {
        path: 'validarSolicitudDeNovedades',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/validar-solicitud-novedades/validar-solicitud-novedades.module')
        .then(m => m.ValidarSolicitudNovedadesModule)
      },
      {
        path: 'gestionarTramiteNovedadesContractualesAprobadas',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/gestionar-novedades-aprobadas/gestionar-novedades-aprobadas.module')
        .then(m => m.GestionarNovedadesAprobadasModule)
      },
      {
        path: 'registratAjusteProgramacion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/registrar-ajuste-programacion/registrar-ajuste-programacion.module')
        .then(m => m.RegistrarAjusteProgramacionModule)
      },
      {
        path: 'validarAjusteProgramacion',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/validar-ajuste-programacion/validar-ajuste-programacion.module')
        .then(m => m.ValidarAjusteProgramacionModule)
      },
      {
        path: 'cargarEnlaceMonitoreoEnLinea',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/cargar-enlace-sistema-monitoreo-linea/cargar-enlace-sistema-monitoreo-linea.module' )
          .then( module => module.CargarEnlaceSistemaMonitoreoLineaModule )
      },      
      {    
        path: 'visualizarAvanceObraTiempoReal',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/visualizar-avance-obra-tiempo-real/visualizar-avance-obra-tiempo-real.module' )
          .then( module => module.VisualizarAvanceObraTiempoRealModule )
      },
      {
        path: 'registrarActuacionesControversiasContractuales',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/registrar-actuacion-controv-contrc/registrar-actuacion-controv-contrc.module')
        .then(m => m.RegistrarActuacionControvContrcModule)
      },
      {
        path: 'gestionarTramiteControversiasContractuales',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/gestionar-tramite-controversias-contractuales/gestionar-tramite-controversias-contractuales.module' )
          .then( module => module.GestionarTramiteControversiasContractualesModule ),

      },
      {
        path: 'programacionPersonalObra',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/programacion-personal-obra/programacion-personal-obra.module' )
          .then( module => module.ProgramacionPersonalObraModule )
      },
      {
        path: 'registrarAvanceSemanal',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/registrar-avance-semanal/registrar-avance-semanal.module' )
          .then( module => module.RegistrarAvanceSemanalModule )
      },
      {
        path: 'registrarValidarRequisitosPago',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/registrar-validar-requisitos-pago/registrar-validar-requisitos-pago.module')
        .then(m => m.RegistrarValidarRequisitosPagoModule)
      },
      {
        path: 'verificarSolicitudPago',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/aprobar-solicitudes-pago/aprobar-solicitudes-pago.module')
        .then(m => m.AprobarSolicitudesPagoModule)
      },
      {
        path: 'autorizarSolicitudPago',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/autorizar-solicitud-pago/autorizar-solicitud-pago.module')
        .then(m => m.AutorizarSolicitudPagoModule)
      },      
      {
        path: 'verificarFinancieramenteSolicitudDePago',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/verificar-financ-solicitud-pago/verificar-financ-solicitud-pago.module')
        .then(m => m.VerificarFinancSolicitudPagoModule)
      },
      {
        path: 'validarFinancieramenteSolicitudDePago',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/validar-financ-solicitud-pago/validar-financ-solicitud-pago.module')
        .then(m => m.ValidarFinancSolicitudPagoModule)
      },
      {
        path: 'generarOrdenDeGiro',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/generar-orden-giro/generar-orden-giro.module')
        .then(m => m.GenerarOrdenGiroModule)
      },
      {
        path: 'verificarOrdenGiro',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/verificar-orden-giro/verificar-orden-giro.module' )
          .then( module => module.VerificarOrdenGiroModule )
      },
      {
        path: 'aprobarOrdenGiro',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/aprobar-orden-giro/aprobar-orden-giro.module' )
          .then( module => module.AprobarOrdenGiroModule )
      },
      {
        path: 'tramitarOrdenGiro',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/tramitar-orden-giro/tramitar-orden-giro.module' )
          .then( module => module.TramitarOrdenGiroModule )
      },
      {
        path: 'gestionarProcesoDefensaJudicial',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/gestionar-procesos-defensa-judicial/gestionar-procesos-defensa-judicial.module')
        .then(m => m.GestionarProcesosDefensaJudicialModule)
      },
      {
        path: 'registrarPagosRendimientos',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/registrar-pagos-rendimientos/registrar-pagos-rendimientos.module')
        .then(m => m.RegistrarPagosRendimientosModule)
      },
      {
        path: 'gestionarRendimientos',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/gestionar-rendimientos/gestionar-rendimientos.module')
        .then(m => m.GestionarRendimientosModule)
      },
      {
        path: 'aprobarIncorporacionRendimientos',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/aprobar-incorporacion-rendimientos/aprobar-incorporacion-rendimientos.module')
        .then(m => m.AprobarIncorporacionRendimientosModule)
      },
      {
        path: 'gestionarBalanceFinancieroTrasladoRecursos',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/gestionar-balan-financ-trasl-recursos/gestionar-balan-financ-trasl-recursos.module')
        .then(m => m.GestionarBalanFinancTraslRecursosModule)
      },
      {
        path: 'registrarActualizacionesPolizasYGarantias',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/registrar-actualiz-polizas-garantias/registrar-actualiz-polizas-garantias.module')
        .then(m => m.RegistrarActualizPolizasGarantiasModule)
      },
      {
        path: 'registrarLiquidacionContrato',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/registrar-liquidacion-contrato/registrar-liquidacion-contrato.module')
        .then(m => m.RegistrarLiquidacionContratoModule)
      },
      {
        path: 'gestionarTramiteLiquidacionContractual',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/gestionar-tramite-liq-contractual/gestionar-tramite-liq-contractual.module')
        .then(m => m.GestionarTramiteLiqContractualModule)
      },
      {
        path: 'gestionListaChequeo',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/gestionar-lista-chequeo/gestionar-lista-chequeo.module' )
          .then( module => module.GestionarListaChequeoModule )
      },
      {
        path: 'crearRoles',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/crear-roles/crear-roles.module' )
          .then( module => module.CrearRolesModule )
      },
      {
        path: 'gestionUsuarios',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/gestionar-usuarios/gestionar-usuarios.module' )
          .then( module => module.GestionarUsuariosModule )
      },
      {
        path: 'gestionParametricas',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import( './_pages/gestionar-parametricas/gestionar-parametricas.module' )
          .then( module => module.GestionarParametricasModule )
      },
      {
        path: 'menu',
        canLoad: [ ActualUserGuard ],
        loadChildren: () => import('./_pages/menu/menu.module').then(m => m.MenuModule)
      },
    ]
    
  },
  {
    path: 'reportes',
    component: LayoutReportesComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('./_pages/reportes/reportes.module').then(m => m.ReportesModule)
      }
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
