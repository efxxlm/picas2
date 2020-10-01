import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Listados, Proyecto, ProjectService } from 'src/app/core/_services/project/project.service';
import { CommonService, Dominio, Localizacion, TiposAportante } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { DatePipe } from '@angular/common';
import { MatSelectChange } from '@angular/material/select';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-formulario-proyectos',
  templateUrl: './formulario-proyectos.component.html',
  styleUrls: ['./formulario-proyectos.component.scss']
})
export class FormularioProyectosComponent implements OnInit {

  maxDate: Date;
  tipoAportante = TiposAportante;
  listadoDocumentoAcreditacion: Dominio[];
  listaTipoAportante: Dominio[];
  listaAportante: any[] = [];
  listadoDepto: any[] = [];
  listadoMun: any[] = [];
  listaVigencias: any[] = [];
  listaInfraestructura: Dominio[];
  listaCordinaciones: Dominio[];
  listaNombreAportantes: any[]=[];

  constructor(
    private fb: FormBuilder,
    public commonServices: CommonService,
    public dialog: MatDialog,
    public projectServices: ProjectService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.maxDate = new Date();
  }

  listadoTipoIntervencion: Dominio[];
  listadoregion: Localizacion[];
  listadoDepartamento: Localizacion[];
  listadoMunicipio: Localizacion[];
  listadoInstitucion: any[];
  listadoSede: any[];
  listadoConvocatoria: Dominio[];
  listadoPredios: Dominio[];
  proyecto: Proyecto = {
    proyectoId: null,
    fechaSesionJunta: null,
    numeroActaJunta: null,
    tipoIntervencionCodigo: null,
    llaveMen: '',
    localizacionIdMunicipio: '',
    institucionEducativaId:0,
    sedeId: 0,
    enConvocatoria: false,
    convocatoriaId: null,
    cantPrediosPostulados: null,
    tipoPredioCodigo: '',
    predioPrincipalId: null,
    valorObra: null,
    valorInterventoria: null,
    valorTotal: null,
    estadoProyectoCodigo: '',
    eliminado: null,
    fechaCreacion: null,
    usuarioCreacion: '',
    fechaModificacion: null,
    usuarioModificacion: '',
    institucionEducativaSede: null,
    localizacionIdMunicipioNavigation: null,
    predioPrincipal: {
      cedulaCatastral: '', direccion: '', documentoAcreditacionCodigo: '',
      fechaCreacion: new Date, institucionEducativaSedeId: null, numeroDocumento: '',
      usuarioCreacion: '', predioId: 0, tipoPredioCodigo: '', ubicacionLatitud: '', ubicacionLongitud: ''
    },
    sede: null,
    infraestructuraIntervenirProyecto: [],
    proyectoAportante: [],
    proyectoPredio: [],
    cantidadAportantes: null
  };
  CodigoDaneIE: string = '';
  codigoDaneSede: string = '';

  onSubmit() {
    // ajusto latitud y longitud de predios
    this.proyecto.predioPrincipal.ubicacionLatitud = this.proyecto.predioPrincipal.ubicacionLatitud + '°' + this.proyecto.predioPrincipal.ubicacionLatitud2;
    this.proyecto.predioPrincipal.ubicacionLongitud = this.proyecto.predioPrincipal.ubicacionLongitud + '°' + this.proyecto.predioPrincipal.ubicacionLongitud2;
    //this.proyecto.institucionEducativaId = this.proyecto.institucionEducativaId;
    //this.proyecto.sedeId = this.proyecto.sede.institucionEducativaSedeId?this.proyecto.sede.institucionEducativaSedeId:this.proyecto.sedeId;
    this.projectServices.createOrUpdateProyect(this.proyecto).subscribe(respuesta => {
      this.openDialog('', respuesta.message);
      this.router.navigate(['/crearProyecto']);
    },
      err => {
        let mensaje: string;
        console.log(err);
        let msn = '';
        if (err.error.code === '501') {
          err.error.data.forEach((element: { errors: { key: string; forEach: (arg0: (element: any) => void) => void; }; }) => {
            msn += 'El campo ' + element.errors.key;
            element.errors.forEach((element: { errorMessage: string; }) => {
              msn += element.errorMessage + ' ';
            });
          });
        }
        if (err.error.message) {
          mensaje = err.error.message;
        }
        else if (err.message) {
          mensaje = err.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });

  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    console.log(id);


    if (id) {
      this.commonServices.forkProject().subscribe(listas => {
        console.log(listas);
        this.listadoTipoIntervencion = listas[0];
        this.listadoregion = listas[1];
        this.listadoPredios = listas[2];
        this.listadoDocumentoAcreditacion = listas[3];
        this.listaTipoAportante = listas[4];
        this.listaInfraestructura = listas[5];
        this.listaCordinaciones = listas[6];
        this.listadoConvocatoria = listas[7];
        this.projectServices.getProjectById(Number(id)).subscribe(respuesta => {
          // console.log(respuesta.numeroActaJunta);
          this.proyecto = respuesta;
          // ajusto lartitud y longitud
          if (respuesta.predioPrincipal.ubicacionLatitud.indexOf('°') > 1) {
            const lat = respuesta.predioPrincipal.ubicacionLatitud.split('°');
            this.proyecto.predioPrincipal.ubicacionLatitud = lat[0];
            this.proyecto.predioPrincipal.ubicacionLatitud2 = lat[1];
          }
          if (respuesta.predioPrincipal.ubicacionLongitud.indexOf('°') > 1) {
            const lon = respuesta.predioPrincipal.ubicacionLongitud.split('°');
            this.proyecto.predioPrincipal.ubicacionLongitud = lon[0];
            this.proyecto.predioPrincipal.ubicacionLongitud2 = lon[1];
          }

          this.proyecto.cantidadAportantes = respuesta.proyectoAportante.length;
          this.getInstitucion(respuesta.institucionEducativaId,respuesta.sedeId);
          
          this.commonServices.forkDepartamentoMunicipio(respuesta.localizacionIdMunicipio).subscribe(
            listadoregiones => {
              this.listadoMunicipio = listadoregiones[0];
              this.listadoDepartamento = listadoregiones[1];
              this.proyecto.localizacionIdMunicipio = respuesta.localizacionIdMunicipio;

              this.proyecto.depid = listadoregiones[0][0].idPadre;
              this.proyecto.regid = listadoregiones[1][0].idPadre;
            }
          );
          let i = 0;
          respuesta.proyectoAportante.forEach(element => {
            this.getAportanteById(element.aportante.tipoAportanteId, i);
            this.getVigenciaById(element.aportanteId, i);
            i++;
          });
        },
          err => {
            let mensaje: string;
            console.log(err);
            if (err.message) {
              mensaje = err.message;
            }
            else if (err.error.message) {
              mensaje = err.error.message;
            }
            this.openDialog('Error', mensaje);
          },
          () => {
            // console.log('terminó');
          });
      });

    }
    else {
      // this.proyecto
      // agrego el predio principal

      /*this.proyecto.proyectoPredio.push({
        ProyectoPredioId: 0, EstadoJuridicoCodigo: '', UsuarioCreacion: '',
        Predio: {
          CedulaCatastral: '', Direccion: '', DocumentoAcreditacionCodigo: '',
          FechaCreacion: new Date, InstitucionEducativaSedeId: 0, NumeroDocumento: '',
          UsuarioCreacion: '', PredioId: 0, TipoPredioCodigo: '', UbicacionLatitud: '', UbicacionLongitud: ''
        }
      });*/
      // cargo lsitas
      this.getListas();
      this.addInfraestructura();
    }

  }
  getListas() {
    this.commonServices.listaTipoIntervencion().subscribe(respuesta => {
      this.listadoTipoIntervencion = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });


    this.commonServices.listaRegion().subscribe(respuesta => {
      this.listadoregion = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });

    this.commonServices.listaTipoPredios().subscribe(respuesta => {
      this.listadoPredios = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });

    this.commonServices.listaDocumentoAcrditacion().subscribe(respuesta => {
      this.listadoDocumentoAcreditacion = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });

    this.commonServices.listaTipoAportante().subscribe(respuesta => {
      this.listaTipoAportante = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });
    this.commonServices.listaInfraestructuraIntervenir().subscribe(respuesta => {
      this.listaInfraestructura = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });
    this.commonServices.listaCoordinaciones().subscribe(respuesta => {
      this.listaCordinaciones = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });
    this.commonServices.listaConvocatoria().subscribe(respuesta => {
      this.listadoConvocatoria = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });

  }

  getDepartments(event: MatSelectChange) {
    console.log(event.value);
    this.commonServices.listaDepartamentosByRegionId(event.value).subscribe(respuesta => {
      this.listadoDepartamento = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });
  }

  getMunicipio(event: MatSelectChange) {
    this.commonServices.listaMunicipiosByIdDepartamento(event.value).subscribe(respuesta => {
      this.listadoMunicipio = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });
  }

  getInstitucion(institudcionid?:number,sedeid?:number) {

    console.log(this.proyecto);
    this.commonServices.listaIntitucionEducativaByMunicipioId(this.proyecto.localizacionIdMunicipio).subscribe(respuesta => {
      this.listadoInstitucion = respuesta;
      this.proyecto.institucionEducativaId=institudcionid;//lo uso como patch pero no esta funcionando
      this.getSede(sedeid); 
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });
  }

  getCodigoDane() {
    if(this.proyecto.tipoIntervencionCodigo!=1)
    {
      let institucion=this.listadoSede.filter(x=>x.institucionEducativaSedeId==this.proyecto.sedeId);
      console.log(institucion);
      this.codigoDaneSede = institucion?institucion[0].codigoDane:"";
    }
  }

  getSede(sedeid?:number) {
     console.log(this.proyecto.tipoIntervencionCodigo);
    if(this.proyecto.tipoIntervencionCodigo>1)
    {
      let institucion=this.listadoInstitucion.filter(x=>x.institucionEducativaSedeId==this.proyecto.institucionEducativaId);
      console.log(institucion);
      this.CodigoDaneIE = institucion?institucion[0].codigoDane:"";  
    }
    
    console.log("loading sede");
    this.commonServices.listaSedeByInstitucionEducativaId(this.proyecto.institucionEducativaId)
      .subscribe(respuesta => {
        console.log("fin sede");
        this.listadoSede = respuesta;    
        console.log("set sede"+sedeid);    
        this.proyecto.sedeId=sedeid;
      },
        err => {
          let mensaje: string;
          console.log(err);
          if (err.message) {
            mensaje = err.message;
          }
          else if (err.error.message) {
            mensaje = err.error.message;
          }
          this.openDialog('Error', mensaje);
        },
        () => {
          // console.log('terminó');
          this.proyecto.sedeId=sedeid;
        });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  addInfraestructura() {
    this.proyecto.infraestructuraIntervenirProyecto.push({
      infraestrucutraIntervenirProyectoId: 0,
      proyectoId: 0,
      infraestructuraCodigo: '',
      cantidad: null,
      eliminado: false,
      fechaCreacion: null,
      usuarioCreacion: '',
      usuarioEliminacion: '',
      plazoMesesObra: null,
      plazoDiasObra: null,
      plazoMesesInterventoria: null,
      plazoDiasInterventoria: null,
      coordinacionResponsableCodigo: ''
    });
  }

  evaluopredios() {
    console.log(this.proyecto.cantPrediosPostulados);
    console.log(this.proyecto.proyectoPredio.length+1);
    if (this.proyecto.cantPrediosPostulados >= 1) {
      if (this.proyecto.cantPrediosPostulados !== this.proyecto.proyectoPredio.length+1) {
        if (this.proyecto.cantPrediosPostulados < this.proyecto.proyectoPredio.length+1) {
          
          console.log("debo eliminar");
          //valido si tiene dataif()
          let bitesvacio=true;
          this.proyecto.proyectoPredio.forEach(element => {
          
            if(element.Predio.numeroDocumento!="")
            {
              bitesvacio=false;
            }
            if(element.Predio.documentoAcreditacionCodigo!="")
            {
              bitesvacio=false;
            }
            if(element.Predio.cedulaCatastral!="")
            {
              bitesvacio=false;
            }

          });
          if(bitesvacio)
          {
            this.proyecto.proyectoPredio.pop();
          }
          else
          {
            console.log(this.proyecto.proyectoPredio.length+1);
            this.proyecto.cantPrediosPostulados=this.proyecto.proyectoPredio.length+1;
            this.openDialog("","<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>");
            
          }
        }
        else {
          if (this.proyecto.cantPrediosPostulados > this.proyecto.proyectoPredio.length+1) {            
            for (let a = this.proyecto.proyectoPredio.length + 1; a < this.proyecto.cantPrediosPostulados; a++) {
              this.proyecto.proyectoPredio.push({
                ProyectoPredioId: 0, UsuarioCreacion: '',
                Predio: {
                  cedulaCatastral: '', direccion: '', documentoAcreditacionCodigo: '',
                  fechaCreacion: new Date, institucionEducativaSedeId: null, numeroDocumento: '',
                  usuarioCreacion: '', predioId: 0, tipoPredioCodigo: '', ubicacionLatitud: '', ubicacionLongitud: ''
                }
              });
            }
          }          
        }
      }
    }

  }
  evaluoaportantes() {
    if(this.proyecto.cantidadAportantes>0 && this.proyecto.cantidadAportantes!=null)
    {
      if (this.proyecto.cantidadAportantes !== this.proyecto.proyectoAportante.length) {
        //this.proyecto.proyectoAportante = [];
        if(this.proyecto.cantidadAportantes<this.proyecto.proyectoAportante.length)
        {
          console.log("resta");
          let bitesvacio=true;
          this.proyecto.proyectoAportante.forEach(element => {
          
            if(element.cofinanciacionDocumentoID>0)
            {
              bitesvacio=false;
            }
            if(element.valorObra>0)
            {
              bitesvacio=false;
            }
            if(element.valorInterventoria>0)
            {
              bitesvacio=false;
            }

          });
          if(bitesvacio)
          {
            this.proyecto.proyectoAportante.pop();
          }
          else
          {            
            this.proyecto.cantidadAportantes=this.proyecto.proyectoAportante.length;            
            this.openDialog("","<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>");            
          }
        }
        else{
          if(this.proyecto.cantidadAportantes>this.proyecto.proyectoAportante.length)
          {
  
            for (let a = this.proyecto.proyectoAportante.length; a < this.proyecto.cantidadAportantes; a++) {
              this.proyecto.proyectoAportante.push({
                proyectoAportanteId: null,
                proyectoId: null,
                aportanteId: null,
                eliminado: false,
                fechaCreacion: null,
                usuarioCreacion: '',
                cofinanciacionDocumentoID: null,
                nombreAportante:"",
                aportante: {
                  cofinanciacionAportanteId: 0,
                  cofinanciacionDocumento: null,
                  cofinanciacionId: 0,
                  municipioId: 0,
                  departamentoId:0,
                  tipoAportanteId: 0
                }
              });
              let listavacia:any[]=[];
              this.listaAportante.push(listavacia);
              this.listaNombreAportantes.push(listavacia);
              this.listadoDepto.push(listavacia);
              this.listadoMun.push(listavacia);
              console.log(this.listaAportante);
              this.listaVigencias.push(listavacia);
            }
          }
        }
      }
    }    
  }
  valorTotal(aportantes: any) {
    console.log(aportantes);
    aportantes.valorTotalAportante = aportantes.valorInterventoria + aportantes.valorObra;
  }

  formularioCompleto() {
    return true;
  }

  getAportante(event: MatSelectChange, i: number) {
    this.commonServices.listaAportanteByTipoAportanteId(event.value).subscribe(respuesta => {
      this.listaVigencias[i]=[];
      this.listaAportante[i]=[];
      if(this.tipoAportante.FFIE.includes(event.value.toString()))
      {
        this.listaVigencias[i]=respuesta;
      }
      else
      {
        if(this.tipoAportante.ET.includes(event.value.toString()))
        {
          this.listaAportante[i]=respuesta;
          console.log(this.listaAportante[i]);
          this.commonServices.listaDepartamentos().subscribe(res=>{
            this.listadoDepto[i]=res;
          });
          this.listadoMun[i]=[]
        }
        else
        {

          this.listaAportante[i]=respuesta;
          respuesta.forEach(element => {
            
            console.log("evaluo");
            console.log(element.nombre);
            this.listaNombreAportantes[i]=[];
            if(!this.listaNombreAportantes[i].includes(element.nombre))
            {
              console.log(this.listaAportante[i].nombre);
              this.listaNombreAportantes[i].push(element.nombre); 
            }
          });
        }  
      }             
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });
  }

  getMunAportante(id: number, i: number)
  {
    console.log(this.proyecto.proyectoAportante[i].depto);
    this.commonServices.listaMunicipiosByIdDepartamento(this.proyecto.proyectoAportante[i].depto).
    subscribe(res=>{
      this.listadoMun[i]=res;
    })
    
  }

  getAportanteById(id: number, i: number) {
    this.commonServices.listaAportanteByTipoAportanteId(id).subscribe(respuesta => {
      console.log(respuesta);
      this.listaAportante[i] = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });
  }

  getVigenciaByMun(event: MatSelectChange, i: number) {
    console.log("busco "+this.proyecto.proyectoAportante[i].mun);
    console.log(this.listaAportante[i]);
    this.listaVigencias[i]=this.listaAportante[i].filter(x=>x.municipioId==this.proyecto.proyectoAportante[i].mun);
  }

  getVigencia(event: MatSelectChange, i: number) {
    console.log("busco "+this.proyecto.proyectoAportante[i].nombreAportante);
    console.log(this.listaAportante[i]);
    this.listaVigencias[i]=this.listaAportante[i].filter(x=>x.nombre==this.proyecto.proyectoAportante[i].nombreAportante);
    /*this.commonServices.listaDocumentoByAportanteId(event.value).subscribe(respuesta => {
      this.listaVigencias[i] = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });*/
  }

  getVigenciaById(id: number, i: number) {
    this.commonServices.listaDocumentoByAportanteId(id).subscribe(respuesta => {
      this.listaVigencias[i] = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });
  }

  deleteAportante(i: number) {    
    this.proyecto.proyectoPredio.splice(i,1);
    this.proyecto.cantPrediosPostulados=this.proyecto.proyectoPredio.length+1;
  }

  deleteAportanteAportante(ii:number){
    this.proyecto.proyectoAportante.splice(ii,1);
    this.proyecto.cantidadAportantes=this.proyecto.proyectoAportante.length;
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  numberDay(e: { keyCode: any; },valor:number,i:number)
  {
    
    const tecla = e.keyCode;
    let ok=false;
    if (tecla === 8 ) { ok= true; } // Tecla de retroceso (para poder borrar)
    if (tecla === 48) { ok= true; } // 0
    if (tecla === 49) { ok= true; } // 1
    if (tecla === 50) { ok= true; } // 2
    if (tecla === 51) { ok= true; } // 3
    if (tecla === 52) { ok= true; } // 4
    if (tecla === 53) { ok= true; } // 5
    if (tecla === 54) { ok= true; } // 6
    if (tecla === 55) { ok= true; } // 7
    if (tecla === 56) { ok= true; } // 8
    if (tecla === 57) { ok= true; } // 9
    const patron = /1/; // ver nota
    const te = String.fromCharCode(tecla);
    
    console.log("patron: valor"+valor);

    if(ok)
    {
      console.log(valor>30);
      if(valor>30)
      {
        return false;
      }      
      else{
        return true;
      }
    }
    else
    {
      return false;
    }
    
  }

  number(e: { keyCode: any; }) {
    const tecla = e.keyCode;
    if (tecla === 8 ) { return true; } // Tecla de retroceso (para poder borrar)
    if (tecla === 48) { return true; } // 0
    if (tecla === 49) { return true; } // 1
    if (tecla === 50) { return true; } // 2
    if (tecla === 51) { return true; } // 3
    if (tecla === 52) { return true; } // 4
    if (tecla === 53) { return true; } // 5
    if (tecla === 54) { return true; } // 6
    if (tecla === 55) { return true; } // 7
    if (tecla === 56) { return true; } // 8
    if (tecla === 57) { return true; } // 9
    const patron = /1/; // ver nota
    const te = String.fromCharCode(tecla);
    return patron.test(te);
  }


  validateKeypressLlave(event: KeyboardEvent) {
    const alphanumeric = /[A-Za-z0-9-]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }
  deleteInfraestructura(indice:number)
  {
    console.log(indice)
    this.proyecto.infraestructuraIntervenirProyecto.splice(indice,1); 
  }
}
