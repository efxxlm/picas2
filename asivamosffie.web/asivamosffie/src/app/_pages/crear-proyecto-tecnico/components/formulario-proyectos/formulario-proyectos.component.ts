import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Listados, Proyecto, ProjectService } from 'src/app/core/_services/project/project.service';
import { CommonService, Dominio, Localizacion } from 'src/app/core/_services/common/common.service';
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

  listadoDocumentoAcreditacion: Dominio[];
  listaTipoAportante: Dominio[];
  listaAportante: any[] = [];
  listaVigencias: any[] = [];
  listaInfraestructura: Dominio[];
  listaCordinaciones: Dominio[];

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
    institucionEducativaId: 0,
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
      cedulaCatastral: "", direccion: "", documentoAcreditacionCodigo: "",
      fechaCreacion: new Date, institucionEducativaSedeId: null, numeroDocumento: "",
      usuarioCreacion: "", predioId: 0, tipoPredioCodigo: "", ubicacionLatitud: "", ubicacionLongitud: ""
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
    //ajusto latitud y longitud de predios
    this.proyecto.predioPrincipal.ubicacionLatitud = this.proyecto.predioPrincipal.ubicacionLatitud + '°' + this.proyecto.predioPrincipal.ubicacionLatitud2;
    this.proyecto.predioPrincipal.ubicacionLongitud = this.proyecto.predioPrincipal.ubicacionLongitud + '°' + this.proyecto.predioPrincipal.ubicacionLongitud2;
    this.proyecto.institucionEducativaId = this.proyecto.institucionEducativaId.institucionEducativaSedeId;
    this.proyecto.sedeId = this.proyecto.sedeId.institucionEducativaSedeId
    this.projectServices.createOrUpdateProyect(this.proyecto).subscribe(respuesta => {
      this.openDialog('', respuesta.message);
      this.router.navigate(["/crearProyecto"]); 
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
          //console.log(respuesta.numeroActaJunta);
          this.proyecto = respuesta;
          //ajusto lartitud y longitud
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
          this.getInstitucion();
          this.getSede();
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
      //this.proyecto 
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

  getInstitucion() {

    this.commonServices.listaIntitucionEducativaByMunicipioId(this.proyecto.localizacionIdMunicipio).subscribe(respuesta => {
      this.listadoInstitucion = respuesta;
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
  
  getCodigoDane(){
    this.codigoDaneSede = this.proyecto.sedeId.codigoDane;
  }

  getSede() {
     //console.log(this.proyecto.institucionEducativaId);
     this.CodigoDaneIE = this.proyecto.institucionEducativaId.codigoDane;
    this.commonServices.listaSedeByInstitucionEducativaId(this.proyecto.institucionEducativaId.institucionEducativaSedeId).subscribe(respuesta => {
      this.listadoSede = respuesta;
      
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
      infraestructuraCodigo: "",
      cantidad: 0,
      eliminado: false,
      fechaCreacion: null,
      usuarioCreacion: "",
      usuarioEliminacion: "",
      plazoMesesObra: 0,
      plazoDiasObra: 0,
      plazoMesesInterventoria: 0,
      plazoDiasInterventoria: 0,
      coordinacionResponsableCodigo: ""
    });
  }

  evaluopredios() {
    if (this.proyecto.cantPrediosPostulados > 1) {
      if (this.proyecto.cantPrediosPostulados != this.proyecto.proyectoPredio.length) {
        if (this.proyecto.cantPrediosPostulados < this.proyecto.proyectoPredio.length) {
          this.proyecto.proyectoPredio = [];
          for (let a = this.proyecto.proyectoPredio.length + 1; a < this.proyecto.cantPrediosPostulados; a++) {
            this.proyecto.proyectoPredio.push({
              ProyectoPredioId: 0,  UsuarioCreacion: "",
              Predio: {
                cedulaCatastral: "", direccion: "", documentoAcreditacionCodigo: "",
                fechaCreacion: new Date, institucionEducativaSedeId: null, numeroDocumento: "",
                usuarioCreacion: "", predioId: 0, tipoPredioCodigo: "", ubicacionLatitud: "", ubicacionLongitud: ""
              }
            });
          }
        }
        else {
          if (this.proyecto.cantPrediosPostulados > this.proyecto.proyectoPredio.length) {

            for (let a = this.proyecto.proyectoPredio.length + 1; a < this.proyecto.cantPrediosPostulados; a++) {
              this.proyecto.proyectoPredio.push({
                ProyectoPredioId: 0,  UsuarioCreacion: "",
                Predio: {
                  cedulaCatastral: "", direccion: "", documentoAcreditacionCodigo: "",
                  fechaCreacion: new Date, institucionEducativaSedeId: null, numeroDocumento: "",
                  usuarioCreacion: "", predioId: 0, tipoPredioCodigo: "", ubicacionLatitud: "", ubicacionLongitud: ""
                }
              });
            }
          }
        }
      }
    }

  }
  evaluoaportantes() {
    if (this.proyecto.cantidadAportantes != this.proyecto.proyectoAportante.length) {
      this.proyecto.proyectoAportante = [];
      /*if(this.proyecto.cantidadAportantes<this.proyecto.ProyectoAportante.length)
      {
        
        //preguntar
      }
      else{
        if(this.proyecto.cantidadAportantes>this.proyecto.ProyectoAportante.length)
        {*/

      for (let a = this.proyecto.proyectoAportante.length; a < this.proyecto.cantidadAportantes; a++) {
        this.proyecto.proyectoAportante.push({
          proyectoAportanteId: null,
          proyectoId: null,
          aportanteId: null,
          eliminado: false,
          fechaCreacion: null,
          usuarioCreacion: "",
          cofinanciacionDocumentoID: null,
          aportante: { cofinanciacionAportanteId: 0, cofinanciacionDocumento: null, cofinanciacionId: 0, municipioId: 0, tipoAportanteId: 0 }
        });
        /*let listavacia:any[]=[];
        this.listaAportante.push(listavacia);
        console.log(this.listaAportante);
        this.listaVigencias.push(listavacia);*/
      }
      //}
      //}
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
        //console.log('terminó');
      });
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
        //console.log('terminó');
      });
  }

  getVigencia(event: MatSelectChange, i: number) {
    this.commonServices.listaDocumentoByAportanteId(event.value).subscribe(respuesta => {
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
        //console.log('terminó');
      });
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
        //console.log('terminó');
      });
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
}
