import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Listados, Proyecto } from 'src/app/core/_services/project/project.service';

@Component({
  selector: 'app-formulario-proyectos',
  templateUrl: './formulario-proyectos.component.html',
  styleUrls: ['./formulario-proyectos.component.scss']
})
export class FormularioProyectosComponent {
    constructor(private fb: FormBuilder) {}

    listadoTipoIntervencion:Listados[];
    listadoregion:Listados[];
    listadoDepartamento:Listados[];
    listadoMunicipio:Listados[];
    listadoInstitucion:Listados[];
    listadoSede:Listados[];
    listadoConvocatoria:Listados[];
    listadoPredios
    proyecto:Proyecto;
    CodigoDaneIE:string="";
    codigoDaneSede:string="";
  onSubmit() {
    alert('Thanks!');
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
      InstitucionEducativaId:null,
      SedeId:null,
      EnConvocatoria:null,
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
      PredioPrincipal:null,
      Sede:null,
      InfraestructuraIntervenirProyecto:null,
      ProyectoAportante:[],
      ProyectoPredio:[],
      cantidadAportantes:null};
    this.proyecto.ProyectoPredio.push({ProyectoPredioId:0,EstadoJuridicoCodigo:"",UsuarioCreacion:"",
      Predio:{CedulaCatastral:"",Direccion:"",DocumentoAcreditacionCodigo:"",
      FechaCreacion:new Date,InstitucionEducativaSedeId:0,NumeroDocumento:"",
      UsuarioCreacion:"",PredioId:0,TipoPredioCodigo:"",UbicacionLatitud:"",UbicacionLongitud:""}});
  }

  addInfraestructura()
  {
    
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
      if(this.proyecto.cantidadAportantes<this.proyecto.ProyectoAportante.length)
      {
        //preguntar
      }
      else{
        if(this.proyecto.cantidadAportantes>this.proyecto.ProyectoAportante.length)
        {
          
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
          }
        }
      }
    }
  }
}
