import { Component, OnInit } from '@angular/core';
import { ProcesoSeleccion, ProcesoSeleccionService } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-invitacion-cerrada',
  templateUrl: './invitacion-cerrada.component.html',
  styleUrls: ['./invitacion-cerrada.component.scss']
})
export class InvitacionCerradaComponent implements OnInit {

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
                private procesoSeleccionService: ProcesoSeleccionService,
                public dialog: MatDialog,    
                public router: Router,
                         
  ) { }

  ngOnInit(): void {
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
