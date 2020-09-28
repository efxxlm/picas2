import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { PolizaGarantiaService, ContratoPoliza } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-gestionar-polizas',
  templateUrl: './gestionar-polizas.component.html',
  styleUrls: ['./gestionar-polizas.component.scss']
})
export class GestionarPolizasComponent implements OnInit {
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
    { name: 'Devuelta', value: '1' }
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

  constructor(
    private polizaService: PolizaGarantiaService,
    private fb: FormBuilder,
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
  ) {
    this.minDate = new Date();
  }
  ngOnInit(): void {
    this.activatedRoute.params.subscribe( param => {
      this.cargarDatos(param.id);
    });
    
    
  }
  cargarDatos(id) {
    this.polizaService.GetListVistaContratoGarantiaPoliza().subscribe(data => {
      //la posicion 0 es una posicion quemada 
      this.tipoContrato = data[id-1].tipoContrato;
      this.objeto = data[id-1].descripcionModificacion;
      this.nombreContratista = data[id-1].nombreContratista;
      this.tipoIdentificacion = "NIT"  // quemado 
      this.numeroIdentificacion = data[id-1].numeroIdentificacion;
      this.valorContrato = data[id-1].valorContrato;
      this.plazoContrato = data[id-1].plazoContrato;
      this.numContrato = data[id-1].numeroContrato;
    });
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
   /* this.polizaService.CreateContratoPoliza("").subscribe(data => {
      if(data.isSuccessful==true){
        this.openDialog('', 'La información ha sido guardada exitosamente.');
      }
      else{
        this.openDialog('', 'Error en el servicio.');
      }
    });*/
    this.openDialog('', 'La información ha sido guardada exitosamente.');
    console.log(this.addressForm.value);
  }
}
