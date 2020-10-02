import { Component, OnInit } from '@angular/core';
import { ProcesoSeleccion, ProcesoSeleccionService, TiposProcesoSeleccion, EstadosProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { forkJoin } from 'rxjs';
import { timeout } from 'rxjs/operators';

@Component({
  selector: 'app-invitacion-cerrada',
  templateUrl: './invitacion-cerrada.component.html',
  styleUrls: ['./invitacion-cerrada.component.scss']
})
export class InvitacionCerradaComponent implements OnInit {

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
    estadoProcesoSeleccionCodigo: this.estadosProcesoSeleccion.Creado,
    

  };
  descripcion_class: number=0;
  estudio_class: number=0;
  datos_class: number=0;

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
          this.descripcion_class=this.estaIncompletoDescripcion(this.procesoSeleccion);
          this.estudio_class=this.estaIncompletoEstudio(this.procesoSeleccion);
          this.datos_class=this.estaIncompletoDatos(this.procesoSeleccion.procesoSeleccionProponente[0]);
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
      this.openDialog( "", respuesta.message )
      this.router.navigate(["/seleccion/invitacionCerrada", respuesta.data.procesoSeleccionId]);
      setTimeout(() => {
        location.reload();  
      }, 1000);
      console.log('respuesta',  respuesta );
    })
  }

  getStyleEvaluacion(){
    if (this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario )
      return 'auto'
    else
      return 'none'

  }

  getStyleProponentesSeleccionados(){
    if ( this.procesoSeleccion.evaluacionDescripcion && this.procesoSeleccion.evaluacionDescripcion.length > 0 )
      return 'auto'
    else
      return 'none'
  }

  getStyleProponentesCreado(){
    if (this.procesoSeleccion.estadoProcesoSeleccionCodigo == this.estadosProcesoSeleccion.Creado )
      return 'auto'
    else
      return 'none'
  }

  estaIncompletoDescripcion(pProceso:any):number{    
    console.log(pProceso);
    
    let retorno:number=0;
    if(pProceso.objeto !="" ||
    pProceso.alcanceParticular!="" ||
    pProceso.justificacion!="" ||
    pProceso.criteriosSeleccion!="" ||
    pProceso.tipoIntervencionCodigo!="" ||
    pProceso.tipoAlcanceCodigo!="" ||
    //pProceso.esDistribucionGrupos!="" ||
    pProceso.responsableEstructuradorUsuarioid!=undefined ||
    pProceso.responsableTecnicoUsuarioId!=undefined ||
    pProceso.procesoSeleccionGrupo.length>=1||
    pProceso.procesoSeleccionCronograma.length>=1)
    {
      if(pProceso.objeto !="" &&
      pProceso.alcanceParticular!="" &&
      pProceso.justificacion!="" &&
      pProceso.criteriosSeleccion!="" &&
      pProceso.tipoIntervencionCodigo!="" &&
      pProceso.tipoAlcanceCodigo!="" &&
      //pProceso.esDistribucionGrupos!="" &&
      pProceso.responsableEstructuradorUsuarioid!=undefined &&
      pProceso.responsableTecnicoUsuarioId!=undefined &&
      pProceso.procesoSeleccionGrupo.length>0 &&
      pProceso.procesoSeleccionCronograma.length>0)
      {
        retorno= 2;
      }
      else{
        console.log(pProceso.objeto!="");
      console.log(pProceso.alcanceParticular!="");
      console.log(pProceso.justificacion!="");

      console.log(pProceso.tipoIntervencionCodigo!="");
      console.log(pProceso.tipoAlcanceCodigo!="");
      console.log(pProceso.responsableEstructuradorUsuarioid!=undefined);
      console.log(pProceso.responsableTecnicoUsuarioId!=undefined);
      console.log(pProceso.procesoSeleccionGrupo.length>=1);
      console.log(pProceso.procesoSeleccionCronograma.length>=1);
      retorno=1;
      }
    }
    return retorno;
  }

  estaIncompletoDatos(pProceso:any):number{
    let retorno=0;
    
    /*switch (pProceso.tipoProponenteCodigo)
    {
      case "1": {
        if(
          pProceso.procesoSeleccionProponenteId!="" ||
        pProceso.direccionProponente!="" ||
        pProceso.emailProponente!="" ||
        pProceso.localizacionIdMunicipio!="" ||
        pProceso.nombreProponente!="" ||
        pProceso.numeroIdentificacion!="" ||
        pProceso.telefonoProponente!="")
        {
          if(
            pProceso.procesoSeleccionProponenteId!="" &&
            pProceso.direccionProponente!="" &&
            pProceso.emailProponente!="" &&
            pProceso.localizacionIdMunicipio!="" &&
            pProceso.nombreProponente!="" &&
            pProceso.numeroIdentificacion!="" &&
            pProceso.telefonoProponente!="")
          {
            retorno=2;
          } 
          else{
            retorno=1;
          }
          
        }break;
          }
          case "2": {
            if(
            pProceso.procesoSeleccionProponenteId!="" ||
            pProceso.nombreProponente!="" ||
            pProceso.nombreProponente!="" ||
            pProceso.numeroIdentificacion!="" ||
            pProceso.nombreRepresentanteLegal!="" ||
            pProceso.cedulaRepresentanteLegal!="" ||
            pProceso.direccionProponente!="" ||
            pProceso.telefonoProponente!="" ||
            pProceso.emailProponente!="")
            {
              if(
                pProceso.procesoSeleccionProponenteId!="" &&
                pProceso.nombreProponente!="" &&
                pProceso.nombreProponente!="" &&
                pProceso.numeroIdentificacion!="" &&
                pProceso.nombreRepresentanteLegal!="" &&
                pProceso.cedulaRepresentanteLegal!="" &&
                pProceso.direccionProponente!="" &&
                pProceso.telefonoProponente!="" &&
                pProceso.emailProponente!="")
                {
                  retorno=2
                }
                else{
                  retorno=1;
                }              
            }
            break;
          }
          case "4": {
            if(
            pProceso.procesoSeleccionProponenteId !="" ||
            pProceso.nombreProponente!="" ||            
            pProceso.nombreRepresentanteLegal!="" ||
            pProceso.cedulaRepresentanteLegal!="" ||
            pProceso.direccionProponente!="" ||
            pProceso.telefonoProponente!="" ||
            pProceso.emailProponente!="" ||
            pProceso.procesoSeleccionIntegrante!=""
            )
            {
              if(
                pProceso.procesoSeleccionProponenteId !="" &&
                pProceso.nombreProponente!=""  &&                
                pProceso.nombreRepresentanteLegal!=""  &&
                pProceso.cedulaRepresentanteLegal!=""  &&
                pProceso.direccionProponente!=""  &&
                pProceso.telefonoProponente!=""  &&
                pProceso.emailProponente!=""  &&
                pProceso.procesoSeleccionIntegrante!=""
                )
                {
                  retorno=2;
                }
                else{
                  retorno=1;
                }
            }
            break;
          }
        
    }
*/
    return retorno;
  }


  estaIncompletoEstudio(pProceso:any):number{
    let retorno=0;
    if(pProceso.cantidadCotizaciones ||
      pProceso.procesoSeleccionCotizacion.length>0
    )
    {
      if(pProceso.cantidadCotizaciones &&
      pProceso.procesoSeleccionCotizacion.length>0
      )
      {
        retorno= 2;
      } 
      else{
        retorno =1;
      }
    }

    
    return retorno;
  }

}
