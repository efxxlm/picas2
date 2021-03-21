import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-form-soporte-solicitud',
  templateUrl: './form-soporte-solicitud.component.html',
  styleUrls: ['./form-soporte-solicitud.component.scss']
})
export class FormSoporteSolicitudComponent implements OnChanges {

  @Output() guardar = new EventEmitter();
  @Input() novedad:NovedadContractual;

  estaEditando = false;

  addressForm = this.fb.group({
    urlSoporte: [null, Validators.required]
  });

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
  ) { }

  ngOnChanges(changes: SimpleChanges): void {
    if ( changes.novedad ){
      this.addressForm.get('urlSoporte').setValue(this.novedad.urlSoporte);
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    // console.log(this.addressForm.value);
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();

    this.novedad.urlSoporte = this.addressForm.get('urlSoporte').value;

    this.guardar.emit(true);    

    //this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }

}
