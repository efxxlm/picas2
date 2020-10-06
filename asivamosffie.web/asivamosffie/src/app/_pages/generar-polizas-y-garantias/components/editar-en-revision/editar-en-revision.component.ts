import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CreatePolizaGarantia, CreatePolizaObservacion, EditPoliza, InsertPoliza, PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-editar-en-revision',
  templateUrl: './editar-en-revision.component.html',
  styleUrls: ['./editar-en-revision.component.scss']
})
export class EditarEnRevisionComponent implements OnInit {

  addressForm = this.fb.group({
    nombre: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(50)])
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
      Validators.required, Validators.minLength(5), Validators.maxLength(20)])
    ],
    polizasYSeguros: [null, Validators.required],
    buenManejoCorrectaInversionAnticipo: [null, Validators.required],
    estabilidadYCalidad: [null, Validators.required],
    cumpleAsegurado: [null, Validators.required],
    cumpleBeneficiario: [null, Validators.required],
    cumpleAfianzado: [null, Validators.required],
    reciboDePago: [null, Validators.required],
    condicionesGenerales: [null, Validators.required],
    fechaRevision: [null, Validators.required],
    estadoRevision: [null, Validators.required],
    observacionesGenerales: [null, Validators.required],
  });

  polizasYSegurosArray = [
    { name: 'Buen manejo y correcta inversión del anticipo', value: '1' },
    { name: 'Garantía de estabilidad y calidad de la obra', value: '2' },
    { name: 'Póliza de cumplimiento', value: '3' },
    { name: 'Garantía de estabilidad y calidad de la obra', value: '4' }
  ];
  estadoArray = [
    { name: 'Devuelta', value: '1' },
    { name: 'Aprobada', value: '2' }
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

  constructor(
    private router: Router,
    private polizaService: PolizaGarantiaService,
    private fb: FormBuilder,
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute
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
    this.polizaService.GetListVistaContratoGarantiaPoliza().subscribe(data=>{
      this.tipoContrato=data[id-1].tipoContrato;
      this.objeto=data[id-1].descripcionModificacion;
      this.nombreContratista = data[id-1].nombreContratista;
      this.tipoIdentificacion = "NIT"  // quemado 
      this.numeroIdentificacion = data[id-1].numeroIdentificacion;
      this.valorContrato = data[id-1].valorContrato;
      this.plazoContrato = data[id-1].plazoContrato;
      this.numContrato = data[id-1].numeroContrato;
    });
  }
  loadData(id){
    this.polizaService.GetContratoPolizaByIdContratoPolizaId(id).subscribe(data=>{
      this.addressForm.get('nombre').setValue(data.nombreAseguradora);
      this.addressForm.get('numeroPoliza').setValue(data.numeroPoliza);
      this.addressForm.get('numeroCertificado').setValue(data.numeroCertificado);
      this.addressForm.get('fecha').setValue(data.fechaExpedicion);
      this.addressForm.get('vigenciaPoliza').setValue(data.vigencia);
      this.addressForm.get('vigenciaAmparo').setValue(data.vigenciaAmparo);
      this.addressForm.get('valorAmparo').setValue(data.valorAmparo);
      this.addressForm.get('cumpleAsegurado').setValue(data.cumpleDatosAsegurado);
      this.addressForm.get('cumpleBeneficiario').setValue(data.cumpleDatosBeneficiario);
      this.addressForm.get('cumpleAfianzado').setValue(data.cumpleDatosTomador);
      this.addressForm.get('reciboDePago').setValue(data.incluyeReciboPago);
      this.addressForm.get('condicionesGenerales').setValue(data.incluyeCondicionesGenerales);
      this.dataLoad2(data);
    }); 
  }

  loadObservations(id){
    this.polizaService.GetListPolizaObservacionByContratoPolizaId(id).subscribe(data_A=>{
      const estadoRevisionCodigo = this.estadoArray.find(p => p.value === data_A[0].estadoRevisionCodigo);
      this.addressForm.get('fechaRevision').setValue(data_A[0].fechaRevision);
      this.addressForm.get('estadoRevision').setValue(estadoRevisionCodigo);
      this.addressForm.get('observacionesGenerales').setValue(data_A[0].observacion);
      this.loadGarantia(data_A[0].polizaObservacionId);
    });
  }

  loadGarantia(id){
    this.polizaService.GetListPolizaGarantiaByContratoPolizaId(id).subscribe(data_B=>{
      const tipoGarantiaCodigo = this.polizasYSegurosArray.find(t => t.value == data_B[0].tipoGarantiaCodigo);
      this.addressForm.get('polizasYSeguros').setValue(tipoGarantiaCodigo);
      this.addressForm.get('buenManejoCorrectaInversionAnticipo').setValue(data_B[0].esIncluidaPoliza);
    });
    this.idObservacion = id;
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
    let auxValue = this.addressForm.value.estadoRevision;
    console.log(auxValue.value);
    const contratoArray :EditPoliza ={
      contratoId: this.idContrato,
      nombreAseguradora: this.addressForm.value.nombre,
      numeroPoliza:this.addressForm.value.numeroPoliza,
      numeroCertificado: this.addressForm.value.numeroCertificado,
      fechaExpedicion:this.addressForm.value.fecha,
      vigencia: this.addressForm.value.vigenciaPoliza,
      vigenciaAmparo: this.addressForm.value.vigenciaAmparo,
      valorAmparo: this.addressForm.value.valorAmparo,
      estadoPolizaCodigo:"",
      usuarioCreacion:"",
      registroCompleto:false,
      fechaModificacion: this.addressForm.value.fecha,
      usuarioModificacion:"",
      contratoPolizaId:this.idPoliza,
      polizaGarantia:[],
      polizaObservacion:[],
      cumpleDatosAsegurado: this.addressForm.value.cumpleAsegurado,
      cumpleDatosBeneficiario: this.addressForm.value.cumpleBeneficiario,
      cumpleDatosTomador: this.addressForm.value.cumpleAfianzado,
      incluyeReciboPago: this.addressForm.value.reciboDePago,
      incluyeCondicionesGenerales: this.addressForm.value.condicionesGenerales
    };
    const polizaGarantia: CreatePolizaGarantia={
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
    }
    this.polizaService.EditarContratoPoliza(contratoArray).subscribe(data => {
      if(data.isSuccessful==true){
        this.openDialog('', data.message);
        this.router.navigate(['/generarPolizasYGarantias']);
      }
      else{
        this.openDialog('', data.message);
      }
    });
    this.polizaService.CreatePolizaGarantia(polizaGarantia).subscribe(data1=>{

    });
    this.polizaService.CreatePolizaObservacion(polizaObservacion).subscribe(data2=>{

    });
    console.log(this.addressForm.value);
  }

}
