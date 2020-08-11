import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FormularioProyectosComponent } from './components/formulario-proyectos/formulario-proyectos.component';
import { CrearProyectoAdminModule } from './crear-proyecto-admin.module';
import { CrearProyectoAdminComponent } from './components/crear-proyecto-admin/crear-proyecto-admin.component';


const routes: Routes = [{
  path: 'crearProyecto',
  component: FormularioProyectosComponent
},
{
  path: '',
  component: CrearProyectoAdminComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CrearProyectoAdminRoutingModule { }
