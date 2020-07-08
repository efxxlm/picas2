import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Listados, Proyecto } from 'src/app/core/_services/project/project.service';

@Component({
  selector: 'app-formulario-proyectos',
  templateUrl: './formulario-proyectos.component.html',
  styleUrls: ['./formulario-proyectos.component.scss']
})
export class FormularioProyectosComponent {
    constructor(private fb: FormBuilder) {}

    listadoTipoIntervencion:Listados[];
    listadoregion:Listados[];
    listadoDepartamento:Listados[];
    listadoMunicipio:Listados[];
    listadoInstitucion:Listados[];
    listadoSede:Listados[];
    listadoConvocatoria:Listados[];
    proyecto:Proyecto=new Proyecto();
    CodigoDaneIE:string="";
    codigoDaneSede:string="";
  onSubmit() {
    alert('Thanks!');
  }

  addInfraestructura()
  {
    
  }
}
