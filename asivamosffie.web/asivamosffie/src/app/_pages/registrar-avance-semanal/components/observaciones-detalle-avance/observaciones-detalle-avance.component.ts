import { Component, Input, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { GuardadoParcialAvanceSemanalService } from 'src/app/core/_services/guardadoParcialAvanceSemanal/guardado-parcial-avance-semanal.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-observaciones-detalle-avance',
  templateUrl: './observaciones-detalle-avance.component.html',
  styleUrls: ['./observaciones-detalle-avance.component.scss']
})
export class ObservacionesDetalleAvanceComponent implements OnInit {
  @Input() seguimientoSemanal: any;
  @Input() isRequired: boolean;


  formObservaciones: FormGroup = this.fb.group({
    // tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required]
  });

  editorStyle = {
    height: '100px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }]
    ]
  };
  estaEditando = false;
  seguimientoSemanalId: number;

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private avanceSemanalSvc: RegistrarAvanceSemanalService,
    private routes: Router,
    private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
    private guardadoParcialAvanceSemanalSvc: GuardadoParcialAvanceSemanalService
  ) {}

  ngOnInit(): void {
    // console.log('seguimientoSemanal: ', this.seguimientoSemanal);
    this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;

    this.formObservaciones
      .get('observaciones')
      .setValue(this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0] ? this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0].observaciones: null);

    if (!this.isRequired) this.formObservaciones.get('observaciones').clearValidators();
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n - 1, e.editor.getLength());
    }
  }

  textoLimpio(evento: any, n: number) {
    if (evento !== undefined) {
      return evento.getLength() > n ? n : evento.getLength();
    } else {
      return 0;
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.formObservaciones.value);
    this.estaEditando = true;

    let pSeguimientoSemanal = this.seguimientoSemanal;

    console.log(pSeguimientoSemanal.seguimientoSemanalAvanceFisico);

    if (pSeguimientoSemanal.seguimientoSemanalAvanceFisico.length > 0) {
      pSeguimientoSemanal.seguimientoSemanalAvanceFisico[0].observaciones =
        this.formObservaciones.get('observaciones').value;
    } else {
      pSeguimientoSemanal.seguimientoSemanalAvanceFisico.push({
        seguimientoSemanalId: this.seguimientoSemanalId,
        observaciones: this.formObservaciones.get('observaciones').value
      });
    }

    this.avanceSemanalSvc.saveUpdateSeguimientoSemanal(pSeguimientoSemanal).subscribe(
      async response => {
        this.openDialog('', `<b>${response.message}</b>`);
        this.routes
          .navigateByUrl('/', { skipLocationChange: true })
          .then(() =>
            this.routes.navigate([
              '/registrarAvanceSemanal/registroSeguimientoSemanal',
              this.seguimientoSemanal.contratacionProyectoId
            ])
          );
      },
      err => this.openDialog('', `<b>${err.message}</b>`)
    );
  }
}
