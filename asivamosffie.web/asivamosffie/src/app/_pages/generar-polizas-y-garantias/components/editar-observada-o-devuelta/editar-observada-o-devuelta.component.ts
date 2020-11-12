import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CreatePolizaGarantia, CreatePolizaObservacion, EditPoliza, InsertPoliza, PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
@Component({
  selector: 'app-editar-observada-o-devuelta',
  templateUrl: './editar-observada-o-devuelta.component.html',
  styleUrls: ['./editar-observada-o-devuelta.component.scss']
})
export class EditarObservadaODevueltaComponent implements OnInit {

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
    buenManejoCorrectaInversionAnticipo: [null, Validators.required],
    estabilidadYCalidad: [null, Validators.required],
    polizaYCoumplimiento: [null, Validators.required],
    polizasYSegurosCompleto: [null, Validators.required],
    cumpleAsegurado: [null, Validators.required],
    cumpleBeneficiario: [null, Validators.required],
    cumpleAfianzado: [null, Validators.required],
    reciboDePago: [null, Validators.required],
    condicionesGenerales: [null, Validators.required],
    fechaRevision: [null, Validators.required],
    estadoRevision: [null, Validators.required],
    fechaAprob: [null, Validators.required],
    responsableAprob: [null, Validators.required],
    observacionesGenerales: [null, Validators.required]
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
  public idObservacion;
  selected: any;
  obj1: boolean;
  obj2: boolean;
  obj3: boolean;
  obj4: boolean;
  fechaFirmaContrato: any;
  tipoSolicitud: any;

  constructor(
    private router: Router,
    private polizaService: PolizaGarantiaService,
    private fb: FormBuilder,
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
    private common: CommonService
  ) {
    this.minDate = new Date();
  }
  ngOnInit(): void { 
    this.activatedRoute.params.subscribe( param => {
      this.loadContrato(param.id);
      this.loadData(param.id);
      this.loadObservations(param.id);
    });
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
  loadContrato(id){
    this.polizaService.GetListVistaContratoGarantiaPoliza(id).subscribe(data=>{
      this.fechaFirmaContrato = data[0].fechaFirmaContrato;
      this.tipoSolicitud = data[0].tipoSolicitud;
      this.tipoContrato=data[0].tipoContrato;
      this.objeto=data[0].descripcionModificacion;
      this.nombreContratista = data[0].nombreContratista;
      this.tipoIdentificacion = data[0].tipoDocumento;
      this.numeroIdentificacion = data[0].numeroIdentificacion;
      this.valorContrato = data[0].valorContrato;
      this.plazoContrato = data[0].plazoContrato;
      this.numContrato = data[0].numeroContrato;
    });
    this.common.listaGarantiasPolizas().subscribe(data0 => {
      this.polizasYSegurosArray = data0;
    });
  }
  loadData(id){
    this.polizaService.GetContratoPolizaByIdContratoId(id).subscribe(data=>{
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
      this.dataLoad2(data);
    }); 
  }

  loadObservations(id){

  }

  loadGarantia(id){

  }
  dataLoad2(data){
    this.idContrato = data.contratoId;
    this.idPoliza = data.contratoPolizaId;
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

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    const members = [this.addressForm.value.polizasYSeguros[0].codigo];
    for (let i = 1; i < this.addressForm.value.polizasYSeguros.length; i++) {
        const membAux = members.push(this.addressForm.value.polizasYSeguros[i].codigo);
    }
    console.log(members);
    let nombreAprobado;
    if(!this.addressForm.value.responsableAprob.name){
      nombreAprobado = "null";
    }
    else{
      nombreAprobado = this.addressForm.value.responsableAprob.name;
    }
    console.log(this.addressForm.value);
    let auxValue = this.addressForm.value.estadoRevision;
    let auxValue2 = this.addressForm.value.polizasYSeguros;
    const contratoArray ={
      'contratoId':this.idContrato,  
      "contratoPolizaId":this.idPoliza, 
      'TipoSolicitudCodigo': "",
      'TipoModificacionCodigo':"",
      'DescripcionModificacion':"",
      'NombreAseguradora':this.addressForm.value.nombre,
      'NumeroPoliza':this.addressForm.value.numeroPoliza,
      'NumeroCertificado':this.addressForm.value.numeroCertificado,
      'Observaciones':"",
      'ObservacionesRevisionGeneral':this.addressForm.value.observacionesGenerales,
      'ResponsableAprobacion':nombreAprobado,
      'EstadoPolizaCodigo':this.addressForm.value.polizasYSeguros[0].codigo,
      'UsuarioCreacion':"usr1",
      'UsuarioModificacion':"usr1",
      'FechaExpedicion': this.addressForm.value.fecha,
      'Vigencia': this.addressForm.value.vigenciaPoliza,
      'VigenciaAmparo': this.addressForm.value.vigenciaAmparo,
      'ValorAmparo': this.addressForm.value.valorAmparo,
      'CumpleDatosAsegurado': this.addressForm.value.cumpleAsegurado,
      'CumpleDatosBeneficiario': this.addressForm.value.cumpleBeneficiario,
      'CumpleDatosTomador': this.addressForm.value.cumpleAfianzado,
      'IncluyeReciboPago':this.addressForm.value.reciboDePago,
      'IncluyeCondicionesGenerales': this.addressForm.value.condicionesGenerales,
      'FechaAprobacion': this.addressForm.value.fechaAprob,
      'Estado': false,
      'FechaCreacion': "",
      'RegistroCompleto': false,
      'FechaModificacion': "",
      'Eliminado': false
    };
    const observacionArray={
      'contratoId':this.idContrato, 
      "contratoPolizaId":this.idPoliza, 
      "Observacion":this.addressForm.value.observacionesGenerales,
      "FechaRevision":this.addressForm.value.fechaRevision,
      "EstadoRevisionCodigo":this.addressForm.value.estadoRevision.value
    }
    /*
    const polizaGarantia: CreatePolizaGarantia={
      polizaGarantiaId : this.idPoliza2,
      contratoPolizaId: this.idPoliza,
      tipoGarantiaCodigo: this.addressForm.value.polizasYSeguros,
      esIncluidaPoliza: this.addressForm.value.buenManejoCorrectaInversionAnticipo
    };
    const polizaObservacion: CreatePolizaObservacion={
      polizaObservacionId: this.idObservacion,
      contratoPolizaId: this.idPoliza,
      observacion: this.addressForm.value.observacionesGenerales,
      fechaRevision: this.addressForm.value.fechaRevision,
      estadoRevisionCodigo: auxValue.value
    }*/
    var statePoliza;
    if(this.addressForm.value.estadoRevision=="1"){
      statePoliza = "3";
    }
    else{
      statePoliza = "2";
    }
    this.polizaService.EditarContratoPoliza(contratoArray).subscribe(data => {
      if(data.isSuccessful==true){
        this.polizaService.CambiarEstadoPolizaByContratoId(statePoliza,this.idContrato).subscribe(resp1=>{

        });
        this.openDialog('', 'La información ha sido guardada exitosamente.');
        this.router.navigate(['/generarPolizasYGarantias']);
      }
      else{
        this.openDialog('', `<b>${data.message}</b>`);
      }
    });
    console.log(this.addressForm.value);
  }
}
