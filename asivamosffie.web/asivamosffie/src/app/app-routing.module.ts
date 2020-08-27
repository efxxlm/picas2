import { GestionarAcuerdoCofinanciacionRoutingModule } from './_pages/gestionar-acuerdo-cofinanciacion/gestionar-acuerdo-cofinanciacion-routing.module';
import { CambiarContrasenaModule } from './_pages/cambiar-contrasena/cambiar-contrasena.module';
import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules, PreloadingStrategy } from '@angular/router';
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

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: '',
        redirectTo: '/inicio',
        pathMatch: 'full',
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
        path: 'cargarMasivamente',
        // tslint:disable-next-line: max-line-length
        loadChildren: () => import('./_pages/cargar-masivamente-proyectos-viabilizados/cargar-masivamente-proyectos-viabilizados.module').then(m => m.CargarMasivamenteProyectosViabilizadosModule)
      },
      {
        path: 'gestionarFuentes',
        loadChildren: () => import('./_pages/gestionar-fuentes-de-financiacion/gestionar-fuentes-de-financiacion.module')
        .then(m => m.GestionarFuentesDeFinanciacionModule)
      },
      {
        path: 'registrarFuentes/:idTipoAportante/:idAportante',
        component: RegistrarComponent,
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
        path: 'solicitarContratacion',
        loadChildren: () => import('./_pages/solicitar-contratacion/solicitar-contratacion.module')
        .then(m => m.SolicitarContratacionModule)
      },
      {
        path: 'generarDisponibilidadPresupuestal',
        loadChildren: () => import('./_pages/generar-disponibilidad-presupuestal/generar-disponibilidad-presupuestal.module')
        .then(m => m.GenerarDisponibilidadPresupuestalModule)
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
    preloadingStrategy: PreloadAllModules
  }),
  MatMomentDateModule,
  MatDatepickerModule],
  exports: [RouterModule],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'es'}
  ]
})
export class AppRoutingModule { }
