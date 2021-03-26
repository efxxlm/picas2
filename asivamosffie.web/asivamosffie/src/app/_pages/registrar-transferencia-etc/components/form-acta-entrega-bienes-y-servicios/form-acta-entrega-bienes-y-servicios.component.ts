import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { RegisterProjectEtcService } from 'src/app/core/_services/registerProjectETC/register-project-etc.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ProyectoEntregaETC } from 'src/app/_interfaces/proyecto-entrega-etc';


@Component({
  selector: 'app-form-acta-entrega-bienes-y-servicios',
  templateUrl: './form-acta-entrega-bienes-y-servicios.component.html',
  styleUrls: ['./form-acta-entrega-bienes-y-servicios.component.scss']
})
export class FormActaEntregaBienesYServiciosComponent implements OnInit {

  addressForm = this.fb.group({
    fechaFirmaActaBienesServicios: [null, Validators.required],
    actaBienesServicios: [null, Validators.required]
  });

  estaEditando = false;
  @Input() id: number;
  @Output("callOnInitParent") callOnInitParent: EventEmitter<any> = new EventEmitter();

  constructor(private fb: FormBuilder, public dialog: MatDialog, private registerProjectETCService: RegisterProjectEtcService) {}

  ngOnInit(): void {
    this.buildForm();
  }

  private buildForm() {    
    this.addressForm = this.fb.group({
      proyectoEntregaEtcid: [null, Validators.required],
      informeFinalId: [this.id, Validators.required],
      fechaFirmaActaBienesServicios: [null, Validators.required],
      actaBienesServicios: [null, Validators.required]
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
    this.createEditActaBienesServicios(this.addressForm.value);
  }

  createEditActaBienesServicios(pActaServicios: any) {
    this.registerProjectETCService.createEditActaBienesServicios(pActaServicios).subscribe((respuesta: Respuesta) => {
      this.openDialog('', respuesta.message);
    });
  }
}
