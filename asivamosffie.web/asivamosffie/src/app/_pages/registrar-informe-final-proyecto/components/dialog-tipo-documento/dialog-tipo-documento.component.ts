import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrar-informe-final-proyecto.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
@Component({
  selector: 'app-dialog-tipo-documento',
  templateUrl: './dialog-tipo-documento.component.html',
  styleUrls: ['./dialog-tipo-documento.component.scss']
})
export class DialogTipoDocumentoComponent implements OnInit {

  estaEditando = false;
  addressForm = this.fb.group({
    informeFinalAnexoId: [null, Validators.required],
    tipoAnexo: [null, Validators.required],
    URLSoporte: [null, Validators.required],
    numRadicadoSac: [null, Validators.required],
    fechaRadicado: [null, Validators.required]
  });

  tipoAnexoArray = [
    { name: 'Físico', value: '1' },
    { name: 'Digital', value: '2' }
  ];

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private registrarInformeFinalProyectoService: RegistrarInformeFinalProyectoService,
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
      console.log(this.addressForm.value,this.data.informe.informeFinalInterventoriaId, this.data.informe.informeFinalAnexoId);
      this.estaEditando = true;
      if(this.data.informe.informeFinalAnexoId != null){
        this.addressForm.value.informeFinalAnexoId = this.data.informe.informeFinalAnexoId;
      }
      this.createEditInformeFinalAnexo(this.addressForm.value,this.data.informe.informeFinalInterventoriaId);
    }

    createEditInformeFinalAnexo( informeFinalAnexo: any , informeFinalInterventoriaid: number) {
      this.registrarInformeFinalProyectoService.createEditInformeFinalAnexo(informeFinalAnexo,informeFinalInterventoriaid)
      .subscribe((respuesta: Respuesta) => {
        this.openDialog('', respuesta.message)
      });
    }

}
