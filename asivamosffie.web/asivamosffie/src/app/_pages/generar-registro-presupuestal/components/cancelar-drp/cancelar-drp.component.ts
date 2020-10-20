import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-cancelar-drp',
  templateUrl: './cancelar-drp.component.html',
  styleUrls: ['./cancelar-drp.component.scss']
})
export class CancelarDrpComponent implements OnInit {
  addressForm = this.fb.group({});
  dataDialog: {
    modalTitle: string,
    modalText: string
  };
  constructor(public dialog: MatDialog,private fb: FormBuilder) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '25em',
      data: { modalTitle, modalText }
    });   
  }
  editorStyle = {
    height: '100px',
    overflow: 'auto'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  crearFormulario(){
    return this.fb.group({
      objeto: [null, Validators.required]
    })
  }
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.openDialog('La información ha sido guardada exitosamente.', "");
  }
}
