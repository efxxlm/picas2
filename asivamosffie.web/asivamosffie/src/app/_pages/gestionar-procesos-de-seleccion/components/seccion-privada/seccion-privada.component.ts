import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { forkJoin } from 'rxjs';
import { ProcesoSeleccionService, ProcesoSeleccion, TiposProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
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
  tiposProcesoSeleccion = TiposProcesoSeleccion; 
  
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

  ngOnInit() {
    this.activatedRoute.params.subscribe( async parametro => {
      //this.procesoSeleccion.tipoProcesoCodigo = parametro['tipoProceso'];
      this.procesoSeleccion.procesoSeleccionId = parametro['id'];
      this.procesoSeleccion.tipoProcesoCodigo = this.tiposProcesoSeleccion.Privada;

      if (this.procesoSeleccion.procesoSeleccionId > 0)
        this.editMode();

    })
  }

  async cargarRegistro(){
    
    return new Promise( resolve => {

      forkJoin([
        this.procesoSeleccionService.getProcesoSeleccionById( this.procesoSeleccion.procesoSeleccionId )
      ]).subscribe( proceso => {
          this.procesoSeleccion = proceso[0];
          setTimeout(() => { resolve(); },1000)
      });
    });

  }

  async editMode(){
    
    
    this.cargarRegistro().then(() => 
    { 

        let botonDescripcion = document.getElementById('botonDescripcion');
        let botonEstudio = document.getElementById('botonEstudio')
        let botonProponente = document.getElementById('botonProponente')
        
        botonDescripcion.click();
        botonEstudio.click();
        botonProponente.click();
    });

  }

  openDialog(modalTitle: string, modalText: string, redirect?:boolean, id?:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
    
    if ( redirect )
    {
      dialogRef.afterClosed().subscribe(result => {
        if(result)
        {
          this.router.navigate([`/seleccion/seccionPrivada/${ id }`])
        }
      });
    }
  }

  onSubmit(){
    console.log(this.procesoSeleccion);
    this.procesoSeleccionService.guardarEditarProcesoSeleccion(this.procesoSeleccion).subscribe( respuesta => {
      this.openDialog( "", respuesta.message,respuesta.code == "200",respuesta.data.procesoSeleccionId )

      
      //console.log('respuesta',  respuesta );
    },
    error => {
      this.openDialog( "", error )
      console.log('respuesta',  error );
    },
     () => {})
  }

  estaIncompleto(objeto){
    return true;
  }
}
