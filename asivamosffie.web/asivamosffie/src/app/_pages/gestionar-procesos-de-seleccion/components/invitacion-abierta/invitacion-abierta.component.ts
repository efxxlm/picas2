import { Component, OnInit } from '@angular/core';
import { ProcesoSeleccion, ProcesoSeleccionService, TiposProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-invitacion-abierta',
  templateUrl: './invitacion-abierta.component.html',
  styleUrls: ['./invitacion-abierta.component.scss']
})
export class InvitacionAbiertaComponent implements OnInit {

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
                private activatedRoute: ActivatedRoute,
                private procesoSeleccionService: ProcesoSeleccionService,
                public dialog: MatDialog,    
             )
{ }

  ngOnInit() {
    this.activatedRoute.params.subscribe( async parametro => {

      this.procesoSeleccion.procesoSeleccionId = parametro['id'];
      this.procesoSeleccion.tipoProcesoCodigo = this.tiposProcesoSeleccion.Abierta;

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

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  async editMode(){
    
    this.cargarRegistro().then(() => 
    { 

        let botonDescripcion = document.getElementById('botonDescripcion');
        let botonevaluacion = document.getElementById('botonevaluacion')
        
        botonDescripcion.click();
        botonevaluacion.click();
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
