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
  estadosComite = EstadosComite;
  objetoSesion: ComiteFiduciario = {
    estadoComiteCodigo: this.estadosComite.sinConvocatoria
  };

  addressForm = this.fb.group({
    tema: this.fb.array([]),
  });



  responsablesArray: Dominio[] = [];

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

  //Metodo para recibir las solicitudes contractuales
  getSesionesSeleccionada(event: DataTable) {

    if (event.estado) {

      const index = this.solicitudesSeleccionadas.findIndex(value => value.data[0].idSolicitud === event.solicitud.data[0].idSolicitud);

      if (index === -1) {
        this.solicitudesSeleccionadas.push(event.solicitud);
      } else {
        this.solicitudesSeleccionadas.splice(index, 1, event.solicitud);
      }


    } else {
      console.log( this.solicitudesSeleccionadas, event )


      if (event.solicitud.data.length === 0) {

        const index = this.solicitudesSeleccionadas.findIndex(value => value.data[0].idSolicitud === event.solicitud.data[0].idSolicitud);

        this.solicitudesSeleccionadas.splice(index, 1);

      } else {

        const index = this.solicitudesSeleccionadas.findIndex(value => value.data[0].idSolicitud === event.solicitud.data[0].idSolicitud);
        this.solicitudesSeleccionadas.splice(index, 1, event.solicitud);

      }

    };

    console.log('solicitudes seleccionadas: ', this.solicitudesSeleccionadas);

  };

  editMode() {

    this.detalle = 'Ver detalle/Editar orden del día';

    this.fiduciaryCommitteeSessionService.getRequestCommitteeSessionById(this.idSesion)
      .subscribe(comite => {
        console.log(comite)

        if (comite.tipoTemaFiduciarioCodigo == "3") {
          this.tipoDeTemas.setValue(this.listaTipoTemas);
        } else {
          let tipoTemaSeleccionado = this.listaTipoTemas.filter(t => t.codigo == comite.tipoTemaFiduciarioCodigo);
          //if ( tipoTemaSeleccionado )
          this.tipoDeTemas.setValue(tipoTemaSeleccionado);
        }

        this.getvalues( this.tipoDeTemas.value );

        let temas = this.addressForm.get('tema') as FormArray;

        temas.clear();
        this.solicitudesSeleccionadas = [];
  
        comite.sesionComiteTema.filter(t => t.esProposicionesVarios != true).forEach(te => {
          let grupoTema = this.crearTema();
          let responsable = this.responsablesArray.find(m => m.codigo == te.responsableCodigo)

          grupoTema.get('tema').setValue(te.tema);
          grupoTema.get('responsable').setValue(responsable);
          grupoTema.get('tiempoIntervencion').setValue(te.tiempoIntervencion);
          grupoTema.get('url').setValue(te.rutaSoporte);
          grupoTema.get('sesionTemaId').setValue(te.sesionTemaId);


          this.tema.push(grupoTema);
        })

        comite.sesionComiteSolicitudComiteTecnicoFiduciario.forEach( tf => {
          this.solicitudesSeleccionadas.push( { nombreSesion: '',
                                                fecha: '',
                                                data:[{ fechaSolicitud: '',
                                                        id: 0, 
                                                        idSolicitud: tf.sesionComiteSolicitudId,
                                                        numeroSolicitud: '',
                                                        tipoSolicitud: tf.tipoSolicitud,
                                                        tipoSolicitudNumeroTabla: tf.tipoSolicitudCodigo
                                                      }]
                                              } );
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
        this.openDialog('', '<b>La información se ha eliminado correctamente.</b>')
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
      if (result===true) {
        this.deleteTema(e)
      }
    });
  }

  eliminarTema(i) {
    let tema = this.addressForm.get('tema');
    this.openDialogSiNo('', '¿Está seguro de eliminar este registro?', i, tema);
  }

  agregaTema() {
    this.tema.push(this.crearTema());
  }

  crearTema() {
    return this.fb.group({
      sesionTemaId: [],
      tema: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ],
      responsable: [null, Validators.required],
      tiempoIntervencion: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(3)])
      ],
      url: [null, [
        //Validators.required,
        //Validators.pattern('/^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/')
      ]],
    });
  }

  onSubmit() {

    console.log(this.solicitudesSeleccionadas);
    if (this.addressForm.invalid) {
      this.openDialog('Falta registrar información', '');

    } else {

      let tipoTema: string = null;
      if ( this.tipoDeTemas.value ){
        if (this.tipoDeTemas.value.length == 1 )
          tipoTema = this.tipoDeTemas.value[0].codigo;
        else if (this.tipoDeTemas.value.length == 2 )
          tipoTema = "3";
      }

      let sesion: ComiteTecnico = {
        comiteTecnicoId: this.idSesion,
        fechaOrdenDia: this.fechaSesion,
        tipoTemaFiduciarioCodigo: tipoTema ,
        sesionComiteTema: [],
        sesionComiteSolicitudComiteTecnico: [],
      }

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

      this.solicitudesSeleccionadas.forEach( ss =>{
        ss.data.forEach(sol => {
          let sesionSol: SesionComiteSolicitud = {
            sesionComiteSolicitudId: sol.idSolicitud,
          }

          sesion.sesionComiteSolicitudComiteTecnico.push( sesionSol );
        });

      })

      console.log(sesion);

      this.fiduciaryCommitteeSessionService.createEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud(sesion)
        .subscribe(respuesta => {
          this.openDialog('', respuesta.message);
          if (respuesta.code == "200")
            this.router.navigate(['/comiteFiduciario'])
        })

    }
  }
}
