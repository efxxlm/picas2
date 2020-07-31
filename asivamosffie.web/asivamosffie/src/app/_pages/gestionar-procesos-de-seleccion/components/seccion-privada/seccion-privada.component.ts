import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { forkJoin } from 'rxjs';
import { ProcesoSeleccionService, ProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { FormDescripcionDelProcesoDeSeleccionComponent } from '../form-descripcion-del-proceso-de-seleccion/form-descripcion-del-proceso-de-seleccion.component';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

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
                private procesoSeleccionService: ProcesoSeleccionService,
                private activatedRoute: ActivatedRoute,
                public dialog: MatDialog,    
                public router: Router      
             ) 
  { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe( parametro => {
      this.procesoSeleccion.tipoProcesoCodigo = parametro['tipoProceso'];
      this.procesoSeleccion.procesoSeleccionId = parametro['id'];

      if (this.procesoSeleccion.procesoSeleccionId > 0)
        this.editMode();

    })
  }

  editMode(){
    this.procesoSeleccionService.getProcesoSeleccionById( this.procesoSeleccion.procesoSeleccionId ).subscribe( proceso => {
      
      this.procesoSeleccion = proceso;
      console.log(this.procesoSeleccion);
      let botonDescripcion = document.getElementById('botonDescripcion');
      let botonEstudio = document.getElementById('botonEstudio')
      let botonProponente = document.getElementById('botonProponente')
      
      botonDescripcion.click();
      botonEstudio.click();
      botonProponente.click();

    })
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  onSubmit(){
    console.log(this.procesoSeleccion);
    this.procesoSeleccionService.guardarEditarProcesoSeleccion(this.procesoSeleccion).subscribe( respuesta => {
      // if ( respuesta.code == "200" )
      // {
      //   this.router.navigate([`/seleccion/seccionPrivada/${ this.procesoSeleccion.tipoProcesoCodigo }/${ this.procesoSeleccion.procesoSeleccionId }`])
      // }
      this.openDialog( "Proceso seleccion", respuesta.message )
      console.log('respuesta',  respuesta );
    })
  }

  
}
