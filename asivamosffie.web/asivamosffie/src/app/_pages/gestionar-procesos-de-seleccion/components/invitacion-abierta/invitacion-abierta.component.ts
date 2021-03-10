import { Component, OnInit } from '@angular/core';
import { EstadosProcesoSeleccion, ProcesoSeleccion, ProcesoSeleccionService, TiposProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-invitacion-abierta',
  templateUrl: './invitacion-abierta.component.html',
  styleUrls: ['./invitacion-abierta.component.scss']
})
export class InvitacionAbiertaComponent implements OnInit {

  /*con este bit controlo los botones, esto lo hago ya sea por el estado del proyecto o en un futuro por el 
    permiso que tenga el usuario
    */
  bitPuedoEditar=true;
  tiposProcesoSeleccion = TiposProcesoSeleccion; 
  estadosProcesoSeleccion = EstadosProcesoSeleccion;

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

  descripcion_class: number=0;
  orden_class: number=3;//asumo que en creación no tiene el estado necesario
  evaluacion_class:number=3;//asumo que en creación no tiene el estado necesario
  constructor(
                private activatedRoute: ActivatedRoute,
                private procesoSeleccionService: ProcesoSeleccionService,
                public dialog: MatDialog, 
                private router: Router,   
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
          this.descripcion_class=this.estaIncompletoDescripcion(this.procesoSeleccion);
          this.evaluacion_class=this.estaIncompletoEvaluacion(this.procesoSeleccion);
          this.orden_class=this.estaIncompletoOrden(this.procesoSeleccion);
          
          setTimeout(() => { resolve(); },1000)
      });
    });

  }

  openDialog(modalTitle: string, modalText: string,id:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
    dialogRef.afterClosed().subscribe(result => {
      this.router.navigate(["/seleccion/invitacionAbierta", id]);
        setTimeout(() => {
          location.reload();  
        }, 1000);
    });
  }

  async editMode(){
    
    this.cargarRegistro().then(() => 
    { 

        let botonDescripcion = document.getElementById('botonDescripcion');
        let botonevaluacion = document.getElementById('botonevaluacion')
        
        botonDescripcion.click();
        botonevaluacion.click();

        if(this.procesoSeleccion.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.Creado||
          this.procesoSeleccion.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario||
          this.procesoSeleccion.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.DevueltaAperturaPorComiteFiduciario||
          this.procesoSeleccion.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.DevueltaAperturaPorComiteTecnico ||
          this.procesoSeleccion.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.DevueltaSeleccionPorComiteFiduciario ||
          this.procesoSeleccion.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.DevueltaSeleccionPorComiteTecnico ||
          this.procesoSeleccion.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.DevueltoPorComiteFiduciario ||
          this.procesoSeleccion.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.DevueltoPorComiteTecnico)
        {
          this.bitPuedoEditar=true;
        }
        else{
          this.bitPuedoEditar=false;
        }
    });

  }

  onSubmit(){
    console.log(this.procesoSeleccion);
    this.procesoSeleccionService.guardarEditarProcesoSeleccion(this.procesoSeleccion).subscribe( respuesta => {
      // if ( respuesta.code == "200" )
      // {
      //   this.router.navigate([`/seleccion/seccionPrivada/${ this.procesoSeleccion.tipoProcesoCodigo }/${ this.procesoSeleccion.procesoSeleccionId }`])
      // }
      this.openDialog( "", respuesta.message,respuesta.data.procesoSeleccionId );
      
      console.log('respuesta',  respuesta );
    })
  }

  estaIncompletoDescripcion(pProceso:ProcesoSeleccion):number{        
  
    
    console.log(pProceso);

    let retorno: number = 0; //sin-diligenciar

    // verifico si hay algo registrado
    if (
      (pProceso.objeto !== "" && pProceso.objeto !== undefined) ||
      (pProceso.alcanceParticular !== "" && pProceso.alcanceParticular !== undefined) ||
      (pProceso.justificacion !== "" && pProceso.justificacion !== undefined) ||
      //(pProceso.criteriosSeleccion !== "" && pProceso.criteriosSeleccion !== undefined) ||
      (pProceso.tipoIntervencionCodigo !== "" && pProceso.tipoIntervencionCodigo !== undefined) ||
      (pProceso.tipoAlcanceCodigo !== "" && pProceso.tipoAlcanceCodigo !== undefined) ||
      (pProceso.esDistribucionGrupos !== undefined) ||
      (pProceso.responsableEstructuradorUsuarioid !== undefined) ||
      (pProceso.responsableTecnicoUsuarioId !== undefined) ||
      (pProceso.condicionesAsignacionPuntaje !== undefined && pProceso.condicionesAsignacionPuntaje !== '' ) ||
      (pProceso.condicionesFinancierasHabilitantes !== undefined && pProceso.condicionesFinancierasHabilitantes !== '' ) ||
      (pProceso.condicionesJuridicasHabilitantes !== undefined && pProceso.condicionesJuridicasHabilitantes !== '' ) ||
      (pProceso.condicionesTecnicasHabilitantes !== undefined && pProceso.condicionesTecnicasHabilitantes !== '' ) ||
      (pProceso.procesoSeleccionGrupo !== undefined && pProceso.procesoSeleccionGrupo.length > 0) ||
      (pProceso.procesoSeleccionCronograma !== undefined && pProceso.procesoSeleccionCronograma.length > 0)) {

        retorno = 2; // completo

      //Verifico si falta algo
      if (
        (pProceso.objeto === "" || pProceso.objeto === undefined) ||
        (pProceso.alcanceParticular === "" || pProceso.alcanceParticular === undefined) ||
        (pProceso.justificacion === "" || pProceso.justificacion === undefined) ||
        // //(pProceso.criteriosSeleccion === "" || pProceso.criteriosSeleccion === undefined) ||
        (pProceso.tipoIntervencionCodigo === "" || pProceso.tipoIntervencionCodigo === undefined) ||
        (pProceso.tipoAlcanceCodigo === "" || pProceso.tipoAlcanceCodigo === undefined) ||
        (pProceso.esDistribucionGrupos === undefined) ||
        (pProceso.responsableEstructuradorUsuarioid === undefined) ||
        (pProceso.responsableTecnicoUsuarioId === undefined) ||
        (pProceso.condicionesAsignacionPuntaje === undefined || pProceso.condicionesAsignacionPuntaje === '' ) ||
        (pProceso.condicionesFinancierasHabilitantes === undefined || pProceso.condicionesFinancierasHabilitantes === '' ) ||
        (pProceso.condicionesJuridicasHabilitantes === undefined || pProceso.condicionesJuridicasHabilitantes === '' ) ||
        (pProceso.condicionesTecnicasHabilitantes === undefined || pProceso.condicionesTecnicasHabilitantes === '' ) ||
        (pProceso.procesoSeleccionGrupo === undefined || pProceso.procesoSeleccionGrupo.length === 0) ||
        (pProceso.procesoSeleccionCronograma === undefined || pProceso.procesoSeleccionCronograma.length === 0)
        ) {

          retorno = 1; // en-proceso  

      }

      // grupos
      pProceso.procesoSeleccionGrupo.forEach(psg => {
        if (
          psg.nombreGrupo === undefined ||
          psg.tipoPresupuestoCodigo === undefined ||
          (psg.tipoPresupuestoCodigo === "2" && psg.valorMaximoCategoria === undefined) ||
          (psg.tipoPresupuestoCodigo === "2" && psg.valorMinimoCategoria === undefined) ||
          (psg.tipoPresupuestoCodigo === "1" && psg.valor === undefined) ||
          psg.plazoMeses === undefined
        ) {
          retorno = 1; // en-proceso   
        }
      });
      
    }
    return retorno;
  }

  estaIncompletoEvaluacion(pProceso:any):number{
    let retorno=0;    
    if(pProceso.estadoProcesoSeleccionCodigo==null || (pProceso.estadoProcesoSeleccionCodigo != EstadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario &&
      pProceso.estadoProcesoSeleccionCodigo != EstadosProcesoSeleccion.AprobadaSelecciónPorComiteFiduciario))
    {
      retorno=3;
    }
    else
    {
      if(pProceso.evaluacionDescripcion)
      {
        if(pProceso.evaluacionDescripcion!="" || pProceso.urlSoporteEvaluacion!="")
        {
         if(pProceso.evaluacionDescripcion!=undefined && pProceso.evaluacionDescripcion!="" && pProceso.urlSoporteEvaluacion!="" && pProceso.urlSoporteEvaluacion!=undefined)
         {
           retorno=2;
         }  
         else{
           retorno=1;
         }
        }
      }
      
    }
    return retorno;
  }

  estaIncompletoOrden(pProceso:ProcesoSeleccion):number{
    let retorno=0;    
    if(pProceso.estadoProcesoSeleccionCodigo != EstadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario &&
      pProceso.estadoProcesoSeleccionCodigo != EstadosProcesoSeleccion.AprobadaSelecciónPorComiteFiduciario)
    {
      retorno=3;
    }
    else
    {
     if(pProceso.procesoSeleccionProponente.length>0)
      {
        retorno=2;
      }  
      else{
        retorno=0;
      }
    }
    return retorno;
  }

  getStyleEvaluacion(){
    if ( this.bitPuedoEditar )
      return 'auto'
    else
      return 'none'

  }

  mostrarAyudaProcesoAprobado(){
    if ( 
          this.procesoSeleccion.estadoProcesoSeleccionCodigo != this.estadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario && 
          this.procesoSeleccion.estadoProcesoSeleccionCodigo != this.estadosProcesoSeleccion.AprobadaSelecciónPorComiteFiduciario 
       ){
         return true;
       }
       else{
         return false;
       }
  }

}
