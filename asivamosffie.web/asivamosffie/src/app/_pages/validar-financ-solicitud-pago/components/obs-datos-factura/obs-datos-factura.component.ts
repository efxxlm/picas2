import { Router, ActivatedRoute } from '@angular/router';
import { CommonService } from './../../../../core/_services/common/common.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { TiposDeFase } from 'src/app/_interfaces/solicitud-pago.interface';

@Component({
  selector: 'app-obs-datos-factura',
  templateUrl: './obs-datos-factura.component.html',
  styleUrls: ['./obs-datos-factura.component.scss']
})
export class ObsDatosFacturaComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() tieneObservacion: boolean;
    @Input() datosFacturaCodigo: string;
    @Input() listaMenusId: any;
    @Input() tieneObservacionOrdenGiro: boolean;
    @Input() faseCodigo: string;
    @Input() contratacionProyectoId: number;
    @Output() semaforoObservacion = new EventEmitter<boolean>();
    esPreconstruccion = true;
    addressForm = this.fb.group({
      tipoDocumento: [ null, Validators.required ],
      numeroFactura: [null, Validators.required],
      fechaFactura: [null, Validators.required],
      numeroDescuentos: [''],
      valorAPagarDespues: [{ value: null, disabled: true }],
      aplicaDescuento: [null],
      descuentos: this.fb.array([])
    });
    listaTipoDocumento: Dominio[] = [
      { nombre: 'Factura', codigo: '1' },
      { nombre: 'Cuenta de cobro', codigo: '2' }
    ];
    fasesContrato = TiposDeFase;
    valorFacturado = 0;
    tiposDescuentoArray: Dominio[] = [];
    listaTipoDescuento: Dominio[] = [];
    solicitudPagoFaseFacturaDescuento: any[] = [];
    solicitudPagoFaseFactura: any;
    solicitudPagoFase: any;
    esAutorizar: boolean;
    observacion: any;
    solicitudPagoObservacionId = 0;
    estaEditando = false;
    minDate = new Date();
  
    get descuentos() {
      return this.addressForm.get('descuentos') as FormArray;
    }
  
    constructor(
      private fb: FormBuilder,
      private commonSvc: CommonService
    ) {}
  
    ngOnInit(): void {
      this.getDatosFactura();
    }
  
    async getDatosFactura() {
      this.tiposDescuentoArray = await this.commonSvc.tiposDescuento().toPromise();
      this.listaTipoDescuento = [...this.tiposDescuentoArray];
      const solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0];
  
      // Verificar la fase seleccionada en el proyecto
      if ( this.faseCodigo === this.fasesContrato.construccion ) {
        this.esPreconstruccion = false;
      }
  
      if ( this.esPreconstruccion === true && solicitudPagoRegistrarSolicitudPago !== undefined ) {
        if ( solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined ) {
          if ( solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
            for (const solicitudPagoFase of solicitudPagoRegistrarSolicitudPago.solicitudPagoFase) {
              if (solicitudPagoFase.esPreconstruccion === true && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId) {
                this.solicitudPagoFase = solicitudPagoFase;
              }
            }
          }
        }
      }
  
      if ( this.esPreconstruccion === false && solicitudPagoRegistrarSolicitudPago !== undefined ) {
        if ( solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined ) {
          if ( solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
            for (const solicitudPagoFase of solicitudPagoRegistrarSolicitudPago.solicitudPagoFase) {
              if (solicitudPagoFase.esPreconstruccion === false && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId) {
                this.solicitudPagoFase = solicitudPagoFase;
              }
            }
          }
        }
      }
  
      if ( solicitudPagoRegistrarSolicitudPago !== undefined ) {
        if ( this.solicitudPagoFase !== undefined ) {
          if ( this.solicitudPagoFase.tieneDescuento !== undefined ) {
            this.addressForm.get( 'aplicaDescuento' ).setValue( this.solicitudPagoFase.tieneDescuento )
          }
          this.solicitudPagoFaseFacturaDescuento = this.solicitudPagoFase.solicitudPagoFaseFacturaDescuento;
          if (this.solicitudPagoFaseFacturaDescuento !== undefined) {
            this.estaEditando = true;
            this.addressForm.markAllAsTouched();
  
            for (const descuento of this.solicitudPagoFaseFacturaDescuento) {
              this.descuentos.markAllAsTouched();
              this.descuentos.push(
                this.fb.group({
                  solicitudPagoFaseFacturaDescuentoId: [descuento.solicitudPagoFaseFacturaDescuentoId],
                  solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                  tipoDescuentoCodigo: [descuento.tipoDescuentoCodigo],
                  valorDescuento: [descuento.valorDescuento]
                })
              );
            }
          }
  
          for (const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio) {
            this.valorFacturado += criterio.valorFacturado;
          }
  
          if ( this.solicitudPagoFaseFacturaDescuento !== undefined && this.solicitudPagoFaseFacturaDescuento.length > 0 ) {
            let totalDescuentos = 0;
            let valorDespuesDescuentos = 0;
  
            this.descuentos.controls.forEach(control => {
              if (control.value.valorDescuento !== null) {
                totalDescuentos += control.value.valorDescuento;
              }
            });
  
            valorDespuesDescuentos = this.valorFacturado - totalDescuentos;
            this.addressForm.get( 'numeroDescuentos' ).setValue( `${ this.solicitudPagoFaseFacturaDescuento.length }` )
            this.addressForm.get('valorAPagarDespues').setValue( valorDespuesDescuentos );
          }
  
          this.addressForm.get('numeroDescuentos').valueChanges.subscribe(value => {
            value = Number(value);
            if (value <= this.listaTipoDescuento.length) {
              if (this.solicitudPagoFaseFacturaDescuento !== undefined && this.solicitudPagoFaseFacturaDescuento.length > 0) {
                if (value > 0) {
                  this.descuentos.clear();
                  for (const descuento of this.solicitudPagoFaseFacturaDescuento) {
                    this.estaEditando = true;
                    this.descuentos.markAllAsTouched();
                    this.descuentos.push(
                      this.fb.group({
                        solicitudPagoFaseFacturaDescuentoId: [descuento.solicitudPagoFaseFacturaDescuentoId],
                        solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                        tipoDescuentoCodigo: [descuento.tipoDescuentoCodigo],
                        valorDescuento: [descuento.valorDescuento]
                      })
                    );
                  }
      
                  this.addressForm.get('numeroDescuentos').setValidators(Validators.min(this.descuentos.length));
                  const nuevosDescuentos = value - this.descuentos.length;
                  if (value < this.descuentos.length) {
                    this.addressForm.get('numeroDescuentos').setValue(String(this.descuentos.length));
                    return;
                  }
                  for (let i = 0; i < nuevosDescuentos; i++) {
                    this.estaEditando = true;
                    this.descuentos.markAllAsTouched();
                    this.descuentos.push(
                      this.fb.group({
                        solicitudPagoFaseFacturaDescuentoId: [0],
                        solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                        tipoDescuentoCodigo: [null],
                        valorDescuento: [null]
                      })
                    );
                  }
                }
              } else {
                if (value > 0) {
                  if (this.descuentos.dirty === true) {
                    this.addressForm.get('numeroDescuentos').setValidators(Validators.min(this.descuentos.length));
                    const nuevosDescuentos = value - this.descuentos.length;
                    if (value < this.descuentos.length) {
                      this.addressForm.get('numeroDescuentos').setValue(String(this.descuentos.length));
                      return;
                    }
                    for (let i = 0; i < nuevosDescuentos; i++) {
                      this.estaEditando = true;
                      this.descuentos.markAllAsTouched();
                      this.descuentos.push(
                        this.fb.group({
                          solicitudPagoFaseFacturaDescuentoId: [0],
                          solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                          tipoDescuentoCodigo: [null],
                          valorDescuento: [null]
                        })
                      );
                    }
                  }
                  if (this.descuentos.dirty === false) {
                    this.descuentos.clear();
                    for (let i = 0; i < value; i++) {
                      this.estaEditando = true;
                      this.descuentos.markAllAsTouched();
                      this.descuentos.push(
                        this.fb.group({
                          solicitudPagoFaseFacturaDescuentoId: [0],
                          solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                          tipoDescuentoCodigo: [null],
                          valorDescuento: [null]
                        })
                      );
                    }
                  }
                }
              }
            } else {
              this.addressForm.get('numeroDescuentos').setValue(String(this.listaTipoDescuento.length));
            }
          });
        }
      }
    }

    getTipoDescuento(tipoDescuentoCodigo: string) {
        if (this.tiposDescuentoArray.length > 0) {
          const descuento = this.tiposDescuentoArray.filter(descuento => descuento.codigo === tipoDescuentoCodigo);
          return descuento[0].nombre;
        }
    }

}
