import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { forkJoin } from 'rxjs';
import { ProcesoSeleccionService, ProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { FormDescripcionDelProcesoDeSeleccionComponent } from '../form-descripcion-del-proceso-de-seleccion/form-descripcion-del-proceso-de-seleccion.component';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-seccion-privada',
  templateUrl: './seccion-privada.component.html',
  styleUrls: ['./seccion-privada.component.scss']
})
export class SeccionPrivadaComponent implements OnInit {

  listaTipoIntervencion: Dominio[] = [];

  procesoSeleccion: ProcesoSeleccion = {
    alcanceParticular: '',
    criteriosSeleccion: '',
    esDistribucionGrupos:false,
    justificacion: '',
    numeroProceso: '',
    objeto: '',
    procesoSeleccionId: 0,
    tipoAlcanceCodigo: '',
    tipoIntervencionCodigo: '',
    tipoProcesoCodigo: '',
    procesoSeleccionGrupo: [],
    procesoSeleccionCronograma: [],

  };

  constructor(
                private commonService: CommonService,
                private fb: FormBuilder,      
                private procesoSeleccionService: ProcesoSeleccionService          
             ) 
  { }

  ngOnInit(): void {

  }

  onSubmit(){
    this.procesoSeleccionService.guardarEditarProcesoSeleccion(this.procesoSeleccion).subscribe( respuesta => {
      console.log( respuesta );
    })
  }

}
