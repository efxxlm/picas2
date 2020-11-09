import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CreatePolizaGarantia, CreatePolizaObservacion, EditPoliza, InsertPoliza, PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
@Component({
  selector: 'app-editar-en-revision',
  templateUrl: './editar-en-revision.component.html',
  styleUrls: ['./editar-en-revision.component.scss']
})
export class EditarEnRevisionComponent implements OnInit {

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
  public idPoliza2;
  public idObservacion;
  public selected = [];
  public obj1;
  public obj2;
  public arrayprueba = ["1","2"];
  obj3: boolean;
  obj4: boolean;

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

  loadContrato(id){
    this.polizaService.GetListVistaContratoGarantiaPoliza(id).subscribe(data=>{
      this.tipoContrato=data[0].tipoContrato;
      this.objeto=data[0].descripcionModificacion;
      this.nombreContratista = data[0].nombreContratista;
      this.tipoIdentificacion = "NIT"  // quemado 
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
      this.loadGarantia(35);
      this.dataLoad2(data);
    }); 
    this.polizaService.GetNotificacionContratoPolizaByIdContratoId(id).subscribe(data_1=>{
      const estadoRevisionCodigo = this.estadoArray.find(p => p.value === data_1.estadoRevision);
      this.addressForm.get('fechaRevision').setValue(data_1.fechaRevisionDateTime);
      this.addressForm.get('estadoRevision').setValue(estadoRevisionCodigo);
    });
  }

  loadObservations(id){

  }

  loadGarantia(id){
    this.polizaService.GetListPolizaGarantiaByContratoPolizaId(id).subscribe(data_B=>{
      this.addressForm.get('buenManejoCorrectaInversionAnticipo').setValue(data_B[0].esIncluidaPoliza);
      const tipoGarantiaCodigo = this.polizasYSegurosArray.find(t => t.codigo == data_B[0].tipoGarantiaCodigo);
      this.addressForm.get('polizasYSeguros').setValue([tipoGarantiaCodigo]);
      this.loadGrantiaID(data_B[0].polizaGarantiaId);
      this.getvalues([tipoGarantiaCodigo]);
    });
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
  dataLoad2(data){
    this.idContrato = data.contratoId;
    this.idPoliza = data.contratoPolizaId;
  }
  loadGrantiaID(id){
    if(id!=undefined){
      this.idPoliza2 = id;
    }
    else{
      this.idPoliza2 = undefined;
    }
  }
  loadObservacionId2(id){
    if(id!=undefined){
      this.idObservacion = id;
    }
    else{
      this.idObservacion = undefined;
    }
  }
  get segurosReq() {
    return this.addressForm.get('polizasYSeguros') as FormArray;
  }
  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }
  clickedOption(){
    console.log(this.selected)
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
    console.log(this.addressForm.value);
    let auxValue = this.addressForm.value.estadoRevision;
    let auxValue2 = this.addressForm.value.polizasYSeguros;
    const contratoArray :InsertPoliza ={
      contratoId:this.idContrato,  
      TipoSolicitudCodigo: "",
      TipoModificacionCodigo:"",
      DescripcionModificacion:"",
      NombreAseguradora:this.addressForm.value.nombre,
      NumeroPoliza:this.addressForm.value.numeroPoliza,
      NumeroCertificado:this.addressForm.value.numeroCertificado,
      Observaciones:"",
      ObservacionesRevisionGeneral:this.addressForm.value.observacionesGenerales,
      ResponsableAprobacion:this.addressForm.value.responsableAprob.name,
      EstadoPolizaCodigo:"2",
      UsuarioCreacion:"usr1",
      UsuarioModificacion:"usr1",
      FechaExpedicion: this.addressForm.value.fecha,
      Vigencia: this.addressForm.value.vigenciaPoliza,
      VigenciaAmparo: this.addressForm.value.vigenciaAmparo,
      ValorAmparo: this.addressForm.value.valorAmparo,
      CumpleDatosAsegurado: this.addressForm.value.cumpleAsegurado,
      CumpleDatosBeneficiario: this.addressForm.value.cumpleBeneficiario,
      CumpleDatosTomador: this.addressForm.value.cumpleAfianzado,
      IncluyeReciboPago:this.addressForm.value.reciboDePago,
      IncluyeCondicionesGenerales: this.addressForm.value.condicionesGenerales,
      FechaAprobacion: this.addressForm.value.fechaAprob,
      Estado: false,
      FechaCreacion: "",
      RegistroCompleto: false,
      FechaModificacion: "",
      Eliminado: false
    };
    
    const polizaGarantia: CreatePolizaGarantia={
      polizaGarantiaId : this.idPoliza2,
      contratoPolizaId: this.idPoliza,
      esIncluidaPoliza: this.addressForm.value.buenManejoCorrectaInversionAnticipo,
      tipoGarantiaCodigo: members,
    };
    /*
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
        this.polizaService.CreatePolizaGarantia(polizaGarantia).subscribe(data0=>{

        });
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
