import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CreatePolizaGarantia, CreatePolizaObservacion, EditPoliza, InsertPoliza, PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
@Component({
  selector: 'app-editar-en-revision',
  templateUrl: './editar-en-revision.component.html',
  styleUrls: ['./editar-en-revision.component.scss']
})
export class EditarEnRevisionComponent implements OnInit, OnDestroy {

  addressForm = this.fb.group({
    nombre: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(50)])
    ],
    numeroPoliza: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(20)])
    ],
    numeroCertificado: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(20)])
    ],
    fecha: [null, Validators.required],
    vigenciaPoliza: [null, Validators.required],
    vigenciaAmparo: [null, Validators.required],
    valorAmparo: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(20)])
    ],
    polizasYSeguros: [null, Validators.required],
    buenManejoCorrectaInversionAnticipo: [null],
    estabilidadYCalidad: [null],
    polizaYCoumplimiento: [null],
    polizasYSegurosCompleto: [null],
    cumpleAsegurado: [null, Validators.required],
    cumpleBeneficiario: [null, Validators.required],
    cumpleAfianzado: [null, Validators.required],
    reciboDePago: [null, Validators.required],
    condicionesGenerales: [null, Validators.required],
    fechaRevision: [null, Validators.required],
    estadoRevision: [null, Validators.required],
    fechaAprob: ['', Validators.required],
    responsableAprob: ['', Validators.required],
    observacionesGenerales: ['']
  });

  polizasYSegurosArray: Dominio[] = [];
  estadoArray = [];/*
    { name: 'Devuelta', value: '1' },
    { name: 'Aprobada', value: '2' }
  ];*/
  aprobadosArray = [
    { name: 'Andres Montealegre', value: '1' },
    { name: 'David Benitez', value: '2' }
  ];
  minDate: Date;

  editorStyle = {
    height: '100px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  public tipoContrato;
  public objeto;
  public nombreContratista;
  public tipoIdentificacion;
  public numeroIdentificacion;
  public valorContrato;
  public plazoContrato;
  public numContrato;
  public observacionesOn;
  public idContrato;
  public idPoliza;
  public idPoliza2;
  public idObservacion;
  public selected = [];
  public obj1;
  public obj2;
  public arrayprueba = ["1", "2"];
  arrayGarantias: any[] = [];
  obj3: boolean;
  obj4: boolean;
  fechaFirmaContrato: any;
  tipoSolicitud: any;
  ultimoEstadoRevision: any;
  ultimaFechaRevision: any;
  listaUsuarios: any[] = [];
  realizoPeticion: boolean = false;

  constructor(
    private router: Router,
    private polizaService: PolizaGarantiaService,
    private fb: FormBuilder,
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
    private common: CommonService,
    private contratacion: ProjectContractingService
  ) {
    this.minDate = new Date();
  }
  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.loadContrato(param.id);
      this.loadData(param.id);
    });
  }
  ngOnDestroy(): void {
    if (this.addressForm.dirty === true && this.realizoPeticion === false) {
      this.openDialogConfirmar('', '¿Desea guardar la información registrada?');
    }
  };

  openDialogConfirmar(modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    confirmarDialog.afterClosed()
      .subscribe(response => {
        if (response === true) {
          this.onSubmit();
        }
      });
  };
  loadContrato(id) {
    this.polizaService.GetListVistaContratoGarantiaPoliza(id).subscribe(data => {
      this.fechaFirmaContrato = data[0].fechaFirmaContrato;
      this.tipoSolicitud = data[0].tipoSolicitud;
      this.numContrato = data[0].numeroContrato;
      this.tipoIdentificacion = data[0].tipoDocumento;
      if (data[0].objetoContrato != undefined || data[0].objetoContrato != null) {
        this.objeto = data[0].objetoContrato;
      }
      else {
        this.objeto = '';
      };
      this.tipoContrato = data[0].tipoContrato;
      if (data[0].valorContrato != undefined || data[0].valorContrato != null) {
        this.valorContrato = data[0].valorContrato;
      }
      else {
        this.valorContrato = 0;
      }
      this.plazoContrato = data[0].plazoContrato;
      this.loadContratacionId(data[0].contratacionId);
    });
    this.common.listaGarantiasPolizas().subscribe(data0 => {
      this.polizasYSegurosArray = data0;
    });
    this.common.getUsuariosByPerfil(10).subscribe(resp => {
      this.listaUsuarios = resp;
    });
    this.common.listaEstadoRevision().subscribe(resp=>{
      this.estadoArray=resp;
    })
  }
  loadData(id) {
    this.polizaService.GetContratoPolizaByIdContratoId(id).subscribe(data => {
      this.addressForm.get('nombre').setValue(data.nombreAseguradora);
      this.addressForm.get('numeroPoliza').setValue(data.numeroPoliza);
      this.addressForm.get('numeroCertificado').setValue(data.numeroCertificado);
      this.addressForm.get('observacionesGenerales').setValue(data.observacionesRevisionGeneral);
      this.addressForm.get('fecha').setValue(data.fechaExpedicion);
      this.addressForm.get('fechaAprob').setValue(data.fechaAprobacion);
      this.addressForm.get('cumpleAsegurado').setValue(data.cumpleDatosAsegurado);
      this.addressForm.get('cumpleBeneficiario').setValue(data.cumpleDatosBeneficiario);
      this.addressForm.get('cumpleAfianzado').setValue(data.cumpleDatosTomador);
      this.addressForm.get('reciboDePago').setValue(data.incluyeReciboPago);
      this.addressForm.get('condicionesGenerales').setValue(data.incluyeCondicionesGenerales);
      this.addressForm.get('vigenciaPoliza').setValue(data.vigencia);
      this.addressForm.get('vigenciaAmparo').setValue(data.vigenciaAmparo);
      this.addressForm.get('valorAmparo').setValue(data.valorAmparo);
      for (let i = 0; i < this.listaUsuarios.length; i++) {
        const responAprob = this.listaUsuarios.find(p => p.usuarioId === parseInt(data.responsableAprobacion));
        this.addressForm.get('responsableAprob').setValue(responAprob);
      }
      this.dataLoad2(data);
    });
  }
  dataLoad2(data) {
    this.idContrato = data.contratoId;
    this.idPoliza = data.contratoPolizaId;
    this.loadGarantia(this.idPoliza);
    this.loadObservations(this.idPoliza);
    this.loadEstadoRevision(this.idPoliza);
  }
  loadEstadoRevision(id) {
    this.polizaService.GetListPolizaObservacionByContratoPolizaId(id).subscribe(data => {
      for (let i = 0; i < data.length; i++) {
        const estadoRevSeleccionado = this.estadoArray.find(t => t.codigo === data[i].estadoRevisionCodigo);
        this.addressForm.get('fechaRevision').setValue(data[i].fechaRevision);
        this.addressForm.get('estadoRevision').setValue(estadoRevSeleccionado);
      }
    });
  }
  loadContratacionId(a) {
    this.contratacion.getContratacionByContratacionId(a).subscribe(data => {
      this.loadInfoContratacion(data);
    });
  }
  loadInfoContratacion(data) {
    this.nombreContratista = data.contratista.nombre;
    this.numeroIdentificacion = data.contratista.numeroIdentificacion;

  }
  loadObservations(id) {
    this.polizaService.GetListPolizaObservacionByContratoPolizaId(id).subscribe(data_1 => {
      this.polizaService.loadTableObservaciones.next(data_1);
      for (let i = 0; i < data_1.length; i++) {
        this.ultimoEstadoRevision = data_1[i].estadoRevisionCodigo;
        this.ultimaFechaRevision = data_1[i].fechaRevision;
      }
      const estadoRevisionCodigo = this.estadoArray.find(p => p.codigo === this.ultimoEstadoRevision);
      this.addressForm.get('fechaRevision').setValue(this.ultimaFechaRevision);
      this.addressForm.get('estadoRevision').setValue(estadoRevisionCodigo);
    });
  }
  loadGarantia(id) {
    this.polizaService.GetListPolizaGarantiaByContratoPolizaId(id).subscribe(data => {
      const tipoGarantiaCodigo = [];
      this.arrayGarantias = data;
      if (this.arrayGarantias.length > 0) {
        const polizasListRead = [this.arrayGarantias[0].tipoGarantiaCodigo];
        for (let i = 1; i < this.arrayGarantias.length; i++) {
          const Garantiaaux = polizasListRead.push(this.arrayGarantias[i].tipoGarantiaCodigo);
        }
        for (let i = 0; i < polizasListRead.length; i++) {
          const polizaSeleccionada = this.polizasYSegurosArray.filter(t => t.codigo === polizasListRead[i]);
          if (polizaSeleccionada.length > 0) { tipoGarantiaCodigo.push(polizaSeleccionada[0]) };
        }
        this.addressForm.get('polizasYSeguros').setValue(tipoGarantiaCodigo);
        for (let j = 0; j < polizasListRead.length; j++) {
          switch (polizasListRead[j]) {
            case '1':
              this.obj1 = true;
              this.addressForm.get('buenManejoCorrectaInversionAnticipo').setValue(this.arrayGarantias[j].esIncluidaPoliza);
              break;
            case '2':
              this.obj2 = true;
              this.addressForm.get('estabilidadYCalidad').setValue(this.arrayGarantias[j].esIncluidaPoliza);
              break;
            case '3':
              this.obj3 = true;
              this.addressForm.get('polizaYCoumplimiento').setValue(this.arrayGarantias[j].esIncluidaPoliza);
              break;
            case '4':
              this.obj4 = true;
              this.addressForm.get('polizasYSegurosCompleto').setValue(this.arrayGarantias[j].esIncluidaPoliza);
              break;
          }
        }
      };
    });
  }

  loadGrantiaID(id) {
    if (id != undefined) {
      this.idPoliza2 = id;
    }
    else {
      this.idPoliza2 = undefined;
    }
  }
  loadObservacionId2(id) {
    if (id != undefined) {
      this.idObservacion = id;
    }
    else {
      this.idObservacion = undefined;
    }
  }

  getvalues(values: Dominio[]) {
    const buenManejo = values.find(value => value.codigo == "1");
    const garantiaObra = values.find(value => value.codigo == "2");
    const pCumplimiento = values.find(value => value.codigo == "3");
    const polizasYSeguros = values.find(value => value.codigo == "4");

    buenManejo ? this.obj1 = true : this.obj1 = false;
    garantiaObra ? this.obj2 = true : this.obj2 = false;
    pCumplimiento ? this.obj3 = true : this.obj3 = false;
    polizasYSeguros ? this.obj4 = true : this.obj4 = false;

  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }
  clickedOption() {
    console.log(this.selected)
  }
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p>');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li>');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    let polizasList;
    if (this.addressForm.value.polizasYSeguros != undefined || this.addressForm.value.polizasYSeguros != null) {
      polizasList = [this.addressForm.value.polizasYSeguros[0].codigo];
      for (let i = 1; i < this.addressForm.value.polizasYSeguros.length; i++) {
        const membAux = polizasList.push(this.addressForm.value.polizasYSeguros[i].codigo);
      }
    }
    let nombreAprobado;
    if (this.addressForm.value.responsableAprob != undefined || this.addressForm.value.responsableAprob != null) {
      if (!this.addressForm.value.responsableAprob.usuarioId) {
        nombreAprobado = "pendiente";
      }
      else {
        nombreAprobado = this.addressForm.value.responsableAprob.usuarioId;
      }
    }
    var statePoliza = "2";
    let revEstado;
    if (this.addressForm.value.estadoRevision != undefined || this.addressForm.value.estadoRevision != null) {
      revEstado = this.addressForm.value.estadoRevision.codigo;
      if (revEstado == "1") {
        statePoliza = "3";
      }
      else {
        statePoliza = "2";
      }
    }
    var completo: boolean;
    if (this.addressForm.valid) {
      completo = true;
    }
    else {
      completo = false;
    }
    console.log(this.addressForm.value);
    console.log(nombreAprobado);
    const contratoArray = {
      'contratoId': this.idContrato,
      "contratoPolizaId": this.idPoliza,
      'TipoSolicitudCodigo': "",
      'TipoModificacionCodigo': "",
      'DescripcionModificacion': "",
      'NombreAseguradora': this.addressForm.value.nombre,
      'NumeroPoliza': this.addressForm.value.numeroPoliza,
      'NumeroCertificado': this.addressForm.value.numeroCertificado,
      'Observaciones': "",
      'ObservacionesRevisionGeneral': this.addressForm.value.observacionesGenerales,
      'ResponsableAprobacion': nombreAprobado,
      'EstadoPolizaCodigo': statePoliza,
      //'UsuarioCreacion': "",
      //'UsuarioModificacion': "",
      'FechaExpedicion': this.addressForm.value.fecha,
      'Vigencia': this.addressForm.value.vigenciaPoliza,
      'VigenciaAmparo': this.addressForm.value.vigenciaAmparo,
      'ValorAmparo': this.addressForm.value.valorAmparo,
      'CumpleDatosAsegurado': this.addressForm.value.cumpleAsegurado,
      'CumpleDatosBeneficiario': this.addressForm.value.cumpleBeneficiario,
      'CumpleDatosTomador': this.addressForm.value.cumpleAfianzado,
      'IncluyeReciboPago': this.addressForm.value.reciboDePago,
      'IncluyeCondicionesGenerales': this.addressForm.value.condicionesGenerales,
      'FechaAprobacion': this.addressForm.value.fechaAprob,
      'Estado': false,
      //'FechaCreacion': "",
      'RegistroCompleto': completo,
      //'FechaModificacion': "",
      'Eliminado': false
    };
    const observacionArray = {
      'contratoId': this.idContrato,
      "contratoPolizaId": this.idPoliza,
      "Observacion": this.addressForm.value.observacionesGenerales,
      "FechaRevision": this.addressForm.value.fechaRevision,
      "EstadoRevisionCodigo": revEstado
    }
    let garantiaArray;
    if (this.addressForm.value.polizasYSeguros != undefined || this.addressForm.value.polizasYSeguros != null) {
      for (let i = 0; i < polizasList.length; i++) {
        switch (polizasList[i]) {
          case '1':
            garantiaArray = {
              'contratoPolizaId': this.idPoliza,
              'TipoGarantiaCodigo': '1',
              'EsIncluidaPoliza': this.addressForm.value.buenManejoCorrectaInversionAnticipo
            };
            this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r => {
            });
            break;
          case '2':
            garantiaArray = {
              'contratoPolizaId': this.idPoliza,
              'TipoGarantiaCodigo': '2',
              'EsIncluidaPoliza': this.addressForm.value.estabilidadYCalidad
            };
            this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r1 => {
            });
            break;
          case '3':
            garantiaArray = {
              'contratoPolizaId': this.idPoliza,
              'TipoGarantiaCodigo': '3',
              'EsIncluidaPoliza': this.addressForm.value.polizaYCoumplimiento
            };
            this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r2 => {
            });
            break;
          case '4':
            garantiaArray = {
              'contratoPolizaId': this.idPoliza,
              'TipoGarantiaCodigo': '4',
              'EsIncluidaPoliza': this.addressForm.value.polizasYSegurosCompleto
            };
            this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r3 => {
            });
            break;
        }
      }
    }
    this.polizaService.EditarContratoPoliza(contratoArray).subscribe(data => {
      if (data.isSuccessful == true) {
        /*this.polizaService.CreatePolizaGarantia(polizaGarantia).subscribe(data0=>{

        });*/
        this.polizaService.CreatePolizaObservacion(observacionArray).subscribe(resp => {

        });
        /*
        this.polizaService.CambiarEstadoPolizaByContratoId(statePoliza, this.idContrato).subscribe(resp1 => {

        });
        */
        this.realizoPeticion = true;
        this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
        this.router.navigate(['/generarPolizasYGarantias']);
      }
      else {
        this.openDialog('', `<b>${data.message}</b>`);
      }
    });
    console.log(this.addressForm.value);
  }

}
