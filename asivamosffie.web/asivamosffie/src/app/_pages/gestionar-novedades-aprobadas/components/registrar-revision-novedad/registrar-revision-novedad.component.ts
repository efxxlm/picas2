import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
@Component({
  selector: 'app-registrar-revision-novedad',
  templateUrl: './registrar-revision-novedad.component.html',
  styleUrls: ['./registrar-revision-novedad.component.scss']
})
export class RegistrarRevisionNovedadComponent implements OnInit {

  addressForm = this.fb.group({
    fechaEnvioSolicitud: [null, Validators.required],
    estadoDelProceso: [null, Validators.required],
    fechaSolicitudNovedad: [null, Validators.required],
    nombreAbogadoPresentoSolicitud: [null, Validators.required],
    fechaAprobacionGrupoGestionContractual: [null, Validators.required],
    nombreAbogadorealizoRevision: [null, Validators.required]
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

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
  ) { }

  ngOnInit(): void {
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.estaEditando = true;
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }
}
