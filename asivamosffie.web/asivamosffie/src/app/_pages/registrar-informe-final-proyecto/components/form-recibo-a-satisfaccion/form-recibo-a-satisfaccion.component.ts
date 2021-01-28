import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

import { Report } from 'src/app/_interfaces/proyecto-final.model';
import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrar-informe-final-proyecto.service';

@Component({
  selector: 'app-form-recibo-a-satisfaccion',
  templateUrl: './form-recibo-a-satisfaccion.component.html',
  styleUrls: ['./form-recibo-a-satisfaccion.component.scss'],
})
export class FormReciboASatisfaccionComponent implements OnInit {
  @Input() report: Report;
  estaEditando = false;

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
    this.addressForm = this.fb.group({
      InformeFinalId: [this.report.contratacionProyectoId, Validators.required],
      ContratacionProyectoId: [
        this.report.contratacionProyectoId,
        Validators.required,
      ],
      UrlActa: [null, Validators.required],
      FechaSuscripcion: [null, Validators.required],
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
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }

  createInformeFinal( informeFinal: any ) {
    this.registrarInformeFinalProyectoService.createInformeFinal(informeFinal)
    .subscribe(respuesta => {
      this.openDialog('', `<b>${respuesta}</b>`)
    });
  }
}
