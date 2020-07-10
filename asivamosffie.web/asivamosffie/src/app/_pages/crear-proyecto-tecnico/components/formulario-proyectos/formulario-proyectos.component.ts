import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Listados, Proyecto, ProjectService } from 'src/app/core/_services/project/project.service';
import { CommonService, Dominio, Localizacion } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { DatePipe } from '@angular/common';
import { MatSelectChange } from '@angular/material/select';

@Component({
  selector: 'app-formulario-proyectos',
  templateUrl: './formulario-proyectos.component.html',
  styleUrls: ['./formulario-proyectos.component.scss']
})
export class FormularioProyectosComponent {
  listadoDocumentoAcreditacion: Dominio[];
  listaTipoAportante: Dominio[];
  listaAportante: any[]=[];
  listaVigencias: any[]=[];
  listaInfraestructura: Dominio[];
  listaCordinaciones: Dominio[];
    constructor(private fb: FormBuilder,public commonServices: CommonService,public dialog: MatDialog,public datepipe: DatePipe,public projectServices:ProjectService) {}

    listadoTipoIntervencion:Dominio[];
    listadoregion:Localizacion[];
    listadoDepartamento:Localizacion[];
    listadoMunicipio:Localizacion[];
    listadoInstitucion:any[];
    listadoSede:any[];
    listadoConvocatoria:Dominio[];
    listadoPredios:Dominio[];
    proyecto:Proyecto;
    CodigoDaneIE:string="";
    codigoDaneSede:string="";

  onSubmit() {
    this.projectServices.createOrUpdateProyect(this.proyecto).subscribe(respuesta => {
      this.openDialog('', respuesta.message);
    }, 
    err => {
      let mensaje: string;
      console.log(err);
      let msn="";
      if(err.error.code=="501")
      {
        err.error.data.forEach(element => {
          msn+="El campo "+element.errors.key;
          element.errors.forEach(element => {
            msn+=element.errorMessage+" ";
          });
        });
      }
      if (err.error.message){
        mensaje = err.error.message;
      }
      else if (err.message){
        mensaje = err.message;
      }
      this.openDialog('Error', mensaje);
   },
   () => {
    //console.log('terminó');
   });
    
  }

  ngOnInit(): void {
    //agrego el predio principal
    this.proyecto={
      ProyectoId:null,
      FechaSesionJunta: null,
      NumeroActaJunta:null,
      TipoIntervencionCodigo:"",
      LlaveMen:"",
      LocalizacionIdMunicipio:"",
      InstitucionEducativaId:0,
      SedeId:0,
      EnConvocatoria:false,
      ConvocatoriaId:null,
      CantPrediosPostulados:null,
      TipoPredioCodigo:"",
      PredioPrincipalId:null,
      ValorObra:null,
      ValorInterventoria:null,
      ValorTotal:null,
      EstadoProyectoCodigo:"",
      Eliminado:null,
      FechaCreacion: null,
      UsuarioCreacion:"",
      FechaModificacion: null,
      UsuarioModificacion:"",
      InstitucionEducativaSede:null,
      LocalizacionIdMunicipioNavigation:null,
      PredioPrincipal:{
        CedulaCatastral:"",Direccion:"",DocumentoAcreditacionCodigo:"",
        FechaCreacion:new Date,InstitucionEducativaSedeId:0,NumeroDocumento:"",
        UsuarioCreacion:"",PredioId:0,TipoPredioCodigo:"",UbicacionLatitud:"",UbicacionLongitud:""
      },
      Sede:null,
      InfraestructuraIntervenirProyecto:[],
      ProyectoAportante:[],
      ProyectoPredio:[],
      cantidadAportantes:null};
    this.proyecto.ProyectoPredio.push({ProyectoPredioId:0,EstadoJuridicoCodigo:"",UsuarioCreacion:"",
      Predio:{CedulaCatastral:"",Direccion:"",DocumentoAcreditacionCodigo:"",
      FechaCreacion:new Date,InstitucionEducativaSedeId:0,NumeroDocumento:"",
      UsuarioCreacion:"",PredioId:0,TipoPredioCodigo:"",UbicacionLatitud:"",UbicacionLongitud:""}});
    //cargo lsitas
    this.getListas();
    this.addInfraestructura();
  

  }
  getListas() {
    this.commonServices.listaTipoIntervencion().subscribe(respuesta => {
      this.listadoTipoIntervencion=respuesta;
    }, 
    err => {
      let mensaje: string;
      console.log(err);
      if (err.message){
        mensaje = err.message;
      }
      else if (err.error.message){
        mensaje = err.error.message;
      }
      this.openDialog('Error', mensaje);
   },
   () => {
    //console.log('terminó');
   });


   this.commonServices.listaRegion().subscribe(respuesta => {
    this.listadoregion=respuesta;
    }, 
    err => {
      let mensaje: string;
      console.log(err);
      if (err.message){
        mensaje = err.message;
      }
      else if (err.error.message){
        mensaje = err.error.message;
      }
      this.openDialog('Error', mensaje);
  },
  () => {
    //console.log('terminó');
  });

  this.commonServices.listaTipoPredios().subscribe(respuesta => {
    this.listadoPredios=respuesta;
  }, 
  err => {
    let mensaje: string;
    console.log(err);
    if (err.message){
      mensaje = err.message;
    }
    else if (err.error.message){
      mensaje = err.error.message;
    }
    this.openDialog('Error', mensaje);
 },
 () => {
  //console.log('terminó');
 });

 this.commonServices.listaDocumentoAcrditacion().subscribe(respuesta => {
  this.listadoDocumentoAcreditacion=respuesta;
}, 
err => {
  let mensaje: string;
  console.log(err);
  if (err.message){
    mensaje = err.message;
  }
  else if (err.error.message){
    mensaje = err.error.message;
  }
  this.openDialog('Error', mensaje);
},
() => {
//console.log('terminó');
});

this.commonServices.listaTipoAportante().subscribe(respuesta => {
  this.listaTipoAportante=respuesta;
}, 
err => {
  let mensaje: string;
  console.log(err);
  if (err.message){
    mensaje = err.message;
  }
  else if (err.error.message){
    mensaje = err.error.message;
  }
  this.openDialog('Error', mensaje);
},
() => {
//console.log('terminó');
});



this.commonServices.listaInfraestructuraIntervenir().subscribe(respuesta => {
  this.listaInfraestructura=respuesta;
}, 
err => {
  let mensaje: string;
  console.log(err);
  if (err.message){
    mensaje = err.message;
  }
  else if (err.error.message){
    mensaje = err.error.message;
  }
  this.openDialog('Error', mensaje);
},
() => {
//console.log('terminó');
});
this.commonServices.listaCoordinaciones().subscribe(respuesta => {
  this.listaCordinaciones=respuesta;
}, 
err => {
  let mensaje: string;
  console.log(err);
  if (err.message){
    mensaje = err.message;
  }
  else if (err.error.message){
    mensaje = err.error.message;
  }
  this.openDialog('Error', mensaje);
},
() => {
//console.log('terminó');
});
  }

  getDepartments(event: MatSelectChange)
  {
    console.log(event.value);
    this.commonServices.listaDepartamentosByRegionId(event.value).subscribe(respuesta => {
      this.listadoDepartamento=respuesta;
    }, 
    err => {
      let mensaje: string;
      console.log(err);
      if (err.message){
        mensaje = err.message;
      }
      else if (err.error.message){
        mensaje = err.error.message;
      }
      this.openDialog('Error', mensaje);
   },
   () => {
    //console.log('terminó');
   });
  }

  getMunicipio(event: MatSelectChange)
  {
    this.commonServices.listaMunicipiosByIdDepartamento(event.value).subscribe(respuesta => {
      this.listadoMunicipio=respuesta;
    }, 
    err => {
      let mensaje: string;
      console.log(err);
      if (err.message){
        mensaje = err.message;
      }
      else if (err.error.message){
        mensaje = err.error.message;
      }
      this.openDialog('Error', mensaje);
   },
   () => {
    //console.log('terminó');
   });
  }

  getInstitucion()
  {
    
    this.commonServices.listaIntitucionEducativaByMunicipioId(this.proyecto.LocalizacionIdMunicipio).subscribe(respuesta => {
      this.listadoInstitucion=respuesta;
    }, 
    err => {
      let mensaje: string;
      console.log(err);
      if (err.message){
        mensaje = err.message;
      }
      else if (err.error.message){
        mensaje = err.error.message;
      }
      this.openDialog('Error', mensaje);
   },
   () => {
    //console.log('terminó');
   });
  }

  getSede()
  {
    //console.log(this.proyecto);
    this.commonServices.listaSedeByInstitucionEducativaId(this.proyecto.InstitucionEducativaId).subscribe(respuesta => {
      this.listadoSede=respuesta;
    }, 
    err => {
      let mensaje: string;
      console.log(err);
      if (err.message){
        mensaje = err.message;
      }
      else if (err.error.message){
        mensaje = err.error.message;
      }
      this.openDialog('Error', mensaje);
   },
   () => {
    //console.log('terminó');
   });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  addInfraestructura()
  {
    this.proyecto.InfraestructuraIntervenirProyecto.push({
      InfraestrucutraIntervenirProyectoId :0,
      ProyectoId :0,
      InfraestructuraCodigo:"" ,
      Cantidad :0,
      Eliminado:false ,
      FechaCreacion:null ,
      UsuarioCreacion:"" ,      
      UsuarioEliminacion:"" ,
      PlazoMesesObra :0,
      PlazoDiasObra:0 ,
      PlazoMesesInterventoria:0 ,
      PlazoDiasInterventoria:0 ,
      CoordinacionResponsableCodigo:""
    });
  }

  evaluopredios()
  {
    if(this.proyecto.CantPrediosPostulados!=this.proyecto.ProyectoPredio.length)
    {
      if(this.proyecto.CantPrediosPostulados<this.proyecto.ProyectoPredio.length)
      {
        //preguntar
      }
      else{
        if(this.proyecto.CantPrediosPostulados>this.proyecto.ProyectoPredio.length)
        {
          
          for(let a=this.proyecto.ProyectoPredio.length;a<this.proyecto.CantPrediosPostulados;a++)
          {
            this.proyecto.ProyectoPredio.push({ProyectoPredioId:0,EstadoJuridicoCodigo:"",UsuarioCreacion:"",
              Predio:{CedulaCatastral:"",Direccion:"",DocumentoAcreditacionCodigo:"",
              FechaCreacion:new Date,InstitucionEducativaSedeId:0,NumeroDocumento:"",
              UsuarioCreacion:"",PredioId:0,TipoPredioCodigo:"",UbicacionLatitud:"",UbicacionLongitud:""}});
          }
        }
      }
    }
  }
  evaluoaportantes()
  {
    if(this.proyecto.cantidadAportantes!=this.proyecto.ProyectoAportante.length)
    {
      this.proyecto.ProyectoAportante=[];
      /*if(this.proyecto.cantidadAportantes<this.proyecto.ProyectoAportante.length)
      {
        
        //preguntar
      }
      else{
        if(this.proyecto.cantidadAportantes>this.proyecto.ProyectoAportante.length)
        {*/
          
          for(let a=this.proyecto.ProyectoAportante.length;a<this.proyecto.cantidadAportantes;a++)
          {
            this.proyecto.ProyectoAportante.push({
              ProyectoAportanteId :null,
              ProyectoId :null,
              AportanteId :null,
              Eliminado :false,
              FechaCreacion:null ,
              UsuarioCreacion:"" ,
            });
            this.listaAportante.push({});
            this.listaVigencias.push({});
          }
        //}
      //}
    }
  }
  valorTotal(aportantes:any)
  {
    aportantes.ValorTotalAportante=aportantes.ValorInterventoria+aportantes.ValorObra;
  }

  formularioCompleto()
  {
    return true;
  }

  getAportante(i:number)
  {
    this.commonServices.listaNombreAportante().subscribe(respuesta => {
      this.listaAportante[i]=respuesta;
    }, 
    err => {
      let mensaje: string;
      console.log(err);
      if (err.message){
        mensaje = err.message;
      }
      else if (err.error.message){
        mensaje = err.error.message;
      }
      this.openDialog('Error', mensaje);
    },
    () => {
    //console.log('terminó');
    });
    
    
  }

  getVigencia(i:number)
  {
    this.commonServices.listaVigencias().subscribe(respuesta => {
      this.listaVigencias[i]=respuesta;
    }, 
    err => {
      let mensaje: string;
      console.log(err);
      if (err.message){
        mensaje = err.message;
      }
      else if (err.error.message){
        mensaje = err.error.message;
      }
      this.openDialog('Error', mensaje);
    },
    () => {
    //console.log('terminó');
    });
  }

}
