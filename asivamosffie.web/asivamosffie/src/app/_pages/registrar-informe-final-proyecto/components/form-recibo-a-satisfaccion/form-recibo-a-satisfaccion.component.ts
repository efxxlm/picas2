import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

import { Report } from 'src/app/_interfaces/proyecto-final.model';
import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrar-informe-final-proyecto.service';
import { Respuesta } from 'src/app/core/_services/common/common.service';


@Component({
  selector: 'app-form-recibo-a-satisfaccion',
  templateUrl: './form-recibo-a-satisfaccion.component.html',
  styleUrls: ['./form-recibo-a-satisfaccion.component.scss'],
})
export class FormReciboASatisfaccionComponent implements OnInit {
  @Input() report: Report;
  estaEditando = false;
  informeFinalId = 0;
  urlActa = null;
  fechaSuscripcion = null;
  addressForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private registrarInformeFinalProyectoService: RegistrarInformeFinalProyectoService
  ) {}

  ngOnInit(): void {
    this.buildForm();
  }

  private buildForm() {
    if(this.report.informeFinal.length>0){
      if(this.report.informeFinal[0].informeFinalId != null){
        this.informeFinalId = this.report.informeFinal[0].informeFinalId;
      }
      if(this.report.informeFinal[0].urlActa != null){
        this.urlActa = this.report.informeFinal[0].urlActa;
      }
      if(this.report.informeFinal[0].fechaSuscripcion != null){
        this.fechaSuscripcion = this.report.informeFinal[0].fechaSuscripcion;
      }
    }
    this.addressForm = this.fb.group({
      InformeFinalId: [this.informeFinalId, Validators.required],
      ContratacionProyectoId: [
        this.report.contratacionProyectoId,
        Validators.required,
      ],
      UrlActa: [this.urlActa, Validators.required],
      FechaSuscripcion: [this.fechaSuscripcion, Validators.required],
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText },
    });
  }

  onSubmit() {
    // console.log(this.addressForm.value);
    this.addressForm.markAllAsTouched();
    this.estaEditando = true;
    this.createInformeFinal(this.addressForm.value);
    //this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }

  createInformeFinal( informeFinal: any ) {
    this.registrarInformeFinalProyectoService.createInformeFinal(informeFinal)
    .subscribe((respuesta: Respuesta) => {
      this.openDialog('', respuesta.message)
    });
  }
}
