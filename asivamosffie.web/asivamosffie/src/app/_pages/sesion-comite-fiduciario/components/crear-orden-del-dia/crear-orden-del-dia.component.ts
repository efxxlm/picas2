import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { SolicitudesContractuales, SesionComiteTema, EstadosComite, ComiteTecnico, SesionComiteSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { forkJoin } from 'rxjs';
import { ColumnasTabla, ComiteFiduciario, DataTable, SolicitudContractual } from 'src/app/_interfaces/comiteFiduciario.interfaces';
import { DatePipe } from '@angular/common';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { TiposProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';

@Component({
  selector: 'app-crear-orden-del-dia',
  templateUrl: './crear-orden-del-dia.component.html',
  styleUrls: ['./crear-orden-del-dia.component.scss']
})

export class CrearOrdenDelDiaComponent implements OnInit {

  solicitudesContractuales: SolicitudesContractuales[] = [];
  //Data que sera recibida del servicio
  dataSolicitudContractual: SolicitudesContractuales[] = [];
  fechaSesion: Date;
  idSesion: number;
  detalle: string = 'Crear orden del día de comité fiduciario';
  listaTipoTemas: Dominio[] = [];
  solicitudBoolean: boolean = false;
  temaNuevoBoolean: boolean = false;
  tipoDeTemas: FormControl = new FormControl();
  solicitudesSeleccionadas = [];
  temasSeleccionadas = [];
  dataTemasNuevos: any[] = [];
  temasFromComite = [];

  estadosComite = EstadosComite;
  objetoComiteTecnico: ComiteTecnico;

  objetoSesion: ComiteFiduciario = {
    estadoComiteCodigo: this.estadosComite.sinConvocatoria
  };

  addressForm = this.fb.group({
    tema: this.fb.array([]),
  });



  responsablesArray: Dominio[] = [];
  estaEditando: boolean;

  constructor(private fb: FormBuilder,
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private datepipe: DatePipe,
    private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
    private commonService: CommonService,
    private techicalCommitteeSessionService: TechnicalCommitteSessionService,

  ) {
    this.getFecha();
  };


  ngOnInit(): void {
    this.idSesion = Number(this.activatedRoute.snapshot.params.id);



    forkJoin([
      this.commonService.listaMiembrosComiteTecnico(),
      this.fiduciaryCommitteeSessionService.getCommitteeSessionFiduciario(),
      this.commonService.listaTipoTema(),

    ]).subscribe(response => {
      this.responsablesArray = response[0];
      this.dataSolicitudContractual = response[1];
      this.listaTipoTemas = response[2].filter(t => t.codigo != "3");
      this.dataTemasNuevos = this.dataSolicitudContractual.filter(r => r?.temas?.length > 0);
      var data = this.dataTemasNuevos;
      this.dataTemasNuevos = [];
      data.forEach(r => {
        var temas = r.temas;
        r.temas = [];
        temas.forEach(t => {
          if(t?.comiteTecnicoFiduciarioIdMapped == this.idSesion || t?.comiteTecnicoFiduciarioIdMapped == null || t?.comiteTecnicoFiduciarioIdMapped == undefined){
            r.temas.push(t);
          }
        })
        if(r.temas.length > 0)
          this.dataTemasNuevos.push(r);
      })
      if (this.idSesion > 0)
        this.editMode();

    })

  }

  //Metodo para obtener la fecha recibida del componente ordenes del dia
  getFecha() {

    if (this.router.getCurrentNavigation().extras.replaceUrl) {
      this.router.navigateByUrl('/comiteFiduciario');
      return;
    };

    if (this.router.getCurrentNavigation().extras.state)
      this.fechaSesion = this.router.getCurrentNavigation().extras.state.fecha;

  };

  //Obteniendo valores booleanos para habilitar/deshabilitar "Solicitudes contractuales/Temas nuevos"
  getvalues(values: Dominio[]) {

    console.log(values);
    const solicitud = values.find(value => value.codigo == "1");
    const temaNuevo = values.find(value => value.codigo == "2");

    solicitud ? this.solicitudBoolean = true : this.solicitudBoolean = false;
    if (temaNuevo) {
      this.temaNuevoBoolean = true;
    } else {
      this.temaNuevoBoolean = false
    };

  };

  //Contador solicitudes seleccionadas
  totalSolicitudes() {
    let contador = 0;
    for (let solicitud of this.solicitudesSeleccionadas) {
      contador += solicitud.data.length;
    };
    return contador;
  }

  //Contador temas seleccionadas
  totalTemas() {
    let contador = 0;
    for (let solicitud of this.temasSeleccionadas) {
      contador += solicitud.temas.filter(r => r.seleccionado == true).length;
    };
    return contador;
  }

  //Metodo para recibir las solicitudes contractuales
  getSesionesSeleccionada(event: DataTable) {
    console.log(this.solicitudesSeleccionadas, event)
    if (event.estado) {
      const index = this.solicitudesSeleccionadas.findIndex(value => value.data[0].idSolicitud === event.solicitud.data[0].idSolicitud);
      if (index === -1) {
        this.solicitudesSeleccionadas.push(event.solicitud);
      } else {
        this.solicitudesSeleccionadas.splice(index, 1, event.solicitud);
      }
    } else {
      if (event.solicitud.data.length === 0) {
        console.log(this.solicitudesSeleccionadas, event)
        const index = this.solicitudesSeleccionadas.findIndex(value => value.data[0].idSolicitud === event.solicitud.data[0].idSolicitud);
        this.solicitudesSeleccionadas.splice(index, 1);
      } else {
        const index = this.solicitudesSeleccionadas.findIndex(value => value.data[0].idSolicitud === event.solicitud.data[0].idSolicitud);
        this.solicitudesSeleccionadas.splice(index, 1, event.solicitud);
      }
    };
    console.log('solicitudes seleccionadas: ', this.solicitudesSeleccionadas);
  };

    //Metodo para recibir los temas nuevos
    getTemasSeleccionados(event: any) {
      const index = this.temasSeleccionadas.findIndex(value => value.comiteTecnicoId === event.comiteTecnicoId);
      if (index === -1) {
        this.temasSeleccionadas.push(event);
      }else{
        this.temasSeleccionadas[index].temas = event.temas;
        if(!event.temas.find(r => r.seleccionado == true)){
          this.temasSeleccionadas.splice(index, 1);
        }
      }

    };

  editMode() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    this.detalle = 'Ver detalle orden del día';

    this.fiduciaryCommitteeSessionService.getRequestCommitteeSessionById(this.idSesion)
      .subscribe(comite => {
        console.log(comite)

        this.objetoComiteTecnico = comite;

        if (comite.tipoTemaFiduciarioCodigo == "3") {
          this.tipoDeTemas.setValue(this.listaTipoTemas);
        } else {
          let tipoTemaSeleccionado = this.listaTipoTemas.filter(t => t.codigo == comite.tipoTemaFiduciarioCodigo);
          //if ( tipoTemaSeleccionado )
          this.tipoDeTemas.setValue(tipoTemaSeleccionado);
        }

        this.getvalues(this.tipoDeTemas.value);

        let temas = this.addressForm.get('tema') as FormArray;

        temas.clear();
        this.solicitudesSeleccionadas = [];
        this.temasSeleccionadas = [];
        this.temasFromComite = [];

        comite.sesionComiteTema.filter(t => t.esProposicionesVarios != true && t.sesionTemaComiteTecnicoId == null).forEach(te => {
          let grupoTema = this.crearTema();
          let responsable = this.responsablesArray.find(m => m.codigo == te.responsableCodigo)

          grupoTema.get('tema').setValue(te.tema);
          grupoTema.get('responsable').setValue(responsable);
          grupoTema.get('tiempoIntervencion').setValue(te.tiempoIntervencion);
          grupoTema.get('url').setValue(te.rutaSoporte);
          grupoTema.get('sesionTemaId').setValue(te.sesionTemaId);


          this.tema.push(grupoTema);
        })

        this.temasFromComite = comite.sesionComiteTema.filter(t => t.sesionTemaComiteTecnicoId > 0);
        this.temasFromComite.forEach(sct => {
          this.dataTemasNuevos.forEach(dt => {
            dt.temas.filter(t => t.sesionTemaId == sct.sesionTemaComiteTecnicoId).forEach(tema =>{
                tema.seleccionado = true;
                if(!this.temasSeleccionadas.find(r => r.comiteTecnicoId ==  dt.comiteTecnicoId)){
                  this.temasSeleccionadas.push({
                    comiteTecnicoId: dt.comiteTecnicoId,
                    data: dt.data,
                    fecha: dt.fecha,
                    nombreSesion: dt.nombreSesion,
                    temas: dt.temas
                  });
                }
            });
          })
        })

        let existeComite = false;

        console.log(comite.sesionComiteSolicitudComiteTecnico.length)

        comite.sesionComiteSolicitudComiteTecnico.forEach(tf => {

          if ( this.dataSolicitudContractual.filter( r => r.comiteTecnicoId == tf.comiteTecnicoId ).length == 0 )
          {
            this.dataSolicitudContractual.push({

              comiteTecnicoId: tf['comiteTecnico'].comiteTecnicoId,
              fecha: tf['comiteTecnico'].fechaOrdenDia,
              nombreSesion: tf['comiteTecnico'].numeroComite,
              data: []


            })
          }

          this.dataSolicitudContractual.forEach(sc => {

            if (sc['comiteTecnicoId'] == tf.comiteTecnicoId) {

              sc.data.push({
                fechaSolicitud: tf.fechaSolicitud,
                id: 0,
                numeroSolicitud: tf.numeroSolicitud,
                sesionComiteSolicitudId: tf.sesionComiteSolicitudId,
                idSolicitud: tf.sesionComiteSolicitudId,
                tipoSolicitud: tf.tipoSolicitud,
                tipoSolicitudCodigo: tf.tipoSolicitudCodigo,
                tipoSolicitudNumeroTabla: tf.tipoSolicitudCodigo,
                ['seleccionado']: true
              })

              existeComite = true;
            }
          });


          // if (existeComite === false)
          // {
          //   this.dataSolicitudContractual.push({

          //     comiteTecnicoId: tf['comiteTecnico'].comiteTecnicoId,
          //     fecha: tf['comiteTecnico'].fechaOrdenDia,
          //     nombreSesion: tf['comiteTecnico'].numeroComite,
          //     data: [
          //       {
          //         fechaSolicitud: tf.fechaSolicitud,
          //         id: 0,
          //         numeroSolicitud: tf.numeroSolicitud,
          //         sesionComiteSolicitudId: tf.sesionComiteSolicitudId,
          //         idSolicitud: tf.sesionComiteSolicitudId,
          //         tipoSolicitud: tf.tipoSolicitud,
          //         tipoSolicitudCodigo: tf.tipoSolicitudCodigo,
          //         tipoSolicitudNumeroTabla: tf.tipoSolicitudCodigo,
          //         ['seleccionado']: true
          //       }
          //     ]

          //   })
          // }


          this.solicitudesSeleccionadas.push({
            nombreSesion: '',
            fecha: '',
            data: [{
              fechaSolicitud: '',
              id: 0,
              idSolicitud: tf.sesionComiteSolicitudId,
              numeroSolicitud: '',
              tipoSolicitud: tf.tipoSolicitud,
              tipoSolicitudNumeroTabla: tf.tipoSolicitudCodigo,
            }]
          });
        })

      })
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  get tema() {
    return this.addressForm.get('tema') as FormArray;
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  deleteTema(i) {
    let grupo = this.addressForm.get('tema') as FormArray;
    let tema = grupo.controls[i];

    console.log(tema, tema.get('sesionTemaId').value)

    this.techicalCommitteeSessionService.deleteSesionComiteTema(tema.get('sesionTemaId').value ? tema.get('sesionTemaId').value : 0)
      .subscribe(respuesta => {
        this.borrarArray(grupo, i)
        this.openDialog('', '<b>La información ha sido eliminada correctamente.</b>')
        this.ngOnInit();
      })

  }

  openDialogSiNo(modalTitle: string, modalText: string, e: number, grupo: any) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result === true) {
        this.deleteTema(e)
      }
    });
  }

  eliminarTema(i) {
    let tema = this.addressForm.get('tema');
    this.openDialogSiNo('', '<b>¿Está seguro de eliminar este registro?</b>', i, tema);
  }

  agregaTema() {
    this.tema.push(this.crearTema());
    if (this.estaEditando) this.addressForm.markAllAsTouched();
  }

  crearTema() {
    return this.fb.group({
      sesionTemaId: [],
      tema: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(1000)])
      ],
      responsable: [null, Validators.required],
      tiempoIntervencion: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(3)])
      ],
      url: [null, [
        Validators.required,
        //Validators.pattern('/^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/')
      ]],
    });
  }

  getStyle() {
    if ( this.idSesion == 0 || this.estadosComite.sinConvocatoria == this.objetoComiteTecnico?.estadoComiteCodigo)
      return 'auto'
    else
      return 'none'
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    // console.log(this.solicitudesSeleccionadas);
    if (this.addressForm.invalid) {
      this.openDialog('', '<b>Falta registrar información</b>');

    } else {

      let tipoTema: string = null;
      if (this.tipoDeTemas.value) {
        if (this.tipoDeTemas.value.length == 1)
          tipoTema = this.tipoDeTemas.value[0].codigo;
        else if (this.tipoDeTemas.value.length == 2)
          tipoTema = "3";
      }

      let sesion: ComiteTecnico = {
        comiteTecnicoId: this.idSesion,
        fechaOrdenDia: this.fechaSesion,
        tipoTemaFiduciarioCodigo: tipoTema,
        sesionComiteTema: [],
        sesionComiteSolicitudComiteTecnico: [],
      }
      console.log(this.tema);

      this.tema.controls.forEach(control => {
        let sesionComiteTema: SesionComiteTema = {
          sesionTemaId: control.get('sesionTemaId').value,
          comiteTecnicoId: this.idSesion,
          tema: control.get('tema').value,
          responsableCodigo: control.get('responsable').value.codigo,
          tiempoIntervencion: control.get('tiempoIntervencion').value,
          rutaSoporte: control.get('url').value,

        }

        sesion.sesionComiteTema.push(sesionComiteTema);
      })

      this.temasFromComite.forEach(tfc => {
        var t = this.temasSeleccionadas.filter(r => r.temas.find(r => r.sesionTemaId == tfc.sesionTemaComiteTecnicoId));
        if(t.length == 0){
          let sesionComiteTema: SesionComiteTema = {
            sesionTemaId: tfc.sesionTemaId,
            comiteTecnicoId: tfc.comiteTecnicoId,
            tema: tfc.tema,
            responsableCodigo: tfc.responsableCodigo,
            tiempoIntervencion: tfc.tiempoIntervencion,
            rutaSoporte: tfc.rutaSoporte,
            eliminado: true
           }
          sesion.sesionComiteTema.push(sesionComiteTema);
        }
     })

      this.temasSeleccionadas.forEach(ts => {
        ts.temas.forEach(sol => {
          let sesionComiteTema: SesionComiteTema = {
            sesionTemaId: this.temasFromComite.find(r => r.sesionTemaComiteTecnicoId == sol.sesionTemaId)?.sesionTemaId ?? 0,
            comiteTecnicoId: sol.comiteTecnicoId,
            sesionTemaComiteTecnicoId: sol.sesionTemaId,
            tema: sol.tema,
            responsableCodigo: sol.responsableCodigo,
            tiempoIntervencion: sol.tiempoIntervencion,
            rutaSoporte: sol.rutaSoporte,
            eliminado: sol.seleccionado == true ? false : true ?? false
           }
          if(!(sesionComiteTema.sesionTemaId == 0 && sesionComiteTema.eliminado == true))
            sesion.sesionComiteTema.push(sesionComiteTema);
        });
     })

     this.solicitudesSeleccionadas.forEach(ss => {
        ss.data.forEach(sol => {
          let sesionSol: SesionComiteSolicitud = {
            sesionComiteSolicitudId: sol.idSolicitud,
          }

          sesion.sesionComiteSolicitudComiteTecnico.push(sesionSol);
        });

      })

      console.log(sesion);

      this.fiduciaryCommitteeSessionService.createEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud(sesion)
        .subscribe(respuesta => {
          this.openDialog('', `<b>${respuesta.message}</b>`);
          if (respuesta.code == "200")
            this.router.navigate(['/comiteFiduciario'])
      })

    }
  }
}
