import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { SolicitudesContractuales, ComiteTecnico, SesionComiteTema, EstadosComite, SesionComiteSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { forkJoin } from 'rxjs';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { DataTable } from 'src/app/_interfaces/comiteFiduciario.interfaces';

@Component({
  selector: 'app-crear-orden-del-dia',
  templateUrl: './crear-orden-del-dia.component.html',
  styleUrls: ['./crear-orden-del-dia.component.scss']
})

export class CrearOrdenDelDiaComponent implements OnInit {

  listaMiembros: Dominio[] = [];
  solicitudesContractuales: SolicitudesContractuales[] = [];
  dataSolicitudContractual: SolicitudesContractuales[] = [];
  fechaSesionString: string;
  fechaSesion: Date;
  idComite: number = 0;
  estadosComite = EstadosComite;
  objetoComiteTecnico: ComiteTecnico = {
    estadoComiteCodigo: this.estadosComite.sinConvocatoria
  };
  tipoDeTemas: FormControl = new FormControl();
  listaTipoTemas: Dominio[] = [];
  solicitudBoolean: boolean = false;
  temaNuevoBoolean: boolean = false;
  solicitudesSeleccionadas = [];
  numeroComite = "";
  addressForm = this.fb.group({
    tema: this.fb.array([]),
  });
  temasString = "";

  estaEditando = false;
  verDetalle = false;
  seleccionarTodos: boolean = false;

  responsablesArray = [
    { name: 'reponsable 1', value: '1' },
    { name: 'reponsable 2', value: '2' },
    { name: 'reponsable 3', value: '3' }
  ];

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
    private techicalCommitteeSessionService: TechnicalCommitteSessionService,
    private router: Router,
    private commonService: CommonService,

  ) {
    this.getFecha();
  }


  ngOnInit(): void {

    this.idComite = Number(this.activatedRoute.snapshot.params.id);
      if(this.verDetalle != true){
        forkJoin([
          this.techicalCommitteeSessionService.getListSolicitudesContractuales(this.fechaSesionString),
          this.commonService.listaMiembrosComiteTecnico(),
          this.commonService.listaTipoTema(),

        ]).subscribe(response => {

          this.solicitudesContractuales = response[0];
          this.solicitudesContractuales.forEach(sc => {
            sc.tipoSolicitudCodigo = sc.tipoSolicitudNumeroTabla

          })
          this.dataSolicitudContractual = this.solicitudesContractuales;

          this.listaMiembros = response[1];
          this.listaTipoTemas = response[2].filter(t => t.codigo != "3");

         /*setTimeout(() => {

            let btnTablaSolicitudes = document.getElementById('btnTablaSolicitudes');
            btnTablaSolicitudes.click();

          }, 1000);*/
          if (this.idComite > 0)
            this.editMode();

        });
      }else{
        if (this.idComite > 0)
          this.editMode();
      }
  }

  editMode() {
    this.temasString = '';

    if(this.verDetalle == true){
      this.commonService.listaTipoTema().subscribe(response =>{
        this.listaTipoTemas = response.filter(t => t.codigo != "3");
      });
    }

    forkJoin([
      //this.techicalCommitteeSessionService.getListSesionComiteTemaByIdSesion( this.idSesion ),
      this.techicalCommitteeSessionService.getComiteTecnicoByComiteTecnicoId(this.idComite),
      this.commonService.listaMiembrosComiteTecnico(),

    ]).subscribe(response => {

      this.objetoComiteTecnico = response[0];
      this.numeroComite = this.objetoComiteTecnico ?.numeroComite;
      this.listaMiembros = response[1];

      if (this.objetoComiteTecnico?.tipoTemaFiduciarioCodigo == "3") {
        this.tipoDeTemas.setValue(this.listaTipoTemas);
      } else {
        let tipoTemaSeleccionado = this.listaTipoTemas.filter(t => t.codigo == this.objetoComiteTecnico?.tipoTemaFiduciarioCodigo);
        //if ( tipoTemaSeleccionado )
        this.tipoDeTemas.setValue(tipoTemaSeleccionado);
      }

      this.tipoDeTemas.value.forEach(tema => {
        if(this.temasString != ''){
          this.temasString = this.temasString + ", " + tema.nombre;
        }else{
          this.temasString = tema.nombre;
        }
      });

      this.getvalues(this.tipoDeTemas.value);

      /**
       * TEMAS
       */

      let temas = this.addressForm.get('tema') as FormArray;

      temas.clear();
      this.solicitudesSeleccionadas = [];

      response[0].sesionComiteTema = response[0].sesionComiteTema.filter(t => t.esProposicionesVarios != true)

      response[0].sesionComiteTema.forEach(te => {
        let grupoTema = this.crearTema();
        let responsable = this.listaMiembros.find(m => m.codigo == te.responsableCodigo)

        grupoTema.get('tema').setValue(te.tema);
        grupoTema.get('responsable').setValue(responsable);
        grupoTema.get('tiempoIntervencion').setValue(te.tiempoIntervencion);
        grupoTema.get('url').setValue(te.rutaSoporte);
        grupoTema.get('sesionTemaId').setValue(te.sesionTemaId);


        temas.push(grupoTema);

      });

      this.solicitudesContractuales = response[0].sesionComiteSolicitudComiteTecnico.filter(r => r.eliminado != true);
      if (this.solicitudesContractuales) {
        this.solicitudesContractuales.forEach(sc => {
          sc.id = sc.solicitudId;
          sc.seleccionado = true;

          if(this.dataSolicitudContractual.length > 0 ){
            this.dataSolicitudContractual.forEach(ds => {
              if(ds.id != sc.solicitudId){
                this.dataSolicitudContractual.push(sc)
              }
            });
          }else{
            this.dataSolicitudContractual.push(sc)
          }

          this.solicitudesSeleccionadas.push({
            nombreSesion: '',
            fecha: '',
            fechaSolicitud: sc.fechaSolicitud,
            id: sc.id,
            idSolicitud: sc.sesionComiteSolicitudId ?? 0,
            numeroSolicitud: '',
            tipoSolicitud: sc.tipoSolicitud,
            tipoSolicitudCodigo: sc.tipoSolicitudCodigo,
            seleccionado: sc.seleccionado
          });

        });
      }
      if(this.verDetalle == true){
        setTimeout(() => {

          let btnTablaSolicitudes = document.getElementById('btnTablaSolicitudes');
          btnTablaSolicitudes.click();

        }, 1000);
      }

    })
  }

  eliminarTema(i) {
    let tema = this.addressForm.get('tema');
    this.openDialogSiNo('', '<b>¿Está seguro de eliminar este registro?</b>', i, tema);

  }

  deleteTema(i) {
    let grupo = this.addressForm.get('tema') as FormArray;
    let tema = grupo.controls[i];

    this.techicalCommitteeSessionService.deleteSesionComiteTema(tema.get('sesionTemaId').value)
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
      if (result===true) {
        this.deleteTema(e)
      }
    });
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

  agregaTema() {
    this.tema.push(this.crearTema());
  }

  getStyle() {
    if (this.estadosComite.sinConvocatoria == this.objetoComiteTecnico.estadoComiteCodigo)
      return 'auto'
    else
      return 'none'
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

  onSubmit() {
    this.estaEditando=true;
    this.addressForm.markAllAsTouched();

    // console.log(this.addressForm);
    if (this.addressForm.invalid && this.temaNuevoBoolean) {
      this.openDialog('', '<b>Falta registrar información</b>');

    } else {
      //deje el mismo campo de fiduciaria, de igual forma los distingue EsComiteFiduciario
      let tipoTema: string = null;
      if (this.tipoDeTemas.value) {
        if (this.tipoDeTemas.value.length == 1)
          tipoTema = this.tipoDeTemas.value[0].codigo;
        else if (this.tipoDeTemas.value.length == 2)
          tipoTema = "3";
      }

      let comite: ComiteTecnico = {
        comiteTecnicoId: this.idComite,
        fechaOrdenDia: this.fechaSesion,
        sesionComiteTema: [],
        sesionComiteSolicitudComiteTecnico: [],
        tipoTemaFiduciarioCodigo: tipoTema
      }

      this.tema.controls.forEach(control => {
        let sesionComiteTema: SesionComiteTema = {
          tema: control.get('tema').value,
          responsableCodigo: control.get('responsable').value.codigo,
          tiempoIntervencion: control.get('tiempoIntervencion').value,
          rutaSoporte: control.get('url').value,
          sesionTemaId: control.get('sesionTemaId').value,
          comiteTecnicoId: this.idComite,

        }

        comite.sesionComiteTema.push(sesionComiteTema);
      })

      if (this.solicitudesSeleccionadas.length > 0) {
        this.solicitudesSeleccionadas.forEach(sol => {
          let sesionComiteSolicitud: SesionComiteSolicitud = {
            comiteTecnicoId: this.idComite,
            solicitudId: sol.id,
            sesionComiteSolicitudId: sol.sesionComiteSolicitudId,
            tipoSolicitudCodigo: sol.tipoSolicitudCodigo,
          }

          comite.sesionComiteSolicitudComiteTecnico.push(sesionComiteSolicitud);
        })
      }


      this.techicalCommitteeSessionService.createEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud(comite).subscribe(respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`)
        if (respuesta.code == "200")
          this.router.navigate(['/comiteTecnico']);
      });
    }
  }

  //Obteniendo valores booleanos para habilitar/deshabilitar "Solicitudes contractuales/Temas nuevos"
  getvalues(values: Dominio[]) {

    const solicitud = values.find(value => value.codigo == "1");
    const temaNuevo = values.find(value => value.codigo == "2");

    if(solicitud){
      this.solicitudBoolean = true;
    }else{
      this.solicitudBoolean = false;
      this.solicitudesSeleccionadas = [];
      if(this.dataSolicitudContractual.length > 0 ){
        this.dataSolicitudContractual.forEach(ds => {
          ds.seleccionado = false;
        });
      }
    }

    if (temaNuevo) {
      this.temaNuevoBoolean = true;
    } else {
      this.temaNuevoBoolean = false
    };

    };

  //Metodo para obtener la fecha recibida del componente ordenes del dia
  getFecha() {

    if (this.router.getCurrentNavigation().extras.replaceUrl) {
      this.router.navigateByUrl('/comiteTecnico');
      return;
    };

    if (this.router.getCurrentNavigation().extras.state)
      this.fechaSesion = this.router.getCurrentNavigation().extras.state.fecha;
      this.verDetalle = this.router.getCurrentNavigation().extras.state.verDetalle ?? false;
      if(this.fechaSesion != null && this.fechaSesion != undefined && this.router.getCurrentNavigation().extras.state.fecha != ''){
        try{
          this.fechaSesionString = `${this.fechaSesion.getFullYear()}/${this.fechaSesion.getMonth() + 1}/${this.fechaSesion.getDate()}`
        }
        catch(e){
          this.fechaSesionString = this.router.getCurrentNavigation().extras.state.fecha;
        }
      }

  };

  //Contador solicitudes seleccionadas
  totalSolicitudes() {
    let contador = this.solicitudesSeleccionadas.length;
    return contador;
  }

  //Metodo para recibir las solicitudes contractuales
  getSesionesSeleccionada(event: DataTable) {
    const index = this.solicitudesSeleccionadas.findIndex(value => value.id === event.solicitud.id && value.tipoSolicitudCodigo === event.solicitud.tipoSolicitudCodigo);
    const data = this.dataSolicitudContractual.find(value => value.id === event.solicitud.id && value.tipoSolicitudCodigo === event.solicitud.tipoSolicitudCodigo);

    if(data != null){
      if(event.estado != true){
        if (index !== -1) {
          this.solicitudesSeleccionadas.splice(index, 1);
          data.seleccionado = false;
        }
      }else{
        if (index === -1) {
          let solicitud = data;
          data.seleccionado = true;
          this.solicitudesSeleccionadas.push(solicitud ?? event.solicitud);
        }
      }
    }

    if(this.solicitudesSeleccionadas.length == this.dataSolicitudContractual.length && this.solicitudesSeleccionadas.length > 0){
      this.seleccionarTodos = true;
    }else{
      this.seleccionarTodos = false;
    }
  };

  selectAll(event: boolean) {
    if(event != true){
      this.seleccionarTodos = false;
      this.solicitudesSeleccionadas = [];
      this.dataSolicitudContractual.forEach(solicitud => {
        solicitud.seleccionado = false;
      });
    }
    else{
      this.seleccionarTodos = true;
      this.dataSolicitudContractual.forEach(solicitud => {
        solicitud.seleccionado = true;
        //validar si existe
        const index = this.solicitudesSeleccionadas.findIndex(value => value.id === solicitud.id && value.tipoSolicitudCodigo === solicitud.tipoSolicitudCodigo);
        if (index === -1) {
          this.solicitudesSeleccionadas.push(solicitud);
        }
      });
    }
  };

  validateButton(){
    //si seleccionaron temas, que exista al menos uno
    if(this.temaNuevoBoolean && this.tema.controls.length <= 0){
      return true;
    }
    //si seleccionaron solicitudes contractuales al menos una este seleccionada
    if(this.solicitudBoolean && this.solicitudesSeleccionadas.length <= 0){
      return true;
    }

    return false;
  };

}

