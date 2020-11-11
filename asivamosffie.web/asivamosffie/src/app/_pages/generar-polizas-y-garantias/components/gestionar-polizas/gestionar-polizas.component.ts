import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { PolizaGarantiaService, ContratoPoliza, InsertPoliza } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
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
    responsableAprob: ['', Validators.required],
    observacionesGenerales: ['', Validators.required]
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
  public idContrato;
  public selectedArray;
  obj1: boolean;
  obj2: boolean;
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
    this.activatedRoute.params.subscribe(param => {
      this.cargarDatos(param.id);
    });
  }
  cargarDatos(id) {
    this.polizaService.GetListVistaContratoGarantiaPoliza(id).subscribe(data => {
      this.tipoContrato = data[0].tipoContrato;
      this.objeto = data[0].descripcionModificacion;
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
    this.idContrato = id;
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
    const polizasList = [this.addressForm.value.polizasYSeguros[0].codigo];
    for (let i = 1; i < this.addressForm.value.polizasYSeguros.length; i++) {
        const membAux = polizasList.push(this.addressForm.value.polizasYSeguros[i].codigo);
    }
    console.log(polizasList);
    let nombreAprobado;
    if(!this.addressForm.value.responsableAprob.name){
      nombreAprobado = "null";
    }
    else{
      nombreAprobado = this.addressForm.value.responsableAprob.name;
    }
    var completo:boolean;
    if(this.addressForm.valid){
      completo = true;
    }
    else{
      completo = false;
    }
    const contratoArray ={
      'contratoId':this.idContrato,  
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
      "Observacion":this.addressForm.value.observacionesGenerales,
      "FechaRevision":this.addressForm.value.fechaRevision,
      "EstadoRevisionCodigo":this.addressForm.value.estadoRevision.value
    }
    this.polizaService.CreateContratoPoliza(contratoArray).subscribe(data => {
      if (data.isSuccessful == true) {
        this.polizaService.CreatePolizaObservacion(observacionArray).subscribe(resp=>{

        });
        this.polizaService.CambiarEstadoPolizaByContratoId("2",this.idContrato).subscribe(resp0=>{

        });
        this.openDialog('', 'La información ha sido guardada exitosamente.');
        this.router.navigate(['/generarPolizasYGarantias']);
      }
      else {
        this.openDialog('', `<b>${data.message}</b>`);
      }
    });
    console.log(this.addressForm.value);
  }
}
