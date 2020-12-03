import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProgramacionPersonalObraComponent } from './components/programacion-personal-obra/programacion-personal-obra.component';


const routes: Routes = [
  {
    path: '',
    component: ProgramacionPersonalObraComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProgramacionPersonalObraRoutingModule { }
