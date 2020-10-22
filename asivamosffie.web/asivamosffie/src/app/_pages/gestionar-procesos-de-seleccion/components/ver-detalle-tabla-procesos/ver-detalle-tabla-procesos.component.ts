import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ProcesoSeleccion, ProcesoSeleccionService, EstadosProcesoSeleccion, TiposProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { pid } from 'process';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Respuesta } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { VerObservacionesComponent } from '../ver-observaciones/ver-observaciones.component';


@Component({
  selector: 'app-ver-detalle-tabla-procesos',
  templateUrl: './ver-detalle-tabla-procesos.component.html',
  styleUrls: ['./ver-detalle-tabla-procesos.component.scss']
})


export class VerDetalleTablaProcesosComponent implements OnInit{

  estadosProcesoSeleccion = EstadosProcesoSeleccion;
  tiposProcesoSeleccion = TiposProcesoSeleccion; 
  
  constructor(
              public dialogRef: MatDialogRef<VerDetalleTablaProcesosComponent>,
              @Inject(MAT_DIALOG_DATA) public data: ProcesoSeleccion,
              private router: Router,
              private procesoseleccionService: ProcesoSeleccionService,
              public dialog: MatDialog,
             ) 
  {

  }
  ngOnInit(): void {
    this.activarBotones();
  }

  editarVerDetalle:boolean=false;
  verDetalle:boolean=false;
  solicitarApertura:boolean=false;
  eliminar:boolean=false;
  monitorear:boolean=false;
  presentarEvaluacion:boolean=false;
  cerrarProceso:boolean=false;
  obervaciones:boolean=false;

  activarBotones()
  {
    this.editarVerDetalle=this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.Creado||
      ((this.data.tipoProcesoCodigo==this.tiposProcesoSeleccion.Abierta||
        this.data.tipoProcesoCodigo==this.tiposProcesoSeleccion.Cerrada) &&
      this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario)||

      this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.DevueltaAperturaPorComiteFiduciario||
      this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.DevueltaAperturaPorComiteTecnico ||
      this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.DevueltaSeleccionPorComiteFiduciario ||
      this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.DevueltaSeleccionPorComiteTecnico ||
      this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.DevueltoPorComiteFiduciario ||
      this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.DevueltoPorComiteTecnico;
    
    this.verDetalle=!this.editarVerDetalle;
    
    this.solicitarApertura=this.data.esCompleto 
      && (this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario &&
        this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadaAperturaPorComiteFiduciario &&
        this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadaAperturaPorComiteTecnico &&
        this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadaSeleccionPorComiteFiduciario &&
        this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadaSeleccionPorComiteTecnico &&
        this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadoPorComiteFiduciario &&
        this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadoPorComiteTecnico &&
        this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.AperturaEntramite)
        ;
    
    this.presentarEvaluacion=
      this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario
      && (this.data.tipoProcesoCodigo==this.tiposProcesoSeleccion.Abierta ||
        this.data.tipoProcesoCodigo==this.tiposProcesoSeleccion.Cerrada)
      && this.data.procesoSeleccionProponente.length>0;

    this.eliminar=this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.Creado;/*this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadaAperturaPorComiteFiduciario &&
      this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadaAperturaPorComiteTecnico &&
      this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadaSeleccionPorComiteFiduciario &&
      this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadaSeleccionPorComiteTecnico &&
      this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadoPorComiteFiduciario &&
      this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadoPorComiteTecnico &&
      this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.AperturaEntramite;*/

    this.monitorear=this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadaAperturaPorComiteFiduciario &&
      this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadaAperturaPorComiteTecnico &&
      this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadaSeleccionPorComiteFiduciario &&
      this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadaSeleccionPorComiteTecnico &&
      this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadoPorComiteFiduciario &&
      this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.RechazadoPorComiteTecnico &&
      this.data.estadoProcesoSeleccionCodigo!=this.estadosProcesoSeleccion.AperturaEntramite ;

    this.cerrarProceso=this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.Creado;
    this.obervaciones=this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.RechazadaAperturaPorComiteFiduciario ||
      this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.RechazadaAperturaPorComiteTecnico ||
      this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.RechazadaSeleccionPorComiteFiduciario ||
      this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.RechazadaSeleccionPorComiteTecnico ||
      this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.RechazadoPorComiteFiduciario ||
      this.data.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.RechazadoPorComiteTecnico;
    
    
  }

  onEditar(){
    
    switch (this.data.tipoProcesoCodigo)
    {
      case "1": { this.router.navigate(['/seleccion/seccionPrivada', this.data.procesoSeleccionId ]); } break;
      case "2": { this.router.navigate(['/seleccion/invitacionCerrada', this.data.procesoSeleccionId ]); } break;
      case "3": { this.router.navigate(['/seleccion/invitacionAbierta', this.data.procesoSeleccionId ]); } break;
    }

    
    this.dialogRef.close();
  }

  onNoClick(){
    
  }

  onMonitorearCronograma(){
    this.router.navigate(['/seleccion/monitorearCronograma', this.data.procesoSeleccionId ]);
    this.dialogRef.close();
  }

  onEliminar( pId: number ){

    this.openDialogSiNo('','¿Está seguro de eliminar este registro?', pId)
  }

  eliminarRegistro( pId: number ){
    this.procesoseleccionService.deleteProcesoSeleccion( pId ).subscribe( respuesta => {
      let r = respuesta as Respuesta;
       if ( r.code == "200" )
       {
         this.openDialog("", "<b>La información se ha eliminado correctamente.</b>",true);         
       }else
        this.openDialog("", r.message,false);
    })
  }

  onCerrarProceso( pId: number ){
    let proceso: ProcesoSeleccion = {
      procesoSeleccionId: pId,
      estadoProcesoSeleccionCodigo: this.estadosProcesoSeleccion.Cerrado
    }

     this.procesoseleccionService.changeStateProcesoSeleccion( proceso ).subscribe( respuesta => {
       this.openDialog("", respuesta.message,true);
       if ( respuesta.code == "200" ){
       }
    })
  }

  onAperturaComite( pId: number ){
    let proceso: ProcesoSeleccion = {
      procesoSeleccionId: pId,
      estadoProcesoSeleccionCodigo: this.estadosProcesoSeleccion.AperturaEntramite
    }

     this.procesoseleccionService.changeStateProcesoSeleccion( proceso ).subscribe( respuesta => {
       this.openDialog("", respuesta.message,true);
       if ( respuesta.code == "200" ){
          //this.dialogRef.close();
          //this.router.navigate(['/seleccion']);
          //location.reload();
       }
    })
  }

  onAperturaEvaluacion(pId:number){
    let proceso: ProcesoSeleccion = {
      procesoSeleccionId: pId,
      estadoProcesoSeleccionCodigo: this.estadosProcesoSeleccion.AprobacionDeSeleccionEnTramite
    }

     this.procesoseleccionService.changeStateProcesoSeleccion( proceso ).subscribe( respuesta => {
       this.openDialog("", respuesta.message,true);
       if ( respuesta.code == "200" ){          
       }
    })
  }

  openDialog(modalTitle: string, modalText: string,reload:boolean) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
    if(reload)
    {
      dialogRef.afterClosed().subscribe(result => {
        this.router.navigate(['/seleccion']);
        setTimeout(() => {
          location.reload();  
        }, 1000);
      });
    }
  }

  openDialogSiNo(modalTitle: string, modalText: string, e:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result === true)
      {
        this.eliminarRegistro(e);
      }           
    });
  }

  onVer()
  {
    switch (this.data.tipoProcesoCodigo)
    {
      case "1": { this.router.navigate(['/seleccion/seccionPrivada', this.data.procesoSeleccionId ]); } break;
      case "2": { this.router.navigate(['/seleccion/invitacionCerrada', this.data.procesoSeleccionId ]); } break;
      case "3": { this.router.navigate(['/seleccion/invitacionAbierta', this.data.procesoSeleccionId ]); } break;
    }

    
    this.dialogRef.close();
  }

  onObservaciones(id)
  {
    let dialogRef =this.dialog.open(VerObservacionesComponent, {
      width: '50em',
      data: { id }
    });   
    dialogRef.afterClosed().subscribe(result => {
            
    });
  }
}
