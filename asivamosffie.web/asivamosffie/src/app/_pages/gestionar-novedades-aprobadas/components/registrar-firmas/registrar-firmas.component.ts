import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-registrar-firmas',
  templateUrl: './registrar-firmas.component.html',
  styleUrls: ['./registrar-firmas.component.scss']
})
export class RegistrarFirmasComponent implements OnInit, OnChanges {

  @Input() novedad: NovedadContractual;
  @Output() guardar = new EventEmitter();

  addressForm = this.fb.group({
    continuarProceso: [null, Validators.required],
    fechaEnvioActaFirmaContratistaObra: [null, Validators.required],
    fechaFirmaContratistaObra: [null, Validators.required],
    fechaEnvioActaFirmaContratistaInterventoria: [null, Validators.required],
    fechaFirmaContratistaInterventoria: [null, Validators.required],
    fechaEnvioActaFirmaApoyoSupervision: [null, Validators.required],
    fechaFirmaApoyoSupervision: [null, Validators.required],
    fechaEnvioFirmaSupervisor: [null, Validators.required],
    fechaFirmaSupervisor: [null, Validators.required],
    razones: [null, Validators.required],
    urlFirmas: [null, Validators.required]
  });

  estaEditando = false;

  editorStyle = {
    height: '75px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  estadoDelProcesoArray = [
    { name: 'Aprobada', value: '1' },
    { name: 'En revisión de gestión contractual', value: '2' }
  ];
  nombreAbogadoArray = [
    { name: 'Laura Andrea Osorio Martínez', value: '1' },
    { name: 'Laura Andrea Osorio Martínez 2', value: '2' }
  ];



  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
  ) { }
  ngOnChanges(changes: SimpleChanges): void {

    if (changes.novedad) {
      this.addressForm.get('continuarProceso').setValue(this.novedad.deseaContinuar);
      this.addressForm.get('fechaEnvioActaFirmaContratistaObra').setValue(this.novedad.fechaEnvioActaContratistaObra);
      this.addressForm.get('fechaFirmaContratistaObra').setValue(this.novedad.fechaFirmaActaContratistaObra);
      this.addressForm.get('fechaEnvioActaFirmaContratistaInterventoria').setValue(this.novedad.fechaEnvioActaContratistaInterventoria);
      this.addressForm.get('fechaFirmaContratistaInterventoria').setValue(this.novedad.fechaFirmaContratistaInterventoria);
      this.addressForm.get('fechaEnvioActaFirmaApoyoSupervision').setValue(this.novedad.fechaEnvioActaApoyo);
      this.addressForm.get('fechaFirmaApoyoSupervision').setValue(this.novedad.fechaFirmaApoyo);
      this.addressForm.get('fechaEnvioFirmaSupervisor').setValue(this.novedad.fechaEnvioActaSupervisor);
      this.addressForm.get('fechaFirmaSupervisor').setValue(this.novedad.fechaFirmaSupervisor);
      this.addressForm.get('razones').setValue(this.novedad.razonesNoContinuaProceso);
      this.addressForm.get('urlFirmas').setValue(this.novedad.urlSoporteFirmas);

      this.estaEditando = true;
      this.addressForm.markAllAsTouched();
      this.addressForm.get('fechaFirmaContratistaInterventoria').updateValueAndValidity();
      this.addressForm.get('fechaFirmaApoyoSupervision').updateValueAndValidity();
    }


  }

  ngOnInit(): void {
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if (texto) {
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {

    let contadorConcurrencias = 0;
    let posicion = 0;
    if (cadena) {
      while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
        ++contadorConcurrencias;
        posicion += subcadena.length;
      }
    }
    return contadorConcurrencias;
  }

  onSubmit() {

    this.estaEditando = true;
    this.addressForm.markAllAsTouched();

    this.novedad.deseaContinuar = this.addressForm.get('continuarProceso').value;
    this.novedad.fechaEnvioActaContratistaObra = this.addressForm.get('fechaEnvioActaFirmaContratistaObra').value;
    this.novedad.fechaFirmaActaContratistaObra = this.addressForm.get('fechaFirmaContratistaObra').value;
    this.novedad.fechaEnvioActaContratistaInterventoria = this.addressForm.get('fechaEnvioActaFirmaContratistaInterventoria').value;
    this.novedad.fechaFirmaContratistaInterventoria = this.addressForm.get('fechaFirmaContratistaInterventoria').value;
    this.novedad.fechaEnvioActaApoyo = this.addressForm.get('fechaEnvioActaFirmaApoyoSupervision').value;
    this.novedad.fechaFirmaApoyo = this.addressForm.get('fechaFirmaApoyoSupervision').value;
    this.novedad.fechaEnvioActaSupervisor = this.addressForm.get('fechaEnvioFirmaSupervisor').value;
    this.novedad.fechaFirmaSupervisor = this.addressForm.get('fechaFirmaSupervisor').value;
    this.novedad.razonesNoContinuaProceso = this.addressForm.get('razones').value;
    this.novedad.urlSoporteFirmas = this.addressForm.get('urlFirmas').value;

    this.guardar.emit(true);

  }
}
