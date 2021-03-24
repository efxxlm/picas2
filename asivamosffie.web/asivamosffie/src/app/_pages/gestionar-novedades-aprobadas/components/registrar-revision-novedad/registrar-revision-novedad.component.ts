import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { NovedadContractual, NovedadContractualObservaciones } from 'src/app/_interfaces/novedadContractual';
@Component({
  selector: 'app-registrar-revision-novedad',
  templateUrl: './registrar-revision-novedad.component.html',
  styleUrls: ['./registrar-revision-novedad.component.scss']
})
export class RegistrarRevisionNovedadComponent implements OnInit, OnChanges {

  addressForm = this.fb.group({
    fechaEnvioGestionContractual: [null, Validators.required],
    estadoProcesoCodigo: [null, Validators.required],
    fechaAprobacionGestionContractual: [null, Validators.required],
    abogadoRevisionId: [null, Validators.required],
  });

  estaEditando = false;

  estadoDelProcesoArray = [
    { name: 'Aprobada', value: '1' },
    { name: 'En revisión de gestión contractual', value: '2' }
  ];
  nombreAbogadoPresentoSolicitudArray = [
    { name: 'Laura Andrea Osorio Martínez', value: '1' },
    { name: 'Laura Andrea Osorio Martínez 2', value: '2' }
  ];
  nombreAbogadorealizoRevisionArray = [
    { name: 'Laura Andrea Osorio Martínez', value: '1' },
    { name: 'Laura Andrea Osorio Martínez 2', value: '2' }
  ];



  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  @Input() novedad: NovedadContractual;
  @Output() guardar = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
  ) { }
  ngOnChanges(changes: SimpleChanges): void {
    if ( changes.novedad ){
      this.addressForm.get('fechaEnvioGestionContractual').setValue( this.novedad.fechaEnvioGestionContractual )
      this.addressForm.get('estadoProcesoCodigo').setValue( this.novedad.estadoProcesoCodigo )
      this.addressForm.get('fechaAprobacionGestionContractual').setValue( this.novedad.fechaAprobacionGestionContractual )
      this.addressForm.get('abogadoRevisionId').setValue( this.novedad.abogadoRevisionId )
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

  onSubmit() {

    this.estaEditando = true;
    this.addressForm.markAllAsTouched();

    this.novedad.fechaEnvioGestionContractual = this.addressForm.get('fechaEnvioGestionContractual').value;
    this.novedad.estadoProcesoCodigo = this.addressForm.get('estadoProcesoCodigo').value;
    this.novedad.fechaAprobacionGestionContractual = this.addressForm.get('fechaAprobacionGestionContractual').value;
    this.novedad.abogadoRevisionId = this.addressForm.get('abogadoRevisionId').value;

    this.guardar.emit(true);

  }
}
