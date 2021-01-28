import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
@Component({
  selector: 'app-dialog-tipo-documento',
  templateUrl: './dialog-tipo-documento.component.html',
  styleUrls: ['./dialog-tipo-documento.component.scss']
})
export class DialogTipoDocumentoComponent implements OnInit {

  estaEditando = false;
  addressForm = this.fb.group({
    tipoDeAnexo: [null, Validators.required],
    URLSoporte: [null, Validators.required],
    numeroRadicadoSAC: [null, Validators.required],
    fechaRadicado: [null, Validators.required]
  });

  tipoAnexoArray = [
    { name: 'Físico', value: '1' },
    { name: 'Digital', value: '2' }
  ];

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data
  ) {}

  ngOnInit(): void {}

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '40em',
          data : { modalTitle, modalText }
        });
    }

    onSubmit() {
      console.log(this.addressForm.value, "Acá se supone:",this.data.informe);
      this.estaEditando = true;
      this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
    }

}
