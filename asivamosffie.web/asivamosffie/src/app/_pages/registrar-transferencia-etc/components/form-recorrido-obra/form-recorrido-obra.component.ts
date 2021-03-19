import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { RegisterProjectEtcService } from 'src/app/core/_services/registerProjectETC/register-project-etc.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ProyectoEntregaETC } from 'src/app/_interfaces/proyecto-entrega-etc';

@Component({
  selector: 'app-form-recorrido-obra',
  templateUrl: './form-recorrido-obra.component.html',
  styleUrls: ['./form-recorrido-obra.component.scss']
})
export class FormRecorridoObraComponent implements OnInit {
  addressForm = this.fb.group({
    fechaRecorridoObra: [null, Validators.required],
    numRepresentantesRecorrido: [null, Validators.compose([Validators.required, Validators.maxLength(2)])],
    fechaFirmaActaEngregaFisica: [null, Validators.required],
    urlActaEntregaFisica: [null, Validators.required]
  });

  @Input() id: number;

  estaEditando = false;

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  constructor(private fb: FormBuilder, public dialog: MatDialog, private registerProjectETCService: RegisterProjectEtcService) {}

  ngOnInit(): void {
    this.buildForm();
  }

  private buildForm() {    
    this.addressForm = this.fb.group({
      proyectoEntregaETCId: [null, Validators.required],
      informeFinalId: [this.id, Validators.required],
      fechaRecorridoObra: [null, Validators.required],
      numRepresentantesRecorrido: [null, Validators.compose([Validators.required, Validators.maxLength(2)])],
      fechaFirmaActaEngregaFisica: [null, Validators.required],
      urlActaEntregaFisica: [null, Validators.required]
    });

    if (this.id != null) {
      this.registerProjectETCService.getProyectoEntregaEtc(this.id)
      .subscribe(
        response => {
          this.addressForm.patchValue(response);
        }
      );
      this.estaEditando = true;
    }
  }


  arrayOne(n: number): any[] {
    return Array(n);
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    this.addressForm.markAllAsTouched();
    this.estaEditando = true;
    this.addressForm.value.informeFinalId = this.id;
    this.createEditRecorridoObra(this.addressForm.value);
  }

  createEditRecorridoObra(pRecorrido: any) {
    this.registerProjectETCService.createEditRecorridoObra(pRecorrido).subscribe((respuesta: Respuesta) => {
      this.openDialog('', respuesta.message);
    });
  }
}
