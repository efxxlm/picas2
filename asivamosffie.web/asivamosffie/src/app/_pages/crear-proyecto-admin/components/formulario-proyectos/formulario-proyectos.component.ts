import { Component } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { FuenteFinanciacion, Aportante, ProyectoAdministrativo, Listados } from 'src/app/core/_services/project/project.service';

@Component({
  selector: 'app-formulario-proyectos',
  templateUrl: './formulario-proyectos.component.html',
  styleUrls: ['./formulario-proyectos.component.scss']
})
export class FormularioProyectosComponent {


  proyectoAdmin:ProyectoAdministrativo;
  listadoAportantes:Listados[];
  listadoFuentes:Listados[];

  addFont(aportante:Aportante) {
    aportante.FuenteFinanciacion.push({ValorFuente:0,FuenteRecursosCodigo:""});
  }

  ngOnInit()
  {
    this.proyectoAdmin={identificador:"0001",Aportante:[{AportanteId:0,FuenteFinanciacion:[{ValorFuente:0,FuenteRecursosCodigo:""}]}]};    
    this.listadoAportantes=[{id:"001",valor:"valor1"},{id:"002",valor:"valor2"}];
    this.listadoFuentes=[{id:"001",valor:"valor1"},{id:"002",valor:"valor2"}];
    
  }
  
  addAportant()
  {       
    this.proyectoAdmin.Aportante.push({AportanteId:0,FuenteFinanciacion:[{ValorFuente:0,FuenteRecursosCodigo:""}]});
  }


  constructor(private fb: FormBuilder) {}

  onSubmit() {
    alert('Thanks!');
  }
}
