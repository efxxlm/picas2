import { Component, OnInit } from '@angular/core';
import { ProcesoSeleccion, ProcesoSeleccionService, TiposProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-invitacion-cerrada',
  templateUrl: './invitacion-cerrada.component.html',
  styleUrls: ['./invitacion-cerrada.component.scss']
})
export class InvitacionCerradaComponent implements OnInit {

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
                private procesoSeleccionService: ProcesoSeleccionService,
                private dialog: MatDialog,    
                private router: Router,
                private activatedRoute: ActivatedRoute 
                         
  ) { }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe( async parametro => {

      this.procesoSeleccion.procesoSeleccionId = parametro['id'];
      this.procesoSeleccion.tipoProcesoCodigo = this.tiposProcesoSeleccion.Cerrada;

      if (this.procesoSeleccion.procesoSeleccionId > 0)
        this.editMode();

    })

  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
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
        let botonevaluacion = document.getElementById('botonevaluacion')
        let botonProponenteInvitar = document.getElementById('botonProponenteInvitar')
        
        
        botonDescripcion.click();
        botonEstudio.click();
        botonProponente.click();
        botonevaluacion.click();
        botonProponenteInvitar.click();
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
      this.router.navigate(["/seleccion/invitacionCerrada", respuesta.data.procesoSeleccionId])
      console.log('respuesta',  respuesta );
    })
  }

}
