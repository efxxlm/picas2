import { CambiarContrasenaModule } from './_pages/cambiar-contrasena/cambiar-contrasena.module';
import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules, PreloadingStrategy } from '@angular/router';

import { LayoutComponent } from './layout/layout.component';

import { AuthGuard } from './_guards/auth.guard';

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
        loadChildren: () => import('./_pages/cargar-masivamente-proyectos-viabilizados/cargar-masivamente-proyectos-viabilizados.module').then(m => m.CargarMasivamenteProyectosViabilizadosModule)
      },
    ]

  },
  {
    path: '**',
    redirectTo: '/inicio',
    // loadChildren: () => import('./page-not-found/page-not-found.module').then(m => m.PageNotFoundModule)
  }
];
@NgModule({
  imports: [RouterModule.forRoot(routes, {
    preloadingStrategy: PreloadAllModules
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
