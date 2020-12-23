import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-descripcion-actuacion',
  templateUrl: './form-descripcion-actuacion.component.html',
  styleUrls: ['./form-descripcion-actuacion.component.scss']
})
export class FormDescripcionActuacionComponent implements OnInit {
  @Input() isEditable;
 
  addressForm = this.fb.group({
    estadoAvanceTramite: [null, Validators.required],
    fechaActuacionAdelantada: [null, Validators.required],
    actuacionAdelantada: [null, Validators.required],
    proximaActuacionRequerida: [null, Validators.required],
    cualOtro: [null, Validators.required],
    diasVencimientoTerminos: [null, Validators.required],
    fechaVencimientoTerminos: [null, Validators.required],
    participacionContratista: [null, Validators.required],
    participacionInterventorContrato: [null, Validators.required],
    participacionSupervisorContrato: [null, Validators.required],
    participacionFiduciaria: [null, Validators.required],
    requiereComiteTecnico: [null, Validators.required],
    observaciones: [null, Validators.required],
  });
  estadoAvanceTramiteArray = [
    { name: 'Aprobación de Comunicación de Inicio de TAI', value: '1' },
  ];
  actuacionAdelantadaArray = [
    { name: 'Actuación 1', value: '1' },
  ];
  proximaActuacionRequeridaArray = [
    { name: 'Otro', value: '1' },
  ];
  editorStyle = {
    height: '50px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  numReclamacion: any;

  constructor(private fb: FormBuilder, public dialog: MatDialog, private services: ContractualControversyService, private common: CommonService,private router: Router) { }

  ngOnInit(): void {
    this.common.listaEstadosAvanceTramite().subscribe(rep => {
      this.estadoAvanceTramiteArrayDom = rep;
    });
    this.common.listaActuacionAdelantada().subscribe(rep1=>{
      this.actuacionAdelantadaArrayDom = rep1;
    });
    this.common.listaProximaActuacionRequerida().subscribe(rep2 => {
      this.proximaActuacionRequeridaArrayDom = rep2;
    });
    if (this.isEditable == true) {
      this.services.GetControversiaActuacionById(this.idActuacionFromEdit).subscribe((data:any)=>{
        const avanceTramSelected = this.estadoAvanceTramiteArrayDom.find(t => t.codigo === data.estadoAvanceTramiteCodigo);
        this.addressForm.get('estadoAvanceTramite').setValue(avanceTramSelected);
        this.addressForm.get('fechaActuacionAdelantada').setValue(data.fechaActuacion);
        const actuacionAdelantadaSelected = this.actuacionAdelantadaArrayDom.find(t => t.codigo === data.actuacionAdelantadaCodigo);
        this.addressForm.get('actuacionAdelantada').setValue(actuacionAdelantadaSelected);
        const actuacionRequeridaSelected = this.proximaActuacionRequeridaArrayDom.find(t => t.codigo === data.proximaActuacionCodigo);
        this.addressForm.get('proximaActuacionRequerida').setValue(actuacionRequeridaSelected);
        this.addressForm.get('cualOtro').setValue(data.actuacionAdelantadaOtro);
        this.addressForm.get('diasVencimientoTerminos').setValue(data.cantDiasVencimiento.toString());
        this.addressForm.get('fechaVencimientoTerminos').setValue(data.fechaVencimiento);
        this.addressForm.get('participacionContratista').setValue(data.esRequiereContratista);
        this.addressForm.get('participacionInterventorContrato').setValue(data.esRequiereInterventor);
        this.addressForm.get('participacionSupervisorContrato').setValue(data.esRequiereSupervisor);
        this.addressForm.get('participacionFiduciaria').setValue(data.esRequiereFiduciaria);
        this.addressForm.get('requiereComiteTecnico').setValue(data.esRequiereComite);
        this.addressForm.get('observaciones').setValue(data.observaciones);
        this.addressForm.get('urlSoporte').setValue(data.rutaSoporte);
        this.numReclamacion = data.numeroReclamacion;
      });
    }
  }
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {
    
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n-1, e.editor.getLength());
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
    console.log(this.addressForm.value);
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }
}
