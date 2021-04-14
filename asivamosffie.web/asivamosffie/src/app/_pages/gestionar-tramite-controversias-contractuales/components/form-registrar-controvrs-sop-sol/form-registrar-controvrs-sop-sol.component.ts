import { Component, EventEmitter, Input, OnInit, Output, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-registrar-controvrs-sop-sol',
  templateUrl: './form-registrar-controvrs-sop-sol.component.html',
  styleUrls: ['./form-registrar-controvrs-sop-sol.component.scss']
})
export class FormRegistrarControvrsSopSolComponent implements OnInit, OnDestroy {

  @Input() isEditable;
  @Input() idControversia;
  @Output() estadoSemaforo1 = new EventEmitter<string>();
  addressForm = this.fb.group({
    urlSoporte: [null, Validators.required]
  });
  realizoPeticion: boolean = false;
  estaEditando = false;
  constructor(private router: Router, private fb: FormBuilder, public dialog: MatDialog, private services: ContractualControversyService) { }

  ngOnInit(): void {
    if (this.isEditable == true) {
      this.estaEditando = true;
      this.addressForm.markAllAsTouched();
      this.services.GetControversiaContractualById(this.idControversia).subscribe((resp: any) => {
        console.log(resp.rutaSoporte);
        this.addressForm.get('urlSoporte').setValue(resp.rutaSoporte);
        this.loadSemaforo();
      });
    }
  }
  ngOnDestroy(): void {
    if (this.addressForm.dirty === true && this.realizoPeticion === false) {
      this.openDialogConfirmar('', '¿Desea guardar la información registrada?');
    }
  }
  openDialogConfirmar(modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    confirmarDialog.afterClosed()
      .subscribe(response => {
        if (response === true) {
          this.onSubmit();
        }
      });
  };
  loadSemaforo() {

    if (this.addressForm.value.urlSoporte != null) {  
      this.estadoSemaforo1.emit('completo');
    }
    if (this.addressForm.value.urlSoporte != "") {  
      this.estadoSemaforo1.emit('completo');
    }
    if (this.addressForm.value.urlSoporte == null) {
      this.estadoSemaforo1.emit('sin-diligenciar');
    }
    if (this.addressForm.value.urlSoporte == "") {
      this.estadoSemaforo1.emit('sin-diligenciar');
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    // console.log(this.addressForm.value);
    this.services.ActualizarRutaSoporteControversiaContractual(this.idControversia, this.addressForm.value.urlSoporte).subscribe(resp => {
      if (resp.isSuccessful == true) {
        this.openDialog('', 'La información ha sido guardada exitosamente.');
        if (this.isEditable == true) {
          this.realizoPeticion = true;
          this.router.navigateByUrl('/', { skipLocationChange: true }).then(
            () => this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarControversia', this.idControversia]));  
        }
        else {
          this.realizoPeticion = true;
          this.router.navigateByUrl('/', { skipLocationChange: true }).then(
            () => this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarControversia', this.idControversia]));  
        }
      }
      else {
        this.openDialog('', resp.message);
      }
    });
  }
}
