import { Router } from '@angular/router';
import { Component, Input, OnInit, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';

@Component({
  selector: 'app-form-amortizacion-anticipo',
  templateUrl: './form-amortizacion-anticipo.component.html',
  styleUrls: ['./form-amortizacion-anticipo.component.scss']
})
export class FormAmortizacionAnticipoComponent implements OnInit, OnChanges {
  @Input() solicitudPago: any;
  @Input() esVerDetalle = false;
  @Input() contrato: any;
  @Input() tieneObservacion: boolean;
  @Input() listaMenusId: any;
  @Input() amortizacionAnticipoCodigo: string;
  @Input() tieneObservacionOrdenGiro: boolean;
  @Output() semaforoObservacion = new EventEmitter<boolean>();
  esPreconstruccion = false;
  solicitudPagoFase: any;
  solicitudPagoFaseAmortizacionId = 0;
  valorTotalDelContrato = 0;
  esAutorizar: boolean;
  observacion: any;
  solicitudPagoObservacionId = 0;
  addressForm = this.fb.group({
    porcentajeAmortizacion: [null, Validators.required],
    valorAmortizacion: [{ value: null, disabled: true }, Validators.required]
  });
  estaEditando = false;
  constructor(
    private fb: FormBuilder,
    private routes: Router,
    private dialog: MatDialog,
    private obsMultipleSvc: ObservacionesMultiplesCuService,
    private registrarPagosSvc: RegistrarRequisitosPagoService
  ) {
    this.addressForm.get('porcentajeAmortizacion').valueChanges.subscribe(value => {
      if (this.valorTotalDelContrato > 0) {
        const exampleValue = this.valorTotalDelContrato * 0.2;
        const porcentajeCalculo = value / 100;
        const valorAmortizacion = exampleValue * porcentajeCalculo;

        if (valorAmortizacion > this.contrato.vContratoPagosRealizados[0].valorFacturado) {
          this.openDialog('', `El valor de amortización no puede ser mayor al valor facturado`);
          this.addressForm.get('porcentajeAmortizacion').setValue('');
        } else {
          this.addressForm.get('valorAmortizacion').setValue(valorAmortizacion);
        }
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.esVerDetalle === false) {
      if (changes.tieneObservacion.currentValue === true) {
        this.addressForm.get('porcentajeAmortizacion').enable();
      }
    }
  }

  ngOnInit(): void {
    if (this.contrato.contratacion.disponibilidadPresupuestal.length > 0) {
      this.contrato.contratacion.disponibilidadPresupuestal.forEach(
        ddp => (this.valorTotalDelContrato += ddp.valorSolicitud)
      );
    }

    if (this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0) {
      for (const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase) {
        if (solicitudPagoFase.esPreconstruccion === false) {
          this.solicitudPagoFase = solicitudPagoFase;
        }
      }
    }

    if (this.solicitudPagoFase.solicitudPagoFaseAmortizacion.length > 0) {
      const solicitudPagoFaseAmortizacion = this.solicitudPagoFase.solicitudPagoFaseAmortizacion[0];
      this.solicitudPagoFaseAmortizacionId = solicitudPagoFaseAmortizacion.solicitudPagoFaseAmortizacionId;
      this.estaEditando = true;
      if (solicitudPagoFaseAmortizacion.registroCompleto === true) {
        this.addressForm.disable();
      }
      if (this.tieneObservacionOrdenGiro !== undefined) {
        this.addressForm.get('porcentajeAmortizacion').enable();
      }
      this.addressForm.markAllAsTouched();
      this.addressForm.setValue({
        porcentajeAmortizacion:
          solicitudPagoFaseAmortizacion.porcentajeAmortizacion !== undefined
            ? solicitudPagoFaseAmortizacion.porcentajeAmortizacion
            : null,
        valorAmortizacion:
          solicitudPagoFaseAmortizacion.valorAmortizacion !== undefined
            ? solicitudPagoFaseAmortizacion.valorAmortizacion
            : null
      });

      if (this.esVerDetalle === false) {
        // Get observacion CU autorizar solicitud de pago 4.1.9
        this.obsMultipleSvc
          .getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
            this.listaMenusId.autorizarSolicitudPagoId,
            this.contrato.solicitudPagoOnly.solicitudPagoId,
            this.solicitudPagoFaseAmortizacionId,
            this.amortizacionAnticipoCodigo
          )
          .subscribe(response => {
            const observacion = response.find(obs => obs.archivada === false);
            if (observacion !== undefined) {
              this.esAutorizar = true;
              this.observacion = observacion;

              if (this.observacion.tieneObservacion === true) {
                this.semaforoObservacion.emit(true);
                this.addressForm.get('porcentajeAmortizacion').enable();
                this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
              }
            }
          });

        // Get observacion CU verificar solicitud de pago 4.1.8
        this.obsMultipleSvc
          .getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
            this.listaMenusId.aprobarSolicitudPagoId,
            this.contrato.solicitudPagoOnly.solicitudPagoId,
            this.solicitudPagoFaseAmortizacionId,
            this.amortizacionAnticipoCodigo
          )
          .subscribe(response => {
            const observacion = response.find(obs => obs.archivada === false);
            if (observacion !== undefined) {
              this.esAutorizar = false;
              this.observacion = observacion;

              if (this.observacion.tieneObservacion === true) {
                this.semaforoObservacion.emit(true);
                this.addressForm.get('porcentajeAmortizacion').enable();
                this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
              }
            }
          });
      }
    }
  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  numberValidate(value: any) {
    if (value > 100) {
      this.addressForm.get('porcentajeAmortizacion').setValue(100);
    }
    if (value < 0) {
      this.addressForm.get('porcentajeAmortizacion').setValue(0);
    }
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    const solicitudPagoFaseAmortizacion = [
      {
        solicitudPagoFaseAmortizacionId: this.solicitudPagoFaseAmortizacionId,
        solicitudPagoFaseId: this.solicitudPagoFase.solicitudPagoFaseId,
        porcentajeAmortizacion: this.addressForm.get('porcentajeAmortizacion').value,
        valorAmortizacion: this.addressForm.get('valorAmortizacion').value
      }
    ];

    for (const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase) {
      if (solicitudPagoFase.esPreconstruccion === false) {
        solicitudPagoFase.solicitudPagoFaseAmortizacion = solicitudPagoFaseAmortizacion;
      }
    }

    this.registrarPagosSvc.createEditNewPayment(this.solicitudPago).subscribe(
      response => {
        this.openDialog('', `<b>${response.message}</b>`);
        if (this.observacion !== undefined) {
          this.observacion.archivada = !this.observacion.archivada;
          this.obsMultipleSvc.createUpdateSolicitudPagoObservacion(this.observacion).subscribe();
        }
        this.registrarPagosSvc.getValidateSolicitudPagoId(this.solicitudPago.solicitudPagoId).subscribe(() => {
          this.routes
            .navigateByUrl('/', { skipLocationChange: true })
            .then(() =>
              this.routes.navigate([
                '/registrarValidarRequisitosPago/verDetalleEditar',
                this.solicitudPago.contratoId,
                this.solicitudPago.solicitudPagoId
              ])
            );
        });
      },
      err => this.openDialog('', `<b>${err.message}</b>`)
    );
  }
}
