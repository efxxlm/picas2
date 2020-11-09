import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-registrar-controvrs-sop-sol',
  templateUrl: './form-registrar-controvrs-sop-sol.component.html',
  styleUrls: ['./form-registrar-controvrs-sop-sol.component.scss']
})
export class FormRegistrarControvrsSopSolComponent implements OnInit {

  @Input() isEditable;

  addressForm = this.fb.group({
    urlSoporte: [null, Validators.required]
  });
  constructor(  private fb: FormBuilder, public dialog: MatDialog) { }

  ngOnInit(): void {
    if(this.isEditable==true){
      this.addressForm.get('urlSoporte').setValue('http://www.prueba1444.com');
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.openDialog('', 'La información ha sido guardada exitosamente.');
  }
}
