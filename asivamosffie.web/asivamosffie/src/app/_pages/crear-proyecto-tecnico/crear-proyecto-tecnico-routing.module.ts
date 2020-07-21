import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FormularioProyectosComponent } from './components/formulario-proyectos/formulario-proyectos.component';
import { CrearProyectoTenicoComponent } from './components/crear-proyecto-tenico/crear-proyecto-tenico.component';


const routes: Routes = [{
  path: 'crearProyecto',
  component: FormularioProyectosComponent
},
{
  path: '',
  component: CrearProyectoTenicoComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CrearProyectoTecnicoRoutingModule { }
