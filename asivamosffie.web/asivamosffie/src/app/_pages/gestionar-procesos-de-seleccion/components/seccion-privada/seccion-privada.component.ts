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
  descripcion_class:number=0;
  estudio_class:number=0;
  datos_class:number=0;
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
      {
        this.editMode();
        
      }

    })
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
        
        botonDescripcion.click();
        botonEstudio.click();
        botonProponente.click();
    });

  }

  openDialog(modalTitle: string, modalText: string, redirect?:boolean, id?:any) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
    
    if ( redirect )
    {
      dialogRef.afterClosed().subscribe(result => {
        if(result)
        {
          if(id.esCompleto)
          {
            this.router.navigate([`/seleccion/`]);            
          }
          else{
            this.router.navigate([`/seleccion/seccionPrivada/${ id.procesoSeleccionId }`]);
            setTimeout(() => {
              location.reload();  
            }, 1000);
          }
          
          
        }
      });
    }
  }

  onSubmit(){
    console.log(this.procesoSeleccion);
    this.procesoSeleccionService.guardarEditarProcesoSeleccion(this.procesoSeleccion).subscribe( respuesta => {
      this.openDialog( "", respuesta.message,respuesta.code == "200",respuesta.data )

      
      //console.log('respuesta',  respuesta );
    },
    error => {
      this.openDialog( "", error )
      console.log('respuesta',  error );
    },
     () => {})
  }

  estaIncompletoDescripcion(pProceso:any):number{    
    console.log(pProceso);
    
    let retorno:number=0;
    if(pProceso.objeto !="" ||
    pProceso.alcanceParticular!="" ||
    pProceso.justificacion!="" ||
    //pProceso.criteriosSeleccion!="" ||
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
     // pProceso.criteriosSeleccion!="" &&
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


    if(pProceso)
    {

      switch (pProceso.tipoProponenteCodigo)
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
    }
    

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
