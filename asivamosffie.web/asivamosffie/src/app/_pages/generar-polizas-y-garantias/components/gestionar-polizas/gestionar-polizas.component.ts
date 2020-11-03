import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { PolizaGarantiaService, ContratoPoliza, InsertPoliza } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

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

  polizasYSegurosArray = ['Pólizas y seguros interventoría','Pólizas y seguros de póliza obra'];
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
  obj1: boolean;
  obj2: boolean;

  constructor(
    private router: Router,
    private polizaService: PolizaGarantiaService,
    private fb: FormBuilder,
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
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
  getvalues(values: any[]) {
    this.obj1 = values.includes('Pólizas y seguros interventoría');
    this.obj2 = values.includes('Pólizas y seguros de póliza obra');
  }
  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    const contratoArray: InsertPoliza = {
      contratoId: this.idContrato.toString(),
      nombreAseguradora: this.addressForm.value.nombre,
      numeroPoliza: this.addressForm.value.numeroPoliza,
      numeroCertificado: this.addressForm.value.numeroCertificado,
      fechaExpedicion: this.addressForm.value.fecha,
      vigencia: this.addressForm.value.vigenciaPoliza,
      vigenciaAmparo: this.addressForm.value.vigenciaAmparo,
      valorAmparo: this.addressForm.value.valorAmparo
    };
    this.polizaService.CreateContratoPoliza(contratoArray).subscribe(data => {
      if (data.isSuccessful == true) {
        this.openDialog('', `<b>${data.message}</b>`);
        this.router.navigate(['/generarPolizasYGarantias']);
      }
      else {
        this.openDialog('', `<b>${data.message}</b>`);
      }
    });
    console.log(this.addressForm.value);
  }
}
