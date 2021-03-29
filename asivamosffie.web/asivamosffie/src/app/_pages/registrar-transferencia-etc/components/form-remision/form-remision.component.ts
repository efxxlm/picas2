import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { RegisterProjectEtcService } from 'src/app/core/_services/registerProjectETC/register-project-etc.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ProyectoEntregaETC } from 'src/app/_interfaces/proyecto-entrega-etc';

@Component({
  selector: 'app-form-remision',
  templateUrl: './form-remision.component.html',
  styleUrls: ['./form-remision.component.scss']
})
export class FormRemisionComponent implements OnInit {
  @Input() proyectoEntregaEtc: ProyectoEntregaETC;
  @Input() id: number;

  @Output("callOnInitParent") callOnInitParent: EventEmitter<any> = new EventEmitter();

  addressForm = this.fb.group({
    fechaEntregaDocumentosEtc: [null, Validators.required],
    numRadicadoDocumentosEntregaEtc: [null, Validators.compose([Validators.required, Validators.maxLength(10)])]
  });

  estaEditando = false;

  constructor(private fb: FormBuilder, public dialog: MatDialog,private registerProjectETCService: RegisterProjectEtcService) {}

  ngOnInit(): void {
    this.buildForm();
  }

  private buildForm() {    
    this.addressForm = this.fb.group({
      proyectoEntregaEtcid: [null, Validators.required],
      informeFinalId: [this.id, Validators.required],
      fechaEntregaDocumentosEtc: [null, Validators.required],
      numRadicadoDocumentosEntregaEtc: [null, Validators.required]
    });

    if (this.id != null) {
      this.registerProjectETCService.getProyectoEntregaEtc(this.id)
      .subscribe(
        response => {
          if(response != null){
            this.addressForm.patchValue(response);
          }
        }
      );
      this.addressForm.markAllAsTouched();
      this.estaEditando = true;
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.callOnInitParent.emit();
      return;
    });
  }

  onSubmit() {
    this.addressForm.markAllAsTouched();
    this.estaEditando = true;
    this.addressForm.value.informeFinalId = this.id;
    this.createEditRemisionDocumentosTecnicos(this.addressForm.value);
  }

  createEditRemisionDocumentosTecnicos(pDocumentos: any) {
    this.registerProjectETCService.createEditRemisionDocumentosTecnicos(pDocumentos).subscribe((respuesta: Respuesta) => {
      this.openDialog('', respuesta.message);
    });
  }
}
