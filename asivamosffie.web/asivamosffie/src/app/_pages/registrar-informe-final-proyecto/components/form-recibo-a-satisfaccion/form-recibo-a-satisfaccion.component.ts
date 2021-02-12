import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

import { Report } from 'src/app/_interfaces/proyecto-final.model';
import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrarInformeFinal/registrar-informe-final-proyecto.service'
import { Respuesta } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-form-recibo-a-satisfaccion',
  templateUrl: './form-recibo-a-satisfaccion.component.html',
  styleUrls: ['./form-recibo-a-satisfaccion.component.scss']
})
export class FormReciboASatisfaccionComponent implements OnInit {
  @Input() report: Report;
  @Output() formCompleto = new EventEmitter<boolean>(true);
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
    this.addressForm = this.fb.group({
      informeFinalId: [null, Validators.required],
      proyectoId: [this.report.proyecto.proyectoId, Validators.required],
      urlActa: [null, Validators.required],
      fechaSuscripcion: [null, Validators.required]
    });
    if (this.report.proyecto.informeFinal.length > 0) {
      this.addressForm.patchValue(this.report.proyecto.informeFinal[0]);
      this.estaEditando = true;
    }
    this.formCompleto.emit(this.respuestaFormCompleto());
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    //[disabled]="addressForm.invalid"
    //console.log(this.addressForm.value);
    this.addressForm.markAllAsTouched();
    this.estaEditando = true;
    this.createInformeFinal(this.addressForm.value);
    //this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }

  createInformeFinal(informeFinal: any) {
    this.registrarInformeFinalProyectoService.createInformeFinal(informeFinal).subscribe((respuesta: Respuesta) => {
      this.openDialog('', respuesta.message);
      this.formCompleto.emit(this.respuestaFormCompleto());
    });
  }

  respuestaFormCompleto() {
    if (this.addressForm.get('urlActa').valid) return true;
    else if (this.addressForm.get('fechaSuscripcion').valid) return true;
    else return false;
  }
}
