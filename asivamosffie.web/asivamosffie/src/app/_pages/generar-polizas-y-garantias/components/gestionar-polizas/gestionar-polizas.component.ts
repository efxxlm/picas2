import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { PolizaGarantiaService, ContratoPoliza, InsertPoliza } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
@Component({
  selector: 'app-gestionar-polizas',
  templateUrl: './gestionar-polizas.component.html',
  styleUrls: ['./gestionar-polizas.component.scss']
})
export class GestionarPolizasComponent implements OnInit {
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
    fechaAprob: [null, Validators.required],
    responsableAprob: ['', Validators.required],
    observacionesGenerales: ['']
  });

  polizasYSegurosArray: Dominio[] = [];
  estadoArray = [
    { name: 'Devuelta', value: '1' },
    { name: 'Aprobada', value: '2' }
  ];
  aprobadosArray = [
    { name: 'Andres Montealegre', value: '1' },
    { name: 'David Benitez', value: '2' }
  ];
  listaUsuarios: any[]=[];
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
  public idContrato;
  public selectedArray;
  obj1: boolean;
  obj2: boolean;
  obj3: boolean;
  obj4: boolean;
  fechaFirmaContrato: any;
  tipoSolicitud: any;
  contratoPolizaId: any;
  constructor(
    private router: Router,
    private polizaService: PolizaGarantiaService,
    private fb: FormBuilder,
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
    private common: CommonService,
    private contratacion:ProjectContractingService 
  ) {
    this.minDate = new Date();
  }
  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.cargarDatos(param.id);
    });
  }
  cargarDatos(id) {
    this.polizaService.GetListVistaContratoGarantiaPoliza(id).subscribe(data => {
      this.fechaFirmaContrato = data[0].fechaFirmaContrato;
      this.tipoSolicitud = data[0].tipoSolicitud;
      this.numContrato = data[0].numeroContrato;
    });
    this.polizaService.GetContratoPolizaByIdContratoId(id).subscribe(a=>{
      this.loadPolizaId(a);
      this.loadContratacionId(a);
    });
    this.common.listaGarantiasPolizas().subscribe(data0 => {
      this.polizasYSegurosArray = data0;
    });
    this.common.getUsuariosByPerfil(10).subscribe(resp=>{
      this.listaUsuarios = resp;
    });
    this.idContrato = id;
  }
  loadPolizaId(a){
    if(this.contratoPolizaId!=null){
      this.contratoPolizaId = a.contratoPolizaId;
    }
    else{
      this.contratoPolizaId = 0;
    }
    
  }

  loadContratacionId(a){
    if(a.contratacionId!=null || a.contratacionId!=undefined){
      this.contratacion.getContratacionByContratacionId(a.contratacionId).subscribe(data=>{
        this.loadInfoContratacion(data);
      });
    }
    else{
      this.loadNoContratacionID();
    }
  }
  loadInfoContratacion(data){
    if(data.disponibilidadPresupuestal.length>0){
      this.tipoContrato = data.disponibilidadPresupuestal[0].opcionContratarCodigo;
      this.objeto = data.disponibilidadPresupuestal[0].objeto;
      this.valorContrato = data.disponibilidadPresupuestal[0].valorSolicitud;
      this.plazoContrato = data.disponibilidadPresupuestal[0].plazoMeses + 'meses / ' + data.disponibilidadPresupuestal[0].plazoDias + 'días';
    }
    else{
      this.tipoContrato = 'Pendiente';
      this.objeto = 'Pendiente';
      this.valorContrato = 0;
      this.plazoContrato =' 0 meses / 0 días';
    }
    this.nombreContratista = data.contratista.nombre;
    if(data.contratista.tipoIdentificacionCodigo != undefined || data.contratista.tipoIdentificacionCodigo != undefined){
      this.tipoIdentificacion = data.contratista.tipoIdentificacionCodigo;
    }
    else{
      this.tipoIdentificacion = 'Pendiente';
    }
    this.numeroIdentificacion = data.contratista.numeroIdentificacion;
    
  }

  loadNoContratacionID(){
    this.tipoContrato = 'Pendiente';
    this.objeto = 'Pendiente';
    this.valorContrato = 0;
    this.tipoIdentificacion = 'Pendiente';
    this.plazoContrato =' 0 meses / 0 días';
  }
  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }
  getvalues(values: Dominio[]) {
    console.log(values);
    const buenManejo = values.find(value => value.codigo == "1");
    const garantiaObra = values.find(value => value.codigo == "2");
    const pCumplimiento = values.find(value => value.codigo == "3");
    const polizasYSeguros = values.find(value => value.codigo == "4");

    buenManejo ? this.obj1 = true : this.obj1 = false;
    garantiaObra ? this.obj2 = true : this.obj2 = false;
    pCumplimiento ? this.obj3 = true : this.obj3 = false;
    polizasYSeguros ? this.obj4 = true : this.obj4 = false;

  }
  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    let polizasList;
    if(this.addressForm.value.polizasYSeguros!=undefined || this.addressForm.value.polizasYSeguros!=null ){
        polizasList = [this.addressForm.value.polizasYSeguros[0].codigo];
      for (let i = 1; i < this.addressForm.value.polizasYSeguros.length; i++) {
        const membAux = polizasList.push(this.addressForm.value.polizasYSeguros[i].codigo);
      }
      console.log(polizasList);
    }
    let nombreAprobado;
    if(this.addressForm.value.responsableAprob!=undefined || this.addressForm.value.responsableAprob!=null ){
      if (!this.addressForm.value.responsableAprob.name) {
        nombreAprobado = null;
      }
      else {
        nombreAprobado = this.addressForm.value.responsableAprob.name;
      }
    }
    var completo: boolean;
    if (this.addressForm.valid) {
      completo = true;
    }
    else {
      completo = false;
    }
    const contratoArray = {
      'contratoId': this.idContrato,
      'TipoSolicitudCodigo': "",
      'TipoModificacionCodigo': "",
      'DescripcionModificacion': "",
      'NombreAseguradora': this.addressForm.value.nombre,
      'NumeroPoliza': this.addressForm.value.numeroPoliza,
      'NumeroCertificado': this.addressForm.value.numeroCertificado,
      'Observaciones': "",
      'ObservacionesRevisionGeneral': this.addressForm.value.observacionesGenerales,
      'ResponsableAprobacion': nombreAprobado,
      'EstadoPolizaCodigo': "2",
      'UsuarioCreacion': "usr1",
      'UsuarioModificacion': "usr1",
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
      'FechaCreacion': null,
      'RegistroCompleto': false,
      'FechaModificacion': null,
      'Eliminado': false
    };
    let garantiaArray;
    this.polizaService.CreateContratoPoliza(contratoArray).subscribe(data => {
      if (data.isSuccessful == true) {
        this.polizaService.CambiarEstadoPolizaByContratoId("2", this.idContrato).subscribe(resp0 => {

        });
        this.polizaService.GetContratoPolizaByIdContratoId(this.idContrato).subscribe(rep1=>{
          if(this.addressForm.value.polizasYSeguros!=undefined || this.addressForm.value.polizasYSeguros!=null){
            for (let i = 0; i < polizasList.length; i++) {
              switch (polizasList[i]) {
                case '1':
                  garantiaArray = {
                    'contratoPolizaId': rep1.contratoPolizaId,
                    'TipoGarantiaCodigo': '1',
                    'EsIncluidaPoliza': this.addressForm.value.buenManejoCorrectaInversionAnticipo
                  };
                  this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r => {
                  });
                  break;
                case '2':
                  garantiaArray = {
                    'contratoPolizaId': rep1.contratoPolizaId,
                    'TipoGarantiaCodigo': '2',
                    'EsIncluidaPoliza': this.addressForm.value.estabilidadYCalidad
                  };
                  this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r1 => {
                  });
                  break;
                case '3':
                  garantiaArray = {
                    'contratoPolizaId': rep1.contratoPolizaId,
                    'TipoGarantiaCodigo': '3',
                    'EsIncluidaPoliza': this.addressForm.value.polizaYCoumplimiento
                  };
                  this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r2 => {
                  });
                  break;
                case '4':
                  garantiaArray = {
                    'contratoPolizaId': rep1.contratoPolizaId,
                    'TipoGarantiaCodigo': '4',
                    'EsIncluidaPoliza': this.addressForm.value.polizasYSegurosCompleto
                  };
                  this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r3 => {
                  });
                  break;
              }
            }
          }
          const observacionArray = {
            'contratoId': this.idContrato,
            "contratoPolizaId":rep1.contratoPolizaId, 
            "Observacion": this.addressForm.value.observacionesGenerales,
            "FechaRevision": this.addressForm.value.fechaRevision,
            "EstadoRevisionCodigo": this.addressForm.value.estadoRevision.value
          }
          this.polizaService.CreatePolizaObservacion(observacionArray).subscribe(resp => {

          });
        });
        this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
        this.router.navigate(['/generarPolizasYGarantias']);
      }
      else {
        this.openDialog('', `<b>${data.message}</b>`);
      }
    });
  }
}
